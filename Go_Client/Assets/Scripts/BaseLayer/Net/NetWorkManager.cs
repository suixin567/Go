using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NetWorkManager : MonoBehaviour
{
    public static NetWorkManager instance;//单例
    public static int maxReceiveSize = 40000;
    private Socket socket;
    private int port = 10100;
    private byte[] dataCarriage = new byte[maxReceiveSize];//数据车厢
    private byte[] dataTrack = new byte[maxReceiveSize];//数据铁轨
    private int dataTrackIndex = 0;//铁轨当前长度

    private List<SocketModel> messages = new List<SocketModel>();
    static Queue<SocketModel> reSendQueue = new Queue<SocketModel>();//需要重新发送的消息列队
    int temp = 0;//记录编辑器下的会话次数

    public static int NET_STATE = 0;    //网络状态

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //if (PlayerPrefs.GetInt("NetModel") == 1)
        //{
        //    TipManager._instance.setGameTip("内网模式...", 2);
        //}
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(AppConst.SocketUrl, port);
            //异步进行socket的读取,读取完毕之后进行回调                                   ，最后一个参数用于给回调函数传递子线程参数，这里不需要
            socket.BeginReceive(dataCarriage, 0, maxReceiveSize, SocketFlags.None, ReceiveCallBack, null);//最多只能接受maxReceiveSize大小数据
            Debug.Log("连接服务器成功...");
            //开始处理消息
            MessageManager.instance.StartCheckMessage();
        }
        catch (Exception e)
        {
            Debug.Log("连接服务器失败:" + e.Message);
            //退出程序
           // TipManager._instance.setGameTip("\n\n无法连接服务器，点击确定可进入单机模式！\n", 0, true, serverErr);
        }
#if UNITY_EDITOR
        EditorApplication.playmodeStateChanged = stopSendThread;
#endif
    }

#if UNITY_EDITOR
    void stopSendThread()
    {
        if (EditorApplication.isPlaying == false)
            temp++;
        if (temp >= 1)
        {
            Debug.LogWarning("结束发送");
            socket.Close();
        }
    }
