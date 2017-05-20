package Item

import (
	"ace"
	"database/sql"
	"encoding/json"
	"fmt"
	"game/data"
	"game/logic/Map"
	"game/logic/protocol"

	_ "github.com/go-sql-driver/mysql"
)

const (
	INIT_CREQ = 0 //请求初始化物品
	INIT_SRES = 1

	PLAYER_ITEM_CREQ = 2 //请求一个角色物品数据
	PLAYER_ITEM_SRES = 3

	PLAYER_EQUIPMENT_CREQ = 4 //请求一个角色已穿戴的装备数据
	PLAYER_EQUIPMENT_SRES = 5

	BUY_CREQ = 6 //申请购买物品
	BUY_SRES = 7

	USE_CREQ = 8 //申请使用物品
	USE_SRES = 9

	PUTON_CREQ = 10 //申请穿戴装备
	PUTON_SREQ = 11

	PUTOFF_CREQ = 12 //脱下装备
	PUTOFF_SREQ = 13
)

//一条物品信息
type ItemDTO struct {
	Id        int
	Name      string
	ItemType  int
	Sprite    string
	Quality   string
	Capacity  int
	Hp        int
	Mp        int
	Attack    int
	Def       int
	Speed     int
	SellPrice int
	BuyPrice  int

	Description string
}

//使用一个物品后网路传输的模型
type UseItemDTO struct {
	Name   string
	ItemId int
}

type ItemHandler struct {
	items             map[int]*ItemDTO           //游戏中所有的物品。预留一个根据id获取物品的接口
	SessionItems      map[*ace.Session][]ItemDTO //一个session中都有什么物品。也就是一个角色的物品
	SessionEquipments map[*ace.Session][]ItemDTO //一个session中都穿戴了什么物品
	SessionSkills     map[*ace.Session][]SkillDTO
}

var ItemHander = &ItemHandler{items: make(map[int]*ItemDTO), SessionItems: make(map[*ace.Session][]ItemDTO), SessionEquipments: make(map[*ace.Session][]ItemDTO), SessionSkills: make(map[*ace.Session][]SkillDTO)}

func init() {
	ItemHander.initProcess()
}

//服务器初始化所有物品信息
func (this *ItemHandler) initProcess() {
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	checkErr(err)
	rows, err := db.Query("SELECT * FROM item")
	checkErr(err)
	for rows.Next() {
		var id int
		var name string
		var itemType int
		var sprite string
		var quality string
		var capacity int
		var hp int
		var mp int
		var attack int
		var def int
		var speed int
		var sellPrice int
		var buyPrice int
		var description string
		err = rows.Scan(&id, &name, &itemType, &sprite, &quality, &capacity, &hp, &mp, &attack, &def, &speed, &sellPrice, &buyPrice, &description)
		checkErr(err)
		itemDTO := &ItemDTO{id, name, itemType, sprite, quality, capacity, hp, mp, attack, def, speed, sellPrice, buyPrice, description}
		fmt.Println(itemDTO)
		this.items[id] = itemDTO //保存物品信息
	}
}

