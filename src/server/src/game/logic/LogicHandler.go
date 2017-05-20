// LogicHandler
package logic

import (
	"ace"
	"fmt"
	"game/data"
	"game/logic/Map"
	"game/logic/User"
	"game/logic/Item"
	"game/logic/Login"
	"game/logic/protocol"
)

//它的三个方法实现了ServerSocket中的Handler接口
type GameHandler struct {
}

func (this *GameHandler) SessionOpen(session *ace.Session) {
	fmt.Println("会话 open", session)
}

func (this *GameHandler) SessionClose(session *ace.Session) {
	fmt.Println("会话 closed", session)
	Map.Manager.SessionClose(session)
	Item.ItemHander.SessionClose(session)
	data.SyncAccount.SessionClose(session)
}

func (this *GameHandler) MessageReceived(session *ace.Session, message interface{}) {
	m := message.(ace.DefaultSocketModel)
	//fmt.Println("收到客户端的请求：", message)
	switch m.Type {
	case protocol.LOGIN: //收到登录消息
		if m.Command == 0 { //注册
			Login.LoginHander.RegistProcess(session, m)
		}
		if m.Command == 2 { //登陆
			Login.LoginHander.LoginProcess(session, m)
		}
		break
	case protocol.USER: //收到职业信息
		if m.Command == 0 { //获取角色
			User.Manager.GetPlayerProcess(session, m)
		}
		if m.Command == 2 { //创建账号
			User.Manager.CreatPlayerProcess(session, m)
		}
		if m.Command == 4 { //开始游戏时，选择了一个人物
			User.Manager.SelectPlayerProcess(session, m)
		}
		break
	case protocol.MAP: //收到地图信息
		Map.Manager.Process(session, m)
		break
	case protocol.ITEM: //请求物品信息
		Item.ItemHander.ItemProcess(session, m)
		break
	case 99://测试类型
		fmt.Println("收到信息:", string(m.Message))
		break
	default:
		fmt.Println("未知协议类型！", m.Type)
		break
	}
}
