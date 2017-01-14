// RegistHandler
package login

import (
	"ace"
	"encoding/json"
	"fmt"
	"game/data"
	"game/logic/protocol"
)

type AccountDTO struct {
	Username string
	Password string
}

const (
	REGIST = 1 //command=1代表这是注册结果
	LOGIN  = 3 //3代表登陆成功
)

type Handler struct {
}

var LoginHander = &Handler{}

//这是服务端处理申请账号的函数
func (this *Handler) RegistProcess(session *ace.Session, message ace.DefaultSocketModel) {
	registData := &AccountDTO{}
	err := json.Unmarshal(message.Message, &registData)
	if err != nil {
		fmt.Println(err)
	}
	fmt.Println(registData.Username, registData.Password)
	regidtResult := data.SyncAccount.RegAccount(&registData.Username, &registData.Password)
	if regidtResult == false {
		fmt.Println("注册失败")
		session.Write(&ace.DefaultSocketModel{protocol.LOGIN, -1, REGIST, []byte("false")})
	} else {
		fmt.Println("注册成功")
		session.Write(&ace.DefaultSocketModel{protocol.LOGIN, -1, REGIST, []byte("true")})
	}
}

//登陆处理函数
func (this *Handler) LoginProcess(session *ace.Session, message ace.DefaultSocketModel) {
	loginData := &AccountDTO{}
	err := json.Unmarshal(message.Message, &loginData)
	if err != nil {
		fmt.Println(err)
	}
	//fmt.Println(loginData.Username, loginData.Password)
	loginResult := data.SyncAccount.GetAccount(session, &loginData.Username, &loginData.Password)
	if loginResult == false {
		//fmt.Println("登陆失败")
		session.Write(&ace.DefaultSocketModel{protocol.LOGIN, -1, LOGIN, []byte("false")})
	} else {
		//fmt.Println("登陆成功")
		session.Write(&ace.DefaultSocketModel{protocol.LOGIN, -1, LOGIN, []byte("true")})
		return
	}
}
