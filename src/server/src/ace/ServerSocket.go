// ServerSocket
package ace

import (
	"bytes"
	"encoding/binary"
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

var count int

func clentConnection(session *Session, server *ServerSocket) {
	server.handler.SessionOpen(session) //调用Handler的SessionOpen()方法
	//数据缓存
	databuf := make([]byte, 10240)
	// 消息缓冲
	msgbuf := bytes.NewBuffer(make([]byte, 0))
	// 消息长度
	length := 0
	// 消息长度uint32
	ulength := uint32(0)
	//不断的从连接中读取信息
	for bytelength, readSuccess := session.Read(databuf); readSuccess; bytelength, readSuccess = session.Read(databuf) {
		//fmt.Println("收到的byte数组长度:", bytelength, "值:", databuf[:bytelength])
		_, err := msgbuf.Write(databuf[:bytelength]) // 数据添加到消息缓冲
		if err != nil {
			fmt.Printf("Buffer write error: %s\n", err)
			return
		}
		for { // 消息分割循环
			// 消息头
			if length == 0 && msgbuf.Len() >= 4 {
				binary.Read(msgbuf, binary.BigEndian, &ulength) //从msgbuf中读取  然后放入ulength
				length = int(ulength)
				//fmt.Println("消息头中所写的长度", ulength)
				count++
				//fmt.Println("收到信息条数", count)
				// 检查超长消息
				if length > 10240 {
					fmt.Printf("Message too length: %d\n", length)
					return
				}
			}
			// 消息体
			if length > 0 && msgbuf.Len() >= length {
				//fmt.Printf(": %s\n", string(msgbuf.Next(length)))
				msg := server.decode.Decode([]byte(string(msgbuf.Next(length))))
				if msg != nil {
					server.handler.MessageReceived(session, msg) //接收到数据，去进行逻辑处理
				} else {
					fmt.Println("别运行啊!!!!!!!")
					session.Close()
				}
				length = 0
			} else {
				break
			}
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
