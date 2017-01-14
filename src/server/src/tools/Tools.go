// SyncServer
package tools

import (
	"ace"
	"fmt"
	"strconv"
)

type Vector3 struct {
	X float64
	Y float64
	Z float64
}

type PlayerDTO struct {
	Id    int
	Name  string
	Map   int
	Point Vector3
}

type Sync struct {
	players map[*ace.Session]*PlayerDTO
	id      int
}

//make是在创建map,这个map key的类型是*ace.Session ，值的类型是*PlayerDTO，使用make创建的map是已经初始化了的
//声明一个strct类型时建议直接创建它的指针，所以使用了取地址符
var SyncServ = &Sync{players: make(map[*ace.Session]*PlayerDTO), id: 0}

//接收session，返回一个playerDTO指针
func (this *Sync) Create(session *ace.Session) *PlayerDTO {
	this.id += 1 //代表第多少个人
	var id = this.id
	var name = "test" + strconv.Itoa(id)
	var mapIndex = 0
	var position = Vector3{0, 1.14, 3}
	//创建一个playerDTO
	result := &PlayerDTO{id, name, mapIndex, position}
	this.players[session] = result
	return result
}

func (this *Sync) GetRole(session *ace.Session) *PlayerDTO {
	fmt.Println(this.players[session])
	return this.players[session]
}
