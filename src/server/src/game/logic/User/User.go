// CreatPlayerHandler
package User

import (
	"ace"
	"database/sql"
	"encoding/json"
	"fmt"
	"game/data"
	"game/logic/protocol"

	_ "github.com/go-sql-driver/mysql"
)

const (
	CREATPLAYER   = 3
	PLAYER_LIST   = 1
	PLAYER_SELECT = 5
)

type StringDTO struct {
	Value string
}

type CreatDTO struct {
	Acc  string //账号
	Job  int
	Name string
}

type Handler struct {
}

var UserHandler = &Handler{}

func (this *Handler) Process(session *ace.Session, message ace.DefaultSocketModel) {
	switch message.Command {
	case 0: //获取角色
		this.GetPlayerProcess(session, message)
		break
	case 2: //创建角色
		this.CreatPlayerProcess(session, message)
		break
	case 4: //开始游戏时，选择了一个人物
		this.SelectPlayerProcess(session, message)
		break
	}
}

//创建角色处理函数
func (this *Handler) CreatPlayerProcess(session *ace.Session, message ace.DefaultSocketModel) {
	creatData := &CreatDTO{}
	err := json.Unmarshal(message.Message, &creatData)
	if err != nil {
		fmt.Println("json解析错误")
	}
	res := CreatPlayer(&creatData.Acc, creatData.Job, &creatData.Name)
	if res == true {
		session.Write(&ace.DefaultSocketModel{protocol.USER, -1, CREATPLAYER, []byte("true")})
	} else {
		session.Write(&ace.DefaultSocketModel{protocol.USER, -1, CREATPLAYER, []byte("false")})
	}
}

//创建账号里的人物角色
func CreatPlayer(acc *string, job int, name *string) bool {
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	checkErr(err)
	//看看此账号下有几个人物
	stmtOut, err := db.Prepare("SELECT playeramount FROM userinfo WHERE username = ?")
	var amount int
	err = stmtOut.QueryRow(*acc).Scan(&amount)
	//fmt.Printf("账号里的人物数：%d\n", amount)

	switch amount {
	case 0:
		//判断是否有同昵称的人物
		stmtOut, err := db.Prepare("SELECT name FROM playerinfo WHERE name = ?")
		var playerName string
		err = stmtOut.QueryRow(*name).Scan(&playerName)
		//fmt.Printf("The square is: %s", username)
		if *name == playerName {
			fmt.Printf("此昵称已存在")
			return false
		}
		//写入人物
		stmtIns, err := db.Prepare("INSERT playerinfo SET uid=?,name=?,job=?,level=?,gold=?,exp=?,atk=?,def=?,hp=?,maxhp=?,pointx=?,pointy=?,pointz=?,rotationx=?,rotationy=?,rotationz=?,rotationw=?,map=?,active=?")
		checkErr(err)
		_, err = stmtIns.Exec(*acc, *name, job, 0, 0, 0, 5, 5, 14, 14, 1, 2, 3, 0, 0, 0, 0, 2, 0)
		checkErr(err)
		//添加物品数据库
		stmtIns, err = db.Prepare("INSERT playeritem SET playername=?,items=?,equipments=?")
		checkErr(err)
		_, err = stmtIns.Exec(*name, "[]", "[]")
		checkErr(err)
		//更细账号表
		stmtUp, err := db.Prepare("update userinfo set playeramount=?,player1=? where username=?")
		checkErr(err)
		_, err = stmtUp.Exec(1, *name, *acc)
		checkErr(err)
		return true
		break
	case 1:
		//判断是否有同昵称的人物
		stmtOut, err := db.Prepare("SELECT name FROM playerinfo WHERE name = ?")
		var playerName string
		err = stmtOut.QueryRow(*name).Scan(&playerName)
		//fmt.Printf("The square is: %s", username)
		if *name == playerName {
			fmt.Printf("此昵称已存在")
			return false
		}
		//写入人物
		stmtIns, err := db.Prepare("INSERT playerinfo SET uid=?,name=?,job=?,level=?,gold=?,exp=?,atk=?,def=?,hp=?,maxhp=?,pointx=?,pointy=?,pointz=?,rotationx=?,rotationy=?,rotationz=?,rotationw=?,map=?,active=?")
		checkErr(err)
		_, err = stmtIns.Exec(*acc, *name, job, 0, 0, 0, 5, 5, 14, 14, 1, 2, 3, 0, 0, 0, 0, 2, 0)
		checkErr(err)
		//添加物品数据库
		stmtIns, err = db.Prepare("INSERT playeritem SET playername=?,items=?,equipments=?,skills=?")
		checkErr(err)
		_, err = stmtIns.Exec(*name, "[]", "[]", "[]")
		checkErr(err)
		//更细账号表
		stmtUp, err := db.Prepare("update userinfo set playeramount=?,player2=? where username=?")
		checkErr(err)
		_, err = stmtUp.Exec(2, *name, *acc)
		checkErr(err)
		return true
		break
	default:
		fmt.Println("已经有两个人物了")
		return false
		break
	}
	return false
}

