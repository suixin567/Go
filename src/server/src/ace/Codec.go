// Codec
package ace

/**
编码器接口
*/
type Encode interface {
	Encode(object interface{}) []byte
}

/**
解码器接口
*/
type Decode interface {
	Decode(msg []byte) interface{}
}