//物品处理逻辑
func (this *ItemHandler) ItemProcess(session *ace.Session, message ace.DefaultSocketModel) {
	switch message.Command {
	case INIT_CREQ: //初始化所有游戏物品
		itemSlice := []ItemDTO{}
		//遍历物品的map
		for _, v := range this.items {
			//	fmt.Printf("k=%v, v=%v\n", k, v)
			itemSlice = append(itemSlice, *v)
		}
		m, _ := json.Marshal(itemSlice)
		//fmt.Println("Json格式的给玩家的初始物品：", string(m))
		session.Write(&ace.DefaultSocketModel{protocol.ITEM, -1, INIT_SRES, m})
		break
	case PLAYER_ITEM_CREQ: //把一个角色拥有的背包物品传给客户端
		pn := data.SyncAccount.SessionPlayer[session].Name //角色名字
		playerItemsStr := GetPlayerItemsFromDB(&pn)        //角色物品的字符串
		//登记一个人的物品信息-----------------------
		itemsInfo := []ItemDTO{}
		err := json.Unmarshal([]byte(*playerItemsStr), &itemsInfo)
		if err != nil {
			fmt.Println(err)
		}
		this.SessionItems[session] = itemsInfo
		//登记结束--------------------------------------
		session.Write(&ace.DefaultSocketModel{protocol.ITEM, -1, PLAYER_ITEM_SRES, []byte(*playerItemsStr)})
		break
	case PLAYER_EQUIPMENT_CREQ: //把一个角色已穿戴的装备传给客户端
		thisPlayer := data.SyncAccount.SessionPlayer[session]
		playerEquipmentsStr := GetPlayerEquipmentsFromDB(&thisPlayer.Name) //角色装备的字符串
		//登记一个人的装备信息-----------------------
		equipmentsInfo := []ItemDTO{}
		err := json.Unmarshal([]byte(*playerEquipmentsStr), &equipmentsInfo)
		if err != nil {
			fmt.Println(err)
		}
		this.SessionEquipments[session] = equipmentsInfo
		//登记结束--------------------------------------
		session.Write(&ace.DefaultSocketModel{protocol.ITEM, -1, PLAYER_EQUIPMENT_SRES, []byte(*playerEquipmentsStr)})
		break
	case BUY_CREQ: //购买物品
		fmt.Println("要购买的是：", string(message.Message))
		buyItem := &ItemDTO{}
		err := json.Unmarshal(message.Message, &buyItem)
		if err != nil {
			fmt.Println(err)
		}
		//买的起
		if data.SyncAccount.SessionPlayer[session].Gold >= buyItem.BuyPrice {
			data.SyncAccount.SessionPlayer[session].Gold -= buyItem.BuyPrice //扣钱
			itemsInfo := this.SessionItems[session]                          //给人物增加物品
			itemsInfo = append(itemsInfo, *buyItem)
			this.SessionItems[session] = itemsInfo
			//响应
			m, _ := json.Marshal(*buyItem)
			//	fmt.Println(string(m))
			session.Write(&ace.DefaultSocketModel{protocol.ITEM, -1, BUY_SRES, m})
		} else {
			session.Write(&ace.DefaultSocketModel{protocol.ITEM, -1, BUY_SRES, []byte("")})
		}
		break
	case USE_CREQ: //使用物品
		fmt.Println("要使用的是：", string(message.Message))
		useItem := &ItemDTO{}
		err := json.Unmarshal(message.Message, &useItem)
		if err != nil {
			fmt.Println(err)
		}
		//使用物品后要删除切片中的物品
		for k, v := range this.SessionItems[session] {
			if *useItem == v {
				fmt.Println("使用的是：", v.Id, v.Name)
				this.SessionItems[session] = append(this.SessionItems[session][:k], this.SessionItems[session][k+1:]...)
				break
			}
		}
		usePlayer := data.SyncAccount.SessionPlayer[session]
		if useItem.Id == 1000 || useItem.Id == 1001 {
			//更新人物的血量属性
			usePlayer.Hp += useItem.Hp
			if usePlayer.Hp >= usePlayer.MaxHP {
				usePlayer.Hp = usePlayer.MaxHP
			}
			//广播新的属性值给地图内的所有人   人物名字+物品名字
			roles := Map.Manager.Maps[usePlayer.Map].Roles
			for se, _ := range roles {
				useItemdto := UseItemDTO{usePlayer.Name, useItem.Id}
				message, _ := json.Marshal(useItemdto)

				se.Write(&ace.DefaultSocketModel{protocol.ITEM, -1, USE_SRES, message})
			}
		}
		if useItem.Id == 6000 { //使用治愈术
			skillDTO := SkillSync.Skills[useItem.Name]
			//给人物增加技能
			skillsInfo := this.SessionSkills[session]
			skillsInfo = append(skillsInfo, *skillDTO)
			this.SessionSkills[session] = skillsInfo
			//响应是技能模型
			message, _ := json.Marshal(skillDTO)
			//fmt.Println(string(message))
			session.Write(&ace.DefaultSocketModel{protocol.ITEM, -1, 15, message})
		}
		break
	case PUTON_CREQ: //穿戴装备
		fmt.Println("穿戴的是：", string(message.Message))
		putOnItem := &ItemDTO{}
		err := json.Unmarshal(message.Message, &putOnItem)
		if err != nil {
			fmt.Println(err)
		}
		putOnPlayer := data.SyncAccount.SessionPlayer[session]
		//因为此物品被穿戴了，所以从背包中移除
		for k, v := range this.SessionItems[session] {
			if *putOnItem == v {
				this.SessionItems[session] = append(this.SessionItems[session][:k], this.SessionItems[session][k+1:]...)
			}
		}
		//如果之前穿了同部位装备,删除旧切片
		for k, v := range this.SessionEquipments[session] {
			if putOnItem.ItemType == v.ItemType {
				putOnPlayer.MaxHP -= v.Hp
				putOnPlayer.Atk -= v.Attack
				putOnPlayer.Def -= v.Def
				this.SessionEquipments[session] = append(this.SessionEquipments[session][:k], this.SessionEquipments[session][k+1:]...)
			}
		}
		//给人物装备列表增加装备
		equipmentsInfo := this.SessionEquipments[session]
		equipmentsInfo = append(equipmentsInfo, *putOnItem)
		this.SessionEquipments[session] = equipmentsInfo
		//穿了装备后应修改人物的属性
		putOnPlayer.MaxHP += putOnItem.Hp
		putOnPlayer.Atk += putOnItem.Attack
		putOnPlayer.Def += putOnItem.Def

		//广播新的属性值给地图内的所有人
		roles := Map.Manager.Maps[putOnPlayer.Map].Roles
		for se, _ := range roles {
			//响应 为人物属性
			m, _ := json.Marshal(*putOnPlayer)

			fmt.Println("人物的最新属性", string(m))
			se.Write(&ace.DefaultSocketModel{protocol.ITEM, -1, PUTON_SREQ, m})
		}
		break
	case PUTOFF_CREQ: //脱下装备
		fmt.Println("脱下的是：", string(message.Message))
		putOffItem := &ItemDTO{}
		err := json.Unmarshal(message.Message, &putOffItem)
		if err != nil {
			fmt.Println(err)
		}
		putOffPlayer := data.SyncAccount.SessionPlayer[session]
		//在装备列表中移除
		for k, v := range this.SessionEquipments[session] {
			if *putOffItem == v {
				this.SessionEquipments[session] = append(this.SessionEquipments[session][:k], this.SessionEquipments[session][k+1:]...)
				//因为此物品被脱下了，所以在背包中增加
				itemsInfo := this.SessionItems[session]
				itemsInfo = append(itemsInfo, *putOffItem)
			}
		}
		//脱下装备后应修改人物的属性
		putOffPlayer.MaxHP -= putOffItem.Hp
		putOffPlayer.Atk -= putOffItem.Attack
		putOffPlayer.Def -= putOffItem.Def
		if putOffPlayer.Hp > putOffPlayer.MaxHP { //脱下加血量的装备后 有可能导致血量>最大血量
			putOffPlayer.Hp = putOffPlayer.MaxHP
		}
		//广播新的属性值给地图内的所有人
		roles := Map.Manager.Maps[putOffPlayer.Map].Roles
		for se, _ := range roles {
			//响应 为人物属性
			m, _ := json.Marshal(*putOffPlayer)

			fmt.Println("人物的最新属性", string(m))
			se.Write(&ace.DefaultSocketModel{protocol.ITEM, -1, PUTOFF_SREQ, m})
		}
		break
	default:
		fmt.Println("物品命令类型错误")
		break
	}

}

