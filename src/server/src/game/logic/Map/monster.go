package Map

import (
	"database/sql"
	"fmt"
	"game/data"
	"time"
	"tools"

	_ "github.com/go-sql-driver/mysql"
)

type MonsterDTO struct {
	Name        string
	MonsterType int
	Look        string
	Level       int
	Exp         int
	Hp          int
	Def         int
	Atk         int
	WalkSpeed   int
	AtkSpeed    int
}

type MonsterHandler struct {
	Monsters map[string]*MonsterDTO //绑定怪物名字与怪物模型
}

//所有怪物
var MonsterSync = &MonsterHandler{Monsters: make(map[string]*MonsterDTO)}

type MonGenDTO struct {
	monMap        int //怪物所在地图
	point         data.Vector3
	name          string
	ranges        int
	amount        int
	interval      int //刷新间隔
	currentAmount int //现在几只
}

func init() {
	//初始化怪物数据库
	MonsterSync.initProcess()
	//初始化刷怪管理器
	genMons := tools.LoadFile("config/MonGen.txt")
	fmt.Println(genMons) //按回车符拆分信息   然后按空格符拆分具体信息

	//	MonGen := &MonGenDTO{0, data.Vector3{0, 1, 2}, "黑野猪", 10, 4, 1, 0}
	//	go MonGen.creatMon(MonGen.interval, &MonGen.name)

	//	MonGen = &MonGenDTO{0, data.Vector3{0, 1, 2}, "白野猪", 10, 3, 5, 0}
	//	go MonGen.creatMon(MonGen.interval, &MonGen.name)

}

//定时刷怪
func (this *MonGenDTO) creatMon(interval int, name *string) {
	timer := time.NewTicker(time.Duration(interval) * time.Second)
	for {
		select {
		case <-timer.C:
			//具体刷怪逻辑
			this.currentAmount++
			if this.currentAmount <= this.amount { //小于刷怪数的话就刷怪
				fmt.Println("刷", *name)
				//通知所有此地图的人刷怪
			}
		}
	}
}

//服务器初始化怪物数据库
func (this *MonsterHandler) initProcess() {
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	checkErr(err)
	rows, err := db.Query("SELECT * FROM monster")
	checkErr(err)
	for rows.Next() {

		var name string
		var monstertype int
		var look string
		var level int
		var exp int
		var hp int
		var def int
		var atk int
		var walkSpeed int
		var atkSpeed int
		err = rows.Scan(&name, &monstertype, &look, &level, &exp, &hp, &def, &atk, &walkSpeed, &atkSpeed)
		checkErr(err)
		MonsterDTO := &MonsterDTO{name, monstertype, look, level, exp, hp, def, atk, walkSpeed, atkSpeed}
		fmt.Println("怪物：", MonsterDTO)
		this.Monsters[name] = MonsterDTO //保存物品信息
	}
}

func checkErr(err error) {
	if err != nil {
		panic(err)
	}
}
