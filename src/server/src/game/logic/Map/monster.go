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
	"math/rand"

	_ "github.com/go-sql-driver/mysql"
)

type MonsterDTO struct {
	Name        string
	MonsterType int
	Look        string
	Level       int
	Exp         int
	Hp          int
	MaxHp       int
	Def         int
	Atk         int
	WalkSpeed   int
	AtkSpeed    int
	FirstIndex  int
	SecondIndex int
	OriPoint    data.Vector3
}

type MonsterHandler struct {
	AllMonsters map[string]*MonsterDTO //绑定怪物名字与怪物模型
}

//所有怪物
var MonsterSync = &MonsterHandler{AllMonsters: make(map[string]*MonsterDTO)}

//type MonGenManager struct {
//	//子管理器的切片
//	MonGens []*MonGenDTO
//}

//var MonManager = &MonGenManager{MonGens: make([]*MonGenDTO, 0)}

type MonGenDTO struct {
	monMap        int //怪物所在地图
	point         data.Vector3
	name          string
	ranges        int
	amount        int
	interval      int //刷新间隔
	currentAmount int //现在几只
	Index         int
	Monsters      []*MonsterDTO
}

func init() {

}

func InitGenMon() {
	//初始化怪物数据库
	MonsterSync.initProcess()
	//初始化刷怪管理器
	genMons := tools.LoadFile("config/MonGen.txt")
	//fmt.Println(genMons) //按回车符拆分信息   然后按空格符拆分具体信息
	dtos := strings.Split(genMons, "\r")
	//note := 0
	for _, v := range dtos {
		//注释行的计数
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
			//创建子管理器
			MonGen := &MonGenDTO{_monMap, data.Vector3{_point_x, _point_y, _point_z}, _name, _ranges, _amount, _interval, -1, -1, make([]*MonsterDTO, 0)}
			Manager.Maps[_monMap].MonGens = append(Manager.Maps[_monMap].MonGens, MonGen)
			MonGen.Index = len(Manager.Maps[_monMap].MonGens) - 1
			//MonManager.MonGens = append(MonManager.MonGens, MonGen)
			fmt.Println("子管理器", MonGen)
			//子管理器刷出所有怪物
			for i := 0; i < MonGen.amount; i++ {
				MonGen.currentAmount++
				if MonGen.currentAmount < MonGen.amount { //小于刷怪数的话就刷怪
					mon := new(MonsterDTO) //新创建一只怪
					*mon = *MonsterSync.AllMonsters[MonGen.name]
					mon.FirstIndex = MonGen.Index
					mon.SecondIndex = MonGen.currentAmount
					//分散一下位置
					rx := ((rand.Float64() - 0.5) * 2) * (float64(MonGen.ranges) / 2)
					rz := ((rand.Float64() - 0.5) * 2) * (float64(MonGen.ranges) / 2)
					mon.OriPoint.X = MonGen.point.X + rx
					mon.OriPoint.Y = MonGen.point.Y
					mon.OriPoint.Z = MonGen.point.Z + rz

					MonGen.Monsters = append(MonGen.Monsters, mon) //把怪物加入子管理器
					m, _ := json.Marshal(*mon)
					fmt.Println("初始化刷怪信息", string(m))
				}
				go MonGen.reLiveMon()
			}
		} else {
			//	note++
		}
	}
}

////玩家上线后要请求这个地图里的所有怪
//func (this *MonGenDTO) GetMons(session *ace.Session) {
//	//建立一个数组，保存所有要发送的怪，作为缓冲
//	tempMonArr := make([]*MonsterDTO, 0)

//	for _, mv := range this.monsters { //遍历刷怪管理器里的所有怪物
//		if mv != nil {
//			if mv.Hp != 0 { //判断是活着的怪,随机怪物的位置
//				tempMonArr = append(tempMonArr, mv)
//			}
//		}
//	}
//	fmt.Println("怪物数量", len(tempMonArr))
//	go this.sendMon(session, tempMonArr)
//}

func sendMon(session *ace.Session, mons []*MonsterDTO) {
	tempcount := 0
	timer := time.NewTicker(time.Duration(50) * time.Millisecond)
	for {
		select {
		case <-timer.C:
			if len(mons) > 0 { //有值的话
				if mons[0] != nil {
					m, _ := json.Marshal(*mons[0])
					session.Write(&ace.DefaultSocketModel{protocol.MAP, -1, MONSTER_INIT_SRES, m})
					mons = append(mons[:0], mons[1:]...)
					tempcount++
					//	fmt.Println("刷一个怪", string(m), tempcount)
				}
			} else {
				fmt.Println("刷完了")
				return
			}
		}
	}
}

//定时刷怪
func (this *MonGenDTO) reLiveMon() {
	timer := time.NewTicker(time.Duration(this.interval) * time.Second)
	for {
		select {
		case <-timer.C:
			//具体刷怪逻辑
			if this.currentAmount < this.amount-1 { //小于刷怪数的话就刷怪
				for _, mon := range this.Monsters {

					fmt.Println(mon.Name)
					if mon.Hp <= 0 { //复活这个死怪
						m, _ := json.Marshal(*mon)
						fmt.Println("复活死怪", string(m))
						//广播所有此地图的人刷怪
						roles := Manager.Maps[this.monMap].Roles
						for se, _ := range roles {
							m, _ := json.Marshal(*mon)

							fmt.Println("刷怪信息", string(m))
							se.Write(&ace.DefaultSocketModel{protocol.MAP, -1, 0, m})
						}
					}
					break
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
		MonsterDTO := &MonsterDTO{name, monstertype, look, level, exp, hp, hp, def, atk, walkSpeed, atkSpeed, -1, -1, data.Vector3{0, 0, 0}}
		//	fmt.Println("从数据库读取怪物：", MonsterDTO)
		this.AllMonsters[name] = MonsterDTO //保存物品信息
	}
}

func checkErr(err error) {
	if err != nil {
		panic(err)
	}
}
