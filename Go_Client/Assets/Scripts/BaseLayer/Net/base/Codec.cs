using UnityEngine;
using System.Collections;
using System.IO;

public class Codec : MonoBehaviour {

    /// <summary>
    /// 编码
    /// </summary>
    /// <param name="sm"></param>
    /// <returns></returns>
    public static byte[] Encode(SocketModel sm)
    {
        if (sm.Message == null)
        {
            Debug.LogError("正在尝试发送一个为null的网络消息！！！");
            return null;
        }
        ByteArray arr = new ByteArray();
        arr.WriteInt(sm.Message.Length + 16);
        arr.WriteInt(sm.Type);
        arr.WriteInt(sm.Area);
        arr.WriteInt(sm.Command);
        if (sm.Message != null)
        {
            arr.WriteInt(sm.Message.Length);
            arr.WriteUTFBytes(sm.Message);
        }
        byte[] removeZero = new byte[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            removeZero[i] = arr.Buffer[i];
            //Debug.LogWarning(removeZero[i]);
        }
        return removeZero;
    }



    /// <summary>
    /// 解码
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static SocketModel Decode(byte[] bytes)
    {
        MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
        ByteArray arr = new ByteArray(ms);
        int type = arr.ReadInt();
        int area = arr.ReadInt();
        int command = arr.ReadInt();
        string m = arr.ReadUTFBytes((uint)(bytes.Length - arr.Postion));
        //转换为Socket消息模型
        SocketModel model = new SocketModel(); //模型与服务器约定，包含下面4个属性：type\area\command\message
        model.Type = type;
        model.Area = area;
        model.Command = command;
        model.Message = m;
        return model;
    }
}
