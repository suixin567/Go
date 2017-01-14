1、Buffer 引用了bytes 和encoding/binary
对基本类型的读写操作

2、Codec 定义了编码与解码的 两个接口

3、Degault  (实现了Codec中的编解码方法)
定义了 DefaultSocketModel
还有编码方法：
即 把DefaultSocketModel对象编码成[]byte
解码方法 
即 把[]byte变成DefaultSocketModel
还有一个DefaultHandler 此Handler有三个方法：
SessionOpen  SessionClose MessageReceived

4、Session
Session的结构
CreateSession方法
Close方法
SetAttribute方法
RemoveAttribute方法
Write方法
Read方法

5、ServerSocket
Handler接口
ServerSocket结构体
func CreateServer() *ServerSocket {创建服务器
func (server *ServerSocket) Start(port int) {开启服务器，监听网络 一旦有接入就交给clentConnection处理
func clentConnection(session *Session, server *ServerSocket) {客户端连接处理