#endif

    /// <summary>
    /// 链接服务器错误后的提示的回调
    /// </summary>
    //void serverErr()
    //{
    //    GameObject.Find("LoginCanvas").GetComponent<LoginPanel>().onStandaloneBtnClick();
    //}

    ////打印铁轨
    //string tempa = "";
    //for (int i = 0; i < 200; i++)
    //{
    //    tempa += dataTrack[i];
    //}
    //Debug.LogWarning("整个铁轨数据：" + tempa);
    //sock异步线程读取完毕后调用此方法 形参ar会在异步线程结束后被自动传递过来
    private void ReceiveCallBack(IAsyncResult ar)
    {
        int carriageLen = 0;//单个车厢的长度
        try
        {
            //单个车厢长度
            carriageLen = socket.EndReceive(ar);
            //Debug.Log("车厢长度:" + carriageLen);
            //把车厢摆放到铁轨上
            Buffer.BlockCopy(dataCarriage, 0, dataTrack, dataTrackIndex, carriageLen);
            dataTrackIndex += carriageLen;
            getTrain();
        }
        catch (SocketException)
        {
            socket.Close();
            return;
        }
        //获得服务器消息后 再次继续监听
        socket.BeginReceive(dataCarriage, 0, maxReceiveSize, SocketFlags.None, ReceiveCallBack, dataCarriage);
    }

    //取出一个火车
    void getTrain()
    {
        if (dataTrack.Length < 4)
        {
            return;
        }
        //获取火车头所显示的长度，对列车长度进行比较，确定是否已经是一个完整的列车
        MemoryStream ms = new MemoryStream(dataTrack, 0, 4);
        ByteArray arr = new ByteArray(ms);
        int headLen = arr.ReadInt();
    //    Debug.Log("车头长度:" + headLen);
        //如果已经至少存在一个列车，取出列车，进行逻辑处理
        if (headLen <= dataTrackIndex)
        {
            //Debug.Log("已经足够一个列车");
            byte[] dataTrain = new byte[headLen - 4];
            Buffer.BlockCopy(dataTrack, 4, dataTrain, 0, headLen - 4);//从轨道中获得一个完整列车数据（不包含车头）
                                                                      //重置轨道
            if (dataTrackIndex == headLen)
            {//正好是一条列车
                dataTrackIndex = 0;
              //  Debug.Log("正好是一个列车，所以清空");
            }
            else
            {//列车后面还连着后面的列车
                byte[] temp = new byte[dataTrackIndex - headLen];
                Buffer.BlockCopy(dataTrack, headLen, temp, 0, dataTrackIndex - headLen);//把剩余铁轨保存在临时变量中。
                Buffer.BlockCopy(temp, 0, dataTrack, 0, dataTrackIndex - headLen);
                dataTrackIndex = dataTrackIndex - headLen;
                Debug.LogWarning("递归数据");
                //Debug.LogError("数据长度："+ dataTrackIndex);
                //Debug.LogError("包头长度："+ headLen);
                getTrain();//递归调用
            }
            messages.Add(Codec.Decode(dataTrain));
        }
        else
        {
            //   Debug.Log("列车还不完全");
        }
    }


    #region 关于消息发送
    public void sendMessage(int type, int area, int command, string message)
    {
        SocketModel sm = new SocketModel(type, area, command, message);
        byte[] removeZero = Codec.Encode(sm);
        try
        {
            if (removeZero != null)
            {
                socket.Send(removeZero);
            }
        }
        catch
        {
            Debug.Log("消息发送失败...尝试重连中...");
        //    reSendQueue.Enqueue(sm);//加入重新发送列表
       //     StartCoroutine(reConnectLogic());
        }
    }
    #endregion

    /// <summary>
    /// 断线重连
    /// </summary>
    //IEnumerator reConnectLogic()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        TipManager._instance.setGameTip("与服务器断开连接，尝试重连中...", 2, false, null);
    //        try
    //        {
    //            if (GameInfo.LOGIN_MODEL == 1)
    //            {
    //                yield break;
    //            }
    //            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //            socket.Connect(AppConst.SocketUrl, port);
    //            //异步进行socket的读取,读取完毕之后进行回调                                   ，最后一个参数用于给回调函数传递子线程参数，这里不需要
    //            socket.BeginReceive(dataCarriage, 0, maxReceiveSize, SocketFlags.None, ReceiveCallBack, null);//最多只能接受maxReceiveSize大小数据
    //            TipManager._instance.setGameTip("重连成功...", 4, true, null);
    //            reLogin();//重新登陆
    //            break;
    //        }
    //        catch (Exception e)
    //        {
    //            Debug.Log("重连失败，继续重连:" + e.Message);
    //        }
    //    }
    //}
    /// <summary>
    /// 重新登陆
    /// </summary>
    //void reLogin()
    //{
    //    Debug.Log("发送重新登陆请求...");
    //    LoginDTO dto = new LoginDTO();
    //    dto.userName = GameInfo.ACC_ID;
    //    dto.passWord = GameInfo.ACC_PSD;
    //    string message = Coding<LoginDTO>.encode(dto);
    //    sendMessage(Protocol.LOGIN, 0, LoginProtocol.LOGIN_CREQ, message);
    //}

    /// <summary>
    /// 重新发送上次失败的消息
    /// </summary>
    //public void reSend()
    //{
    //    StartCoroutine(reSendLogic());
    //}

    //IEnumerator reSendLogic()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(Time.deltaTime);
    //        if (reSendQueue.Count > 0)
    //        {
    //            SocketModel sm = reSendQueue.Dequeue();
    //            byte[] removeZero = Codec.Encode(sm);
    //            try
    //            {
    //                if (removeZero != null)
    //                {
    //                    socket.Send(removeZero);
    //                }
    //            }
    //            catch
    //            {
    //                Debug.Log("消息重新发送失败...尝试再次重连中...");
    //                reSendQueue.Enqueue(sm);//加入重新发送列表
    //                StartCoroutine(reConnectLogic());
    //            }
    //        }
    //        else
    //        {//没有需要重新发送的消息后就跳出循环
    //            break;
    //        }
    //    }
    //}


    public List<SocketModel> getList()
    {
        return messages;
    }

}


//using UnityEngine;
//using System.Collections;
//using System.Net.Sockets;
//using System.IO;
//using System;
//using System.Text;
//using System.Threading;
//using System.Collections.Generic;
//#if UNITY_EDITOR
//using UnityEditor;
//#endif
//public  class NetWorkManager
//{
//	private static NetWorkManager script;//单例
//	public static int maxReceiveSize = 5000;
//	private Socket socket;
//	private string host = "127.0.0.1";
//	private int port = 10100;
//	private byte[] readM = new byte[maxReceiveSize];
//	private List<SocketModel> messages = new List<SocketModel>();
//	static Queue<SocketModel> sendQueue = new Queue<SocketModel>();//发送列队
//	public Thread sendThread;

