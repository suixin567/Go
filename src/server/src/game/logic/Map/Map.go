// Map
package Map

import (
	"ace"
	"encoding/json"
	"fmt"
	"game/data"
	"game/logic/protocol"
)

const (
	ENTER_CREQ = 0
	ENTER_SRES = 1
	ENTER_BRO  = 2
	MOVE_CREQ  = 3
	MOVE_BRO   = 4
	LEAVE_CREQ = 5
	LEAVE_BRO  = 6
)

type EnterMapDTO struct {
	Name     string
	Map      int
	Point    data.Vector3
	Rotation data.Vector4
}

type MoveDTO struct {
	Name     string
	Dir      int
	Point    data.Vector3
	Rotation data.Vector4
}

type MapManager struct {
	Maps map[int]*MapHandler
}

type MapHandler struct {
	Area  int
	Roles map[*ace.Session]string //根据session获得此地图中的角色名字
}

var Manager = &MapManager{make(map[int]*MapHandler)}

func init() {
	var i int = 0
	for ; i < 10; i += 1 {
		fmt.Println("创建地图", i)
		Manager.Maps[i] = &MapHandler{i, make(map[*ace.Session]string)}
	}

}

func (this *MapManager) Process(session *ace.Session, model ace.DefaultSocketModel) {
	//收到角色传来的地图消息  根据传输消息中的区域码来获取 对应的地图模块 调用该模块的消息处理函数
	var area = model.Area
	this.Maps[area].Process(session, model)
}

func (this *MapHandler) Process(session *ace.Session, model ace.DefaultSocketModel) {
	switch model.Command {
	case ENTER_CREQ: //收到用户申请进入地图
		this.enter(session, model)
		//fmt.Println("用户申请进入地图 area", model.Area)
		break
	case MOVE_CREQ: //用户移动
		this.move(session, model)
		break
	}
}

func (this *MapHandler) enter(session *ace.Session, message ace.DefaultSocketModel) {
	enterData := &EnterMapDTO{}
	err := json.Unmarshal(message.Message, &enterData)
	if err != nil {
		fmt.Println(err)
	}
	fmt.Println("进入信息", enterData.Name, enterData.Map, enterData.Point, enterData.Rotation)
	//获得角色对象，修改其值。 并将此角色加入本地图
	enterPlayer := data.SyncAccount.SessionPlayer[session]
	enterPlayer.Map = enterData.Map
	enterPlayer.Point = enterData.Point
	enterPlayer.Rotation = enterData.Rotation
	this.Roles[session] = enterData.Name //凡是进入此地图，都要把session上交列表，以后广播使用
	m, _ := json.Marshal(enterPlayer)    //把这个角色转成Json
	fmt.Println("进入地图的人是：", string(m))
	//将进入地图的角色信息发送给已经在这个场景的所有人
	this.exBrocast(session, ENTER_BRO, m)
	//获取场景内所有人信息发送给进入场景的用户
	ms, _ := json.Marshal(this.getRoles())

	fmt.Println("此地图里的所有人：", string(ms))
	this.Write(session, ENTER_SRES, ms)
}

func (this *MapHandler) move(session *ace.Session, model ace.DefaultSocketModel) {
	//获取用户移动消息体
	var moveData MoveDTO
	err := json.Unmarshal(model.Message, &moveData)
	if err != nil {
		fmt.Println("err", err)
	}
	//fmt.Println("移动信息：", moveData.Name, moveData.Point)
	//获得角色对象
	var movePlayer *data.PlayerDTO
	movePlayer = data.SyncAccount.SessionPlayer[session]
	//更新人物信息里的位置信息
	movePlayer.Point = moveData.Point
	movePlayer.Rotation = moveData.Rotation
	//将移动消息转发给场景所有人
	message, _ := json.Marshal(moveData)
	this.exBrocast(session, MOVE_BRO, message) //除了自己的广播
}

func (this *MapHandler) getRoles() []*data.PlayerDTO {
	var players []*data.PlayerDTO
	for s, _ := range this.Roles {
		dto := data.SyncAccount.SessionPlayer[s]
		players = append(players, dto)
	}
	return players
}

func (this *MapHandler) Write(session *ace.Session, command int, message []byte) {
	session.Write(&ace.DefaultSocketModel{protocol.MAP, this.Area, command, message})
}

func (this *MapHandler) exBrocast(session *ace.Session, command int, message []byte) {
	for s, _ := range this.Roles {
		if s != session {
			this.Write(s, command, message)
		}
	}
}

func (this *MapHandler) brocast(command int, message []byte) {
	for s, _ := range this.Roles {
		this.Write(s, command, message)
	}
}

//下线处理
func (this *MapManager) SessionClose(session *ace.Session) {
	//根据session得到角色名字,但一定先判断
	_, ok := data.SyncAccount.SessionPlayer[session]
	if ok {
		leavePlayer := data.SyncAccount.SessionPlayer[session]
		this.Maps[leavePlayer.Map].SessionClose(session, &leavePlayer.Name)
	}
}

func (this *MapHandler) SessionClose(session *ace.Session, leaveName *string) {
	delete(this.Roles, session)
	this.brocast(LEAVE_BRO, []byte(*leaveName)) //告诉所有在这个地图的玩家，这个人离开了
}
