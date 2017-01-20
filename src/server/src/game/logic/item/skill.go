package item

import (
	"database/sql"
	"fmt"

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
}

type SkillHandler struct {
	Skills map[string]*SkillDTO //绑定技能名字与技能模型
}

var SkillSync = &SkillHandler{Skills: make(map[string]*SkillDTO)}

func init() {
	SkillSync.initProcess()
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
		err = rows.Scan(&id, &name, &icon, &des, &applyType, &applyProperty, &applyValue, &applyTime, &mp, &coldTime, &job, &level, &releaseType, &distance)
		checkErr(err)
		skillDto := &SkillDTO{id, name, icon, des, applyType, applyProperty, applyValue, applyTime, mp, coldTime, job, level, releaseType, distance}
		fmt.Println(skillDto)
		this.Skills[name] = skillDto //保存物品信息
	}
}
