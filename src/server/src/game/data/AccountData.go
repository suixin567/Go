package data

import (
	"ace"
	"database/sql"
	"fmt"
	"time"

	_ "github.com/go-sql-driver/mysql"
)

type Vector3 struct {
	X float64
	Y float64
	Z float64
}
type Vector4 struct {
	X float64
	Y float64
	Z float64
	W float64
}

//人物角色
type PlayerDTO struct {
	//	Id       int
	Name     string
	Job      int
	Level    int
	Gold     int
	Exp      int
	Atk      int
	Def      int //防御
	Hp       int
	MaxHP    int
	Point    Vector3 //坐标
	Rotation Vector4 //旋转
	Map      int
	Active   int
}

type Sync struct {
	AccountSession map[string]*ace.Session     //关联一个账号的session ,踢下线时，根据账号把一个session踢掉
	SessionAccount map[*ace.Session]string     //离线时 ，根据session 清理在线列表
	SessionPlayer  map[*ace.Session]*PlayerDTO //根据session获得一个角色,一个玩家开始游戏后在这里登记，其他文件中就可以根据session获得一个角色了。
}

var SyncAccount = &Sync{AccountSession: make(map[string]*ace.Session), SessionAccount: make(map[*ace.Session]string), SessionPlayer: make(map[*ace.Session]*PlayerDTO)}

func checkErr(err error) {
	if err != nil {
		panic(err)
	}
}

//处理离线
func (this *Sync) SessionClose(session *ace.Session) {
	tempacc, ok := this.SessionAccount[session]
	//说明此session没有登陆 ，那就没有什么可以需要操作的
	if !ok {
		return
	}
	//更新用户表的最后登录时间
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	checkErr(err)
	stmtUp, err := db.Prepare("update userinfo set online=?,lasttime=? where username=?")
	checkErr(err)
	_, err = stmtUp.Exec(0, time.Now().Format("2006-01-02 15:04:05"), this.SessionAccount[session])
	checkErr(err)
	//持久化人物数据
	lp, ok := this.SessionPlayer[session] //一个账号里没有角色，或者此session还没有绑定角色，离线会导致空指针，所以先判断一下
	if ok {
		stmtUp, err = db.Prepare("update playerinfo set name=?,job=?,level=?,gold=?,exp=?,atk=?,def=?,hp=?,maxhp=?,pointx=?,pointy=?,pointz=?,rotationx=?,rotationy=?,rotationz=?,rotationw=?,map=?,active=? where name=?")
		checkErr(err)
		_, err = stmtUp.Exec(lp.Name, lp.Job, lp.Level, lp.Gold, lp.Exp, lp.Atk, lp.Def, lp.Hp, lp.MaxHP, lp.Point.X, lp.Point.Y, lp.Point.Z, lp.Rotation.X, lp.Rotation.Y, lp.Rotation.Z, lp.Rotation.W, lp.Map, 0, lp.Name)
		checkErr(err)
		//持久化之后删除内存
		delete(this.SessionPlayer, session)
	}
	fmt.Println("持久化角色数据")
	//清除session与账号相关联的 map数据
	delete(this.AccountSession, tempacc)
	delete(this.SessionAccount, session)
}
