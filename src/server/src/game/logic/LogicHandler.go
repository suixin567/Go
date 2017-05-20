// LogicHandler
package logic

import (
	"ace"
	"fmt"
	"game/data"
	"game/logic/Item"
	"game/logic/Login"
	"game/logic/Map"
	"game/logic/User"
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
	Item.ItemHandler.SessionClose(session)
	data.SyncAccount.SessionClose(session)
}

func (this *GameHandler) MessageReceived(session *ace.Session, message interface{}) {
	m := message.(ace.DefaultSocketModel)
	//fmt.Println("收到客户端的请求：", message)
	switch m.Type {
	case protocol.LOGIN: //收到登录消息
		Login.LoginHandler.Process(session, m)
		break
	case protocol.USER: //收到人物信息
		User.UserHandler.Process(session, m)
		break
	case protocol.MAP: //收到地图信息
		Map.Manager.Process(session, m)
		break
	case protocol.ITEM: //请求物品信息
		Item.ItemHandler.Process(session, m)
		break

	case 99: //测试类型
		fmt.Println("收到信息:", string(m.Message))
		break
	default:
		fmt.Println("未知协议类型！", m.Type)
		break
	}
}