//	public int temp =0;



//	public static NetWorkManager getInstance() {
//		if (script == null) {
//			script = new NetWorkManager();
//			script.init();
//		}
//		return script;
//	}

//	private void init() {
//		try
//		{
//			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//			socket.Connect(host, port);
//			//异步进行socket的读取,读取完毕之后进行回调                                   ，最后一个参数用于给回调函数传递子线程参数，这里不需要
//			socket.BeginReceive(readM, 0, maxReceiveSize, SocketFlags.None, ReceiveCallBack,null);//最多只能接受maxReceiveSize大小数据
//			Debug.Log("连接服务器成功...");
//		}
//		catch (Exception e) {
//			Debug.Log("连接服务器失败:"+e.Message);
//		}

//		sendThread = new Thread(send){IsBackground =true};
//		sendThread.Start();
//		#if UNITY_EDITOR
//		EditorApplication.playmodeStateChanged = stopSendThread;
//		#endif
//	}
//	#if UNITY_EDITOR
//	void stopSendThread()
//	{
//		if(EditorApplication.isPlaying ==false)
//		temp++;
//		if(temp>=1)
//		{
//			Debug.LogWarning("结束发送");
//			sendThread.Abort();
//		}
//	}
//	#endif

//	//sock异步线程读取完毕后调用此方法 形参ar会在异步线程结束后被自动传递过来
//	private void ReceiveCallBack(IAsyncResult ar)
//	{
//	//	Debug.Log("收到消息");
//		int readCount = 0;
//		try
//		{
//			//读取消息长度
//			readCount = socket.EndReceive(ar);//调用这个函数来结束本次接收并返回接收到的数据长度。 
//			if (readCount>=maxReceiveSize)
//			{
//				Debug.LogError("收到超长数据");
//			}
//			byte[] bytes = new byte[readCount];//创建长度对等的bytearray用于接收
//			Buffer.BlockCopy(readM, 0, bytes, 0, readCount);
//			readMessage(bytes);
//		}
//		catch (SocketException)
//		{
//			socket.Close();
//			return;
//		}
//		//获得服务器消息后 再次继续监听
//		socket.BeginReceive(readM, 0, maxReceiveSize, SocketFlags.None, ReceiveCallBack,readM);	
//	}

//	//读取上面方法所收到的信息
//	public void readMessage(byte[] bytes)
//	{
//		MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
//		ByteArray arr = new ByteArray(ms);
//		int type = arr.ReadInt();
//		int area = arr.ReadInt();
//		int command = arr.ReadInt();
//		string m = arr.ReadUTFBytes((uint)(bytes.Length - arr.Postion));
//		//转换为Socket消息模型
//		SocketModel model= new SocketModel(); //模型与服务器约定，包含下面4个属性：type\area\command\message
//		model.Type = type;
//		model.Area = area;
//		model.Command = command;
//		model.Message = m;
//		//消息接收完毕后，存入收到消息队列
//		messages.Add(model);
//	}

//	public void sendMessage(int type, int area, int command, string message) {
//		SocketModel sm = new SocketModel(type, area, command, message);
//		sendQueue.Enqueue(sm);
//	}

//	void send()
//	{
//		while (true)
//		{
//		//	Debug.Log("线程");
//			if (sendQueue.Count > 0)
//			{
//				SocketModel sm = sendQueue.Dequeue();
//				try
//				{
//					ByteArray arr = new ByteArray();
//					arr.WriteInt(sm.Message.Length+16);
//					arr.WriteInt(sm.Type);
//					arr.WriteInt(sm.Area);
//					arr.WriteInt(sm.Command);
//					if (sm.Message != null)
//					{
//						arr.WriteInt(sm.Message.Length);//20
//						arr.WriteUTFBytes(sm.Message);
//					}
//					//Debug.Log(arr.Length);
//					//Debug.Log(arr.Buffer.Length);
//					byte[] removeZero = new byte[arr.Length];

//					for(int i=0;i<arr.Length;i++ )
//					{
//						removeZero[i]=arr.Buffer[i];
//						//Debug.LogWarning(removeZero[i]);
//					}
//					socket.Send(removeZero);
//				}
//				catch (System.Exception ex)
//				{
//					UnityEngine.Debug.LogError(ex.Message);
//				}
//			}
//			Thread.Sleep(1);
//		}
//	}

//	public List<SocketModel> getList() {
//		return messages;
//	}
//}