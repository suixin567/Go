package Item

import (
	"ace"
	"database/sql"
	"encoding/json"
	"fmt"
	//"game/data"
	"game/logic/protocol"

	_ "github.com/go-sql-driver/mysql"
)

type SkillDTO struct {
	Id            int
	Name          string
	Icon          string
	Des           string
	ApplyType     string
	ApplyProperty string
	ApplyValue    int
	ApplyTime     float32
	Mp            int
	ColdTime      float32
	Job           int
	Level         int
	ReleaseType   string
	Distance      float32
	Shortcut      int
}

type SkillHandler struct {
	Id_Skills   map[int]*SkillDTO    //绑定技能id与技能模型
	Name_Skills map[string]*SkillDTO //绑定技能id与技能模型
}

var SkillHandlerSync = &SkillHandler{Id_Skills: make(map[int]*SkillDTO), Name_Skills: make(map[string]*SkillDTO)}

func init() {
	SkillHandlerSync.initProcess()
}

//服务器初始化技能数据库
func (this *SkillHandler) initProcess() {
	db, err := sql.Open("mysql", "root:@tcp(localhost:3306)/go?charset=utf8")
	defer db.Close()
	checkErr(err)
	rows, err := db.Query("SELECT * FROM skill")
	checkErr(err)
	for rows.Next() {
		var id int
		var name string
		var icon string
		var des string
		var applyType string
		var applyProperty string
		var applyValue int
		var applyTime float32
		var mp int
		var coldTime float32
		var job int
		var level int
		var releaseType string
		var distance float32
		var shortcut int
		err = rows.Scan(&id, &name, &icon, &des, &applyType, &applyProperty, &applyValue, &applyTime, &mp, &coldTime, &job, &level, &releaseType, &distance, &shortcut)
		checkErr(err)
		skillDto := &SkillDTO{id, name, icon, des, applyType, applyProperty, applyValue, applyTime, mp, coldTime, job, level, releaseType, distance, shortcut}
		fmt.Println(skillDto)
		this.Id_Skills[id] = skillDto     //保存物品信息
		this.Name_Skills[name] = skillDto //保存物品信息
	}
}

//学习一个技能
func (this *SkillHandler) LearnSkill(skillName string, session *ace.Session) {
	//所要学的技能
	SkillDTO := this.Name_Skills[skillName]

	//这个人已有的技能
	skillsInfo := ItemHandler.SessionSkills[session]
	//检查是否已经学习
	for _, v := range skillsInfo {
		if v.Id == SkillDTO.Id { //已存在
			session.Write(&ace.DefaultSocketModel{protocol.ITEM, -1, LEARN_SKILL_SRES, []byte("false")})
			return
		}
	}
	skillsInfo = append(skillsInfo, *SkillDTO)
	ItemHandler.SessionSkills[session] = skillsInfo
	//响应是技能模型
	message, _ := json.Marshal(SkillDTO)
	//fmt.Println(string(message))
	session.Write(&ace.DefaultSocketModel{protocol.ITEM, -1, LEARN_SKILL_SRES, message})
}

//为一个技能设置快捷键
func (this *SkillHandler) LearnSkill(skillName string, session *ace.Session) {

}
