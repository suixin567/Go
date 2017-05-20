// RegistHandler
package Login

import (
	"ace"
	"database/sql"
	"encoding/json"
	"fmt"
	"game/data"
	"game/logic/protocol"
	"time"

	_ "github.com/go-sql-driver/mysql"
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

//实例
var LoginHandler = &Handler{}

func (this *Handler) Process(session *ace.Session, message ace.DefaultSocketModel) {
	switch message.Command {
	case 0: //注册
		this.RegistProcess(session, message)
		break
	case 2: //登陆
		this.LoginProcess(session, message)
		break
	}
}

//注册账号
func (this *Handler) RegistProcess(session *ace.Session, message ace.DefaultSocketModel) {
	registData := &AccountDTO{}
	err := json.Unmarshal(message.Message, &registData)
	if err != nil {
		fmt.Println(err)
	}
	fmt.Println(registData.Username, registData.Password)
	regidtResult := this.RegAccount(&registData.Username, &registData.Password)
	if regidtResult == false {
		fmt.Println("注册失败")
		session.Write(&ace.DefaultSocketModel{protocol.LOGIN, -1, REGIST, []byte("false")})
	} else {
		fmt.Println("注册成功")
		session.Write(&ace.DefaultSocketModel{protocol.LOGIN, -1, REGIST, []byte("true")})
	}
}

//登陆
func (this *Handler) LoginProcess(session *ace.Session, message ace.DefaultSocketModel) {
	loginData := &AccountDTO{}
	err := json.Unmarshal(message.Message, &loginData)
	if err != nil {
		fmt.Println(err)
	}
	//fmt.Println(loginData.Username, loginData.Password)
	loginResult := this.GetAccount(session, &loginData.Username, &loginData.Password)
	if loginResult == false {
		//fmt.Println("登陆失败")
		session.Write(&ace.DefaultSocketModel{protocol.LOGIN, -1, LOGIN, []byte("false")})
	} else {
		//fmt.Println("登陆成功")
		session.Write(&ace.DefaultSocketModel{protocol.LOGIN, -1, LOGIN, []byte("true")})
		return
	}
}

//******************************************************************
//                       注册具体逻辑
//******************************************************************
//注册账号
func (this *Handler) RegAccount(un *string, psw *string) bool {
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	checkErr(err)
	//先对比数据库 看是否已被注册
	stmtOut, err := db.Prepare("SELECT username FROM userinfo WHERE username = ?")
	var username string
	err = stmtOut.QueryRow(*un).Scan(&username)
	//fmt.Printf("The square is: %s", username)
	if *un == username {
		fmt.Printf("这账号已被注册")
		return false
	}
	//插入数据
	stmt, err := db.Prepare("INSERT userinfo SET username=?,password=?,playeramount=?,player1=?,player2=?,online=?,lasttime=?,createdtime=?")
	checkErr(err)
	_, err = stmt.Exec(*un, *psw, 0, "", "", 0, time.Now().Format("2006-01-02 15:04:05"), time.Now().Format("2006-01-02 15:04:05"))
	checkErr(err)
	return true
}

//******************************************************************
//                       登陆具体逻辑
//******************************************************************
//登陆
func (this *Handler) GetAccount(session *ace.Session, un *string, psw *string) bool {
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	checkErr(err)
	//验证账号与密码
	stmtOut, err := db.Prepare("SELECT password FROM userinfo WHERE username = ?")
	var password string
	err = stmtOut.QueryRow(*un).Scan(&password)
	//fmt.Printf("The square is: %s", password)
	if *psw == password {
		fmt.Printf("账号%s与密码%s匹配", *un, *psw)
		//检验此账号是否已经登录
		_, ok := data.SyncAccount.AccountSession[*un]
		if ok { //如果能在此切片中取出值，说明已登录
			fmt.Println("此账号已登录")
			//把之前登陆的人顶掉
			//fmt.Println("踢掉之前的", se)
			//se.Write(&ace.DefaultSocketModel{protocol.OFFLINE, -1, -1, []byte("false")})
			//se.Close() //关闭老session
			return false
		} else {
			fmt.Println("  此账号没有登录")
		}

		//更新最后登录时间
		stmtUp, err := db.Prepare("update userinfo set online=?,lasttime=? where username=?")
		checkErr(err)
		_, err = stmtUp.Exec(1, time.Now().Format("2006-01-02 15:04:05"), *un)
		checkErr(err)
		//此账号与session相关联
		data.SyncAccount.AccountSession[*un] = session
		data.SyncAccount.SessionAccount[session] = *un
		return true
	} else {
		fmt.Printf("登陆失败")
		return false
	}
	return false
}

func checkErr(err error) {
	if err != nil {
		panic(err)
	}
}