//当一个玩家开始游戏后，初始化他的物品
func (this *ItemHandler) InitPlayerItems(session *ace.Session, playerItems *string) {
	fmt.Println("Item：这个玩家有的物品：", *playerItems)

}

////--------------------------------------------------------
////							工具方法
////---------------------------------------------------------
//根据人物名字从数据库得到人物的物品数据
func GetPlayerItemsFromDB(playerName *string) *string {
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	checkErr(err)
	stmtOut, err := db.Prepare("SELECT items FROM playeritem WHERE playername = ?")
	checkErr(err)
	var playerItems string
	err = stmtOut.QueryRow(playerName).Scan(&playerItems)
	checkErr(err)
	return &playerItems
}

//根据人物名字从数据库得到人物的装备
func GetPlayerEquipmentsFromDB(playerName *string) *string {
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	checkErr(err)
	stmtOut, err := db.Prepare("SELECT equipments FROM playeritem WHERE playername = ?")
	checkErr(err)
	var playerEquipments string
	err = stmtOut.QueryRow(playerName).Scan(&playerEquipments)
	checkErr(err)
	return &playerEquipments
}

func (this *ItemHandler) Items2Json(session *ace.Session) string {
	itemsJson, _ := json.Marshal(this.SessionItems[session])
	//fmt.Println("供持久化的角色物品数据：", string(itemsJson))
	return string(itemsJson)
}

func checkErr(err error) {
	if err != nil {
		panic(err)
	}
}

//下线处理
func (this *ItemHandler) SessionClose(session *ace.Session) {
	//根据session得到角色名字,但一定先判断
	_, ok := data.SyncAccount.SessionPlayer[session]
	if ok {
		lp := data.SyncAccount.SessionPlayer[session].Name //离开的玩家
		db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
		defer db.Close()
		checkErr(err)
		//持久化物品数据
		jsonItems, _ := json.Marshal(this.SessionItems[session]) //[]item 转json字符串
		stmtUp, err := db.Prepare("update playeritem set items=? WHERE playername = ?")
		_, err = stmtUp.Exec(string(jsonItems), lp)
		checkErr(err)
		//持久化装备数据
		jsonEquipments, _ := json.Marshal(this.SessionEquipments[session]) //[]item 转json字符串
		stmtUp, err = db.Prepare("update playeritem set equipments=? WHERE playername = ?")
		_, err = stmtUp.Exec(string(jsonEquipments), lp)
		checkErr(err)
		//持久化技能数据
		jsonSkills, _ := json.Marshal(this.SessionSkills[session]) //转json字符串
		stmtUp, err = db.Prepare("update playeritem set skills=? WHERE playername = ?")
		_, err = stmtUp.Exec(string(jsonSkills), lp)
		checkErr(err)
		//持久化之后删除内存
		delete(this.SessionItems, session)
		delete(this.SessionEquipments, session)
		delete(this.SessionSkills, session)
	}
}
