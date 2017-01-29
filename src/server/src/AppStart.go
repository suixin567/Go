// AppStart
package main

import (
	"ace"
	_ "bufio"
	//"fmt"
	"game/logic"
	_ "os"
)

func main() {
	//服务器
	server := ace.CreateServer()
	//此Handler即LogicHandler文件
	server.SetHandler(&logic.GameHandler{})
	server.Start(10100)
}
