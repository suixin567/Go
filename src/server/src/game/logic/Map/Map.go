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
	ENTER_CREQ        = 0
	ENTER_SRES        = 1
	ENTER_BRO         = 2
	MOVE_CREQ         = 3
	MOVE_BRO          = 4
	LEAVE_CREQ        = 5
	LEAVE_BRO         = 6
	MONSTER_INIT_SRES = 9  //服务器为一个客户端初始化怪物
	ATTACK_MON_CREQ   = 12 //客户端发起攻击怪物
	ATTACK_MON_BRO    = 13 //攻击怪物的广播
	MOSTER_RELIVE_BRO = 11 //怪物复活

	ATTACK_PLAYER_CREQ = 14 //攻击人物
	ATTACK_PLAYER_BRO  = 15 //攻击人物的广播
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

//攻击怪物
type AttackMonDTO struct {
	Player      string
	TarPlayer   string
	FirstIndex  int
	SecondIndex int
	Skill       int
}

////攻击角色
//type AttackPlayerDTO struct {
//	Player    string
//	TarPlayer string
//	Skill     int
//}

type MapManager struct {
	Maps map[int]*MapHandler
}

type MapHandler struct {
	Area  int
	Roles map[*ace.Session]string //根据session获得此地图中的角色名字
	//Name_Player map[string]*data.PlayerDTO //根据一个角色名字获得角色模型
	MonGens []*MonGenDTO //每个地图管理员有多个刷怪管理器
}

var Manager = &MapManager{make(map[int]*MapHandler)}

func init() {
	var i int = 0
	for ; i < 10; i += 1 {
		fmt.Println("创建地图", i)
		Manager.Maps[i] = &MapHandler{i, make(map[*ace.Session]string), nil}
	}
	InitGenMon()
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
	case ATTACK_MON_CREQ: //客户端发起攻击怪物
		go this.attackMon(session, model)
		break
		//	case ATTACK_PLAYER_CREQ: //攻击角色
		//		this.attackPlayer(session, model)
		//		break
	default:
		fmt.Println("未知的地图协议")
		break
	}
}

//func (this *MapHandler) attackPlayer(session *ace.Session, message ace.DefaultSocketModel) {
//	attData := &AttackPlayerDTO{}
//	err := json.Unmarshal(message.Message, &attData)
//	if err != nil {
//		fmt.Println(err)
//	}

//}

func (this *MapHandler) attackMon(session *ace.Session, message ace.DefaultSocketModel) {
	attData := &AttackMonDTO{}
	err := json.Unmarshal(message.Message, &attData)
	if err != nil {
		fmt.Println(err)
	}
	if attData.TarPlayer == "" { //******************************************攻击怪物
		//fmt.Println("就是这个怪", this.MonGens[monData.FirstIndex].Monsters[monData.SecondIndex].Name)
		mon := this.MonGens[attData.FirstIndex].Monsters[attData.SecondIndex]
		if mon.Hp <= 0 { //必须是活着的怪，死怪不需要继续被掉血
			return
		}
		player := data.SyncAccount.SessionPlayer[session]
		mon.Hp -= player.Atk
		if mon.Hp <= 0 { //怪物被杀死
			this.MonGens[attData.FirstIndex].currentAmount--
		}
		//广播人物、怪物 和人物技能
		attData.Player = player.Name
		m, _ := json.Marshal(*attData)
		fmt.Println("广播打怪物", string(m))
		this.exBrocast(session, ATTACK_MON_BRO, m)
	} else { //****************************************************攻击人物
		player := data.SyncAccount.SessionPlayer[session]
		tarPlayer := &data.PlayerDTO{}
		for s, name := range this.Roles { //遍历此地图内的所有角色
			if name == attData.TarPlayer {
				tarPlayer = data.SyncAccount.SessionPlayer[s]
			}
		}
		tarPlayer.Hp -= player.Atk
		if tarPlayer.Hp <= 0 { //人物被杀死
		}

		//广播主动人物、被动人物 和人物技能
		attData.Player = player.Name
		m, _ := json.Marshal(*attData)
		fmt.Println("广播攻击人物", string(m))
		this.exBrocast(session, ATTACK_MON_BRO, m)
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

	//给这个新进入的玩家所有怪物信息
	//建立一个数组，保存所有要发送的怪，作为缓冲
	tempMonArr := make([]*MonsterDTO, 0)
	for k := range this.MonGens { //遍历此地图内的所有怪物管理器
		for _, mv := range this.MonGens[k].Monsters { //遍历刷怪管理器里的所有怪物
			if mv != nil {
				if mv.Hp > 0 { //判断是活着的怪,随机怪物的位置
					tempMonArr = append(tempMonArr, mv)
				}
			}
		}
	}
	go sendMon(session, tempMonArr)
	fmt.Println("此地图怪物数量", len(tempMonArr))
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

//小广播MAP协议
func (this *MapHandler) exBrocast(session *ace.Session, command int, message []byte) {
	for s, _ := range this.Roles {
		if s != session {
			this.Write(s, command, message)
		}
	}
}

//大广播MAP协议
func (this *MapHandler) brocast(command int, message []byte) {
	for s, _ := range this.Roles {
		this.Write(s, command, message)
	}
}

//写入Session
func (this *MapHandler) Write(session *ace.Session, command int, message []byte) {
	session.Write(&ace.DefaultSocketModel{protocol.MAP, this.Area, command, message})
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
