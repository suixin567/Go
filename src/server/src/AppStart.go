// AppStart
package main

import (
	"ace"
	_ "bufio"
	"game/logic"
	_ "os"
)

func main() {
	server := ace.CreateServer()
	//此Handler即LogicHandler文件
	server.SetHandler(&logic.GameHandler{})
	server.Start(10100)
}
