// ServerSocket
package ace

import (
	"fmt"
	"net"
	"runtime"
)

/**
逻辑处理接口 在Default文件中去实现这个接口
*/
type Handler interface {
	SessionOpen(session *Session)
	SessionClose(session *Session)
	MessageReceived(session *Session, message interface{})
}

type ServerSocket struct {
	encode  Encode
	decode  Decode
	handler Handler //服务器的handler
}

//程序最开始调用的  创建服务器
func CreateServer() *ServerSocket {
	encode := &DefaultEncode{}
	decode := &DefaultDecode{}
	handler := &DefaultHandler{}
	return &ServerSocket{encode, decode, handler}
}

//程序最开始调用的  开启服务器
func (server *ServerSocket) Start(port int) {
	runtime.GOMAXPROCS(runtime.NumCPU())
	addr := fmt.Sprintf(":%d", port)
	listener, err := net.Listen("tcp", addr) //开始网络监听
	if err != nil {
		fmt.Printf("Error: %v\n", err)
		fmt.Printf("Exiting...\n")
		return
	}
	//服务器关闭时 关闭端口监听
	defer listener.Close()
	fmt.Println("服务器端口号:", addr)
	for {
		conn, err := listener.Accept() //一直等待客户端的接入
		if err != nil {
			fmt.Printf("Error: %v\n", err)
			fmt.Printf("Exiting...\n")
			return
		}
		session := CreateSession(conn, server.encode) //一旦有接入就创建Session，并在协程中处理
		go clentConnection(session, server)
	}
}

func clentConnection(session *Session, server *ServerSocket) {
	server.handler.SessionOpen(session) //调用Handler的SessionOpen()方法
	buffer := make([]byte, 1024)
	//让session读取信息
	for bytelength, readSuccess := session.Read(buffer); readSuccess; bytelength, readSuccess = session.Read(buffer) {
		temp := buffer[0:bytelength]
		msg := server.decode.Decode(temp)
		if msg != nil {
			server.handler.MessageReceived(session, msg) //接收到数据
		} else {
			session.Close()
		}
	}
	server.handler.SessionClose(session) //调用Handler的SessionClose()方法
}

//程序最开始调用的设置逻辑路由
func (server *ServerSocket) SetHandler(handler Handler) {
	server.handler = handler
}

//这俩方法还没有用到，先注释掉
//func (server *ServerSocket) SetEncode(encode Encode) {
//	server.encode = encode
//}

//func (server *ServerSocket) SetDecode(decode Decode) {
//	server.decode = decode
//}
