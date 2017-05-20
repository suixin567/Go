// Default
package ace

// "fmt"

type DefaultDecode struct {
}

type DefaultEncode struct {
}

type DefaultHandler struct {
}

type DefaultSocketModel struct {
	Type    int
	Area    int
	Command int
	Message []byte
}

//编码
func (encode *DefaultEncode) Encode(msg interface{}) []byte {
	m := msg.(*DefaultSocketModel)
	buffer := NewBuffer1()
	//消息长度信息
	buffer.WriteInt(len(m.Message) + 16)
	//fmt.Println("包头长度------>", len(m.Message)+16)
	//四个主体信息
	buffer.WriteInt(m.Type)
	buffer.WriteInt(m.Area)
	buffer.WriteInt(m.Command)
	buffer.WriteBytes(m.Message)
	//fmt.Println("编码------》 ：把对象转成[]byte")
	return buffer.Bytes()
}

//解码
func (decode *DefaultDecode) Decode(msg []byte) interface{} {
	if len(msg) < 4 {
		return nil
	}
	buffer := NewBuffer(msg)
	Type := buffer.ReadInt()
	Area := buffer.ReadInt()
	Command := buffer.ReadInt()
	Message := buffer.ReadString()
	//	fmt.Println("解码------》 ：把[]byte转成对象", Message)
	return DefaultSocketModel{Type, Area, Command, []byte(Message)}
}

//这三个方法实现了ServerSocket中的Handler接口 是最默认的服务器Handler,本程序中使用了LogicHanlder，没有使用这三个默认的。
func (handler *DefaultHandler) SessionOpen(session *Session) {
}
func (handler *DefaultHandler) SessionClose(session *Session) {
}
func (handler *DefaultHandler) MessageReceived(session *Session, message interface{}) {
}