//为新创建的人物添加物品数据库
func initPlayerItems(playerName *string) {
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	stmtIns, err := db.Prepare("INSERT playeritem SET playername=?,items=?")
	checkErr(err)
	_, err = stmtIns.Exec(*playerName, "")
	checkErr(err)
}

//获取角色，仅为了在角色场景中展示
func (this *Handler) GetPlayerProcess(session *ace.Session, message ace.DefaultSocketModel) {
	getData := &StringDTO{}
	err := json.Unmarshal(message.Message, &getData)
	if err != nil {
		fmt.Println(err)
	}
	playerSlice := GetPlayer(&getData.Value)
	if len(playerSlice) == 0 {
		fmt.Println("此账号没有角色")
		session.Write(&ace.DefaultSocketModel{protocol.USER, -1, PLAYER_LIST, nil})
		return
	}
	if len(playerSlice) == 1 {
		fmt.Println("此账号有1角色：", playerSlice[0]) //输出纯数值
		m, _ := json.Marshal(playerSlice)
		//fmt.Println("Json格式的角色：", string(m)) //输出json字符串
		session.Write(&ace.DefaultSocketModel{protocol.USER, -1, PLAYER_LIST, m})
		return
	}
	if len(playerSlice) == 2 {
		fmt.Println("此账号有2角色：", playerSlice[0], playerSlice[1])
		m, _ := json.Marshal(playerSlice)
		//fmt.Println("Json格式的角色：", string(m)) //输出json字符串
		session.Write(&ace.DefaultSocketModel{protocol.USER, -1, PLAYER_LIST, m})
		return
	}
}

//获取账号里的人物角色列表，返回一个存储角色的切片
func GetPlayer(acc *string) []*data.PlayerDTO {
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	checkErr(err)
	//看看此账号下有几个人物
	stmtOut, err := db.Prepare("SELECT playeramount,player1,player2 FROM userinfo WHERE username = ?")
	checkErr(err)
	var amount int
	var p1 string
	var p2 string
	err = stmtOut.QueryRow(*acc).Scan(&amount, &p1, &p2)
	switch amount {
	case 0:
		return make([]*data.PlayerDTO, 0, 2)
		break
	case 1:
		player1Dto := GetPlayerFromDB(&p1)
		playerSlice1 := []*data.PlayerDTO{player1Dto}
		return playerSlice1
		break
	case 2:
		player1Dto := GetPlayerFromDB(&p1)
		player2Dto := GetPlayerFromDB(&p2)
		playerSlice2 := []*data.PlayerDTO{player1Dto, player2Dto}
		return playerSlice2
		break
	default:
		return make([]*data.PlayerDTO, 0, 2)
		break
	}
	return make([]*data.PlayerDTO, 0, 2)
}

//游戏开始时选择了一个人物,返回这个人物
func (this *Handler) SelectPlayerProcess(session *ace.Session, message ace.DefaultSocketModel) {
	startData := &StringDTO{}
	err := json.Unmarshal(message.Message, &startData)
	if err != nil {
		fmt.Println(err)
	}
	//根据角色名字获取角色模型
	selectPlayerDto := GetPlayerFromDB(&startData.Value)
	//session与角色模型相关联
	data.SyncAccount.SessionPlayer[session] = selectPlayerDto
	//写入响应
	m, _ := json.Marshal(selectPlayerDto)
	fmt.Println("即将开始游戏的角色的物品数据的Json格式：", string(m)) //输出json字符串
	session.Write(&ace.DefaultSocketModel{protocol.USER, -1, PLAYER_SELECT, m})
}

//根据人物名字从数据库得到人物数据
func GetPlayerFromDB(playerName *string) *data.PlayerDTO {
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	checkErr(err)

	stmtOut, err := db.Prepare("SELECT * FROM playerinfo WHERE name = ?")
	checkErr(err)
	var pid int     //用不上
	var uid string  //用不上
	var name string //用不上
	var job int
	var level int
	var gold int
	var exp int
	var atk int
	var def int
	var hp int
	var maxhp int
	var pointx float64
	var pointy float64
	var pointz float64
	var rotationx float64
	var rotationy float64
	var rotationz float64
	var rotationw float64
	var rolemap int
	var active int
	err = stmtOut.QueryRow(playerName).Scan(&pid, &uid, &name, &job, &level, &gold, &exp, &atk, &def, &hp, &maxhp, &pointx, &pointy, &pointz, &rotationx, &rotationy, &rotationz, &rotationw, &rolemap, &active)
	checkErr(err)
	playerDto := data.PlayerDTO{name, job, level, gold, exp, atk, def, hp, maxhp, data.Vector3{pointx, pointy, pointz}, data.Vector4{rotationx, rotationy, rotationz, rotationw}, rolemap, active}
	//fmt.Println(player1Dto)
	return &playerDto
}

func checkErr(err error) {
	if err != nil {
		panic(err)
	}
}
