package Map

import (
	"database/sql"
	"fmt"

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

var MonsterSync = &MonsterHandler{Monsters: make(map[string]*MonsterDTO)}

func init() {
	MonsterSync.initProcess()
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
