// Buffer
package ace

import (
	"bytes"
	"encoding/binary"
	"fmt"
)

type Buffer struct {
	message []byte
	length  int
	index   int
}

func NewBuffer(in []byte) *Buffer { return &Buffer{in, len(in), 0} }

func NewBuffer1() *Buffer {
	in := make([]byte, 0)
	return &Buffer{in, 0, 0}
}

/*读取int*/
func (b *Buffer) ReadInt() int {
	in := b.message[b.index : b.index+4]
	result := int(in[3]) | (int(in[2]) << 8) | (int(in[1]) << 16) | (int(in[0]) << 24)
	b.index = b.index + 4
	return int(result)
}

/*读取float*/
func (b *Buffer) ReadFloat() float32 {
	in := b.message[b.index : b.index+4]
	var result float32
	buf := bytes.NewBuffer(in)
	err := binary.Read(buf, binary.BigEndian, &result)
	if err != nil {
		fmt.Println("float解析失败", err)
	}
	b.index = b.index + 4
	return result
}

/*读取double*/
func (b *Buffer) ReadDouble() float64 {
	in := b.message[b.index : b.index+8]
	var result float64
	buf := bytes.NewBuffer(in)
	err := binary.Read(buf, binary.BigEndian, &result)
	if err != nil {
		fmt.Println("double解析失败", err)
	}
	b.index = b.index + 8
	return result
}

func (b *Buffer) ReadByte() byte {
	in := b.message[b.index]
	b.index = b.index + 1
	return in
}

/*读取string*/
func (b *Buffer) ReadString() string {
	length := b.ReadInt()
	in := b.message[b.index : b.index+length]
	b.index = b.index + length
	return string(in)
}

func (b *Buffer) ReadBytes() []byte {
	result := b.message[b.index:b.length]
	b.index = b.length
	return result
}

/*写入int*/
func (b *Buffer) WriteInt(value int) {
	by := make([]byte, 4)
	by[3] = byte(value >> 24)
	by[2] = byte(value >> 16)
	by[1] = byte(value >> 8)
	by[0] = byte(value)
	b.message = append(b.message, by...)
	b.length = len(b.message)
}

/*写入byte*/
func (b *Buffer) WriteByte(value byte) {
	b.message = append(b.message, value)
	b.length = len(b.message)
}

/*写入float*/
func (b *Buffer) WriteFloat(value float32) {
	by := make([]byte, 0)
	buf := bytes.NewBuffer(by)
	err := binary.Write(buf, binary.BigEndian, &value)
	if err != nil {
		fmt.Println("float写入失败", err)
	}
	b.message = append(b.message, buf.Bytes()...)
	b.length = len(b.message)
}

/*写入double*/
func (b *Buffer) WriteDouble(value float64) {
	by := make([]byte, 0)
	buf := bytes.NewBuffer(by)
	err := binary.Write(buf, binary.BigEndian, &value)
	if err != nil {
		fmt.Println("double写入失败", err)
	}
	b.message = append(b.message, buf.Bytes()...)
	b.length = len(b.message)
}

/*写入String*/
func (b *Buffer) WriteString(value string) {
	by := []byte(value)
	b.WriteInt(len(by))
	b.message = append(b.message, by...)
	b.length = len(b.message)
}

func (b *Buffer) WriteBytes(value []byte) {
	b.message = append(b.message, value...)
	b.length = len(b.message)
}

/*获取对象的byteArray值*/
func (b *Buffer) Bytes() []byte {
	return b.message
}

func (b *Buffer) Clear() {
	b.message = make([]byte, 0)
	b.length = 0
	b.index = 0
}
func (b *Buffer) Reset() { b.index = 0 }
