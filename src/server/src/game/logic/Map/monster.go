package Map

//这是一个刷怪文件

import (
	"ace"
	"database/sql"
	"fmt"
	"game/data"
	"strings"
	"time"
	"tools"

	"encoding/json"
	"game/logic/protocol"

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
	//fmt.Println(genMons) //按回车符拆分信息   然后按空格符拆分具体信息
	dtos := strings.Split(genMons, "\r")
	for _, v := range dtos {
		if !strings.Contains(v, ";") { //过滤掉带注释的行
			//fmt.Println("--->", v)
			vs := strings.Fields(v) //拆分为具体值 根据空白返回一个属性列表["1" "2" "2"]
			//fmt.Println("具体值", values[0], values[1])
			_monMap := tools.String2int(&vs[0])
			_point_x := tools.String2float(&vs[1])
			_point_y := tools.String2float(&vs[2])
			_point_z := tools.String2float(&vs[3])
			_name := vs[4]
			_ranges := tools.String2int(&vs[5])
			_amount := tools.String2int(&vs[6])
			_interval := tools.String2int(&vs[7])

			MonGen := &MonGenDTO{_monMap, data.Vector3{_point_x, _point_y, _point_z}, _name, _ranges, _amount, _interval, 0}
			fmt.Println(MonGen)
			go MonGen.creatMon()
		}
	}
}

//定时刷怪
func (this *MonGenDTO) creatMon() {
	timer := time.NewTicker(time.Duration(this.interval) * time.Second)
	for {
		select {
		case <-timer.C:
			//具体刷怪逻辑
			this.currentAmount++
			if this.currentAmount <= this.amount { //小于刷怪数的话就刷怪
				fmt.Println("刷怪信息0")
				//广播所有此地图的人刷怪
				roles := Manager.Maps[this.monMap].Roles
				for se, _ := range roles {
					m, _ := json.Marshal(*this)

					fmt.Println("刷怪信息", string(m))
					se.Write(&ace.DefaultSocketModel{protocol.MAP, -1, 0, m})
				}
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
