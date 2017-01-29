using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public  class NetWorkScript
{
	private static NetWorkScript script;//单例
	public static int maxReceiveSize = 5000;
	private Socket socket;
	private string host = "127.0.0.1";
	private int port = 10100;
	private byte[] readM = new byte[maxReceiveSize];
	private List<SocketModel> messages = new List<SocketModel>();
	static Queue<SocketModel> sendQueue = new Queue<SocketModel>();//发送列队
	public Thread sendThread;

	public int temp =0;



	public static NetWorkScript getInstance() {
		if (script == null) {
			script = new NetWorkScript();
			script.init();
		}
		return script;
	}

	private void init() {
		try
		{
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(host, port);
			//异步进行socket的读取,读取完毕之后进行回调                                   ，最后一个参数用于给回调函数传递子线程参数，这里不需要
			socket.BeginReceive(readM, 0, maxReceiveSize, SocketFlags.None, ReceiveCallBack,null);//最多只能接受maxReceiveSize大小数据
			Debug.Log("连接服务器成功...");
		}
		catch (Exception e) {
			Debug.Log("连接服务器失败:"+e.Message);
		}

		sendThread = new Thread(send){IsBackground =true};
		sendThread.Start();
		#if UNITY_EDITOR
		EditorApplication.playmodeStateChanged = stopSendThread;
		#endif
	}
	#if UNITY_EDITOR
	void stopSendThread()
	{
		if(EditorApplication.isPlaying ==false)
		temp++;
		if(temp>=1)
		{
			Debug.LogWarning("结束发送");
			sendThread.Abort();
		}
	}
	#endif

	//sock异步线程读取完毕后调用此方法 形参ar会在异步线程结束后被自动传递过来
	private void ReceiveCallBack(IAsyncResult ar)
	{
	//	Debug.Log("收到消息");
		int readCount = 0;
		try
		{
			//读取消息长度
			readCount = socket.EndReceive(ar);//调用这个函数来结束本次接收并返回接收到的数据长度。 
			if (readCount>=maxReceiveSize)
			{
				Debug.LogError("收到超长数据");
			}
			byte[] bytes = new byte[readCount];//创建长度对等的bytearray用于接收
			Buffer.BlockCopy(readM, 0, bytes, 0, readCount);
			readMessage(bytes);
		}
		catch (SocketException)
		{
			socket.Close();
			return;
		}
		//获得服务器消息后 再次继续监听
		socket.BeginReceive(readM, 0, maxReceiveSize, SocketFlags.None, ReceiveCallBack,readM);	
	}

	//读取上面方法所收到的信息
	public void readMessage(byte[] bytes)
	{
		MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
		ByteArray arr = new ByteArray(ms);
		int type = arr.ReadInt();
		int area = arr.ReadInt();
		int command = arr.ReadInt();
		string m = arr.ReadUTFBytes((uint)(bytes.Length - arr.Postion));
		//转换为Socket消息模型
		SocketModel model= new SocketModel(); //模型与服务器约定，包含下面4个属性：type\area\command\message
		model.Type = type;
		model.Area = area;
		model.Command = command;
		model.Message = m;
		//消息接收完毕后，存入收到消息队列
		messages.Add(model);
	}

	public void sendMessage(int type, int area, int command, string message) {
		SocketModel sm = new SocketModel(type, area, command, message);
		sendQueue.Enqueue(sm);
	}

	void send()
	{
		while (true)
		{
		//	Debug.Log("线程");
			if (sendQueue.Count > 0)
			{
				SocketModel sm = sendQueue.Dequeue();
				try
				{
					ByteArray arr = new ByteArray();
					arr.WriteInt(sm.Type);
					arr.WriteInt(sm.Area);
					arr.WriteInt(sm.Command);
					if (sm.Message != null)
					{
						arr.WriteInt(sm.Message.Length);
						arr.WriteUTFBytes(sm.Message);
					}
					socket.Send(arr.Buffer);
				}
				catch (System.Exception ex)
				{
					UnityEngine.Debug.LogError(ex.Message);
				}
			}
			Thread.Sleep(1);
		}
	}

	public List<SocketModel> getList() {
		return messages;
	}
}

//using UnityEngine;
//using System.Collections;
//using System.Net.Sockets;
//using System.IO;
//using System;
//using System.Text;
////using LitJson;
//using System.Threading;
//using System.Collections.Generic;
//
//public  class NetWorkScript 
//{
//	public const int MessageMaxSize =8192;//最大数据尺寸
//	private static NetWorkScript script;
//	private Socket socket;
//	private string host = "127.0.0.1";//服务器IP地址
//	private int port = 10100;//服务器端口
//	private byte[] readM = new byte[MessageMaxSize];//从服务器收到的字节信息
//	
//	private List<SocketModel> messages = new List<SocketModel>();
//	
//	//获取连接对象
//	public static NetWorkScript getInstance() {
//		if (script == null) {
//			//第一次调用的时候 创建单例对象 并进行初始化操作
//			script = new NetWorkScript();
//			script.init();
//		}
//		return script;
//	}
//
//	private void init() {
//		try
//		{
//			//创建socket连接对象
//			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//			//连接到服务器
//			socket.Connect(host, port);
//			//连接后开始从服务器读取网络消息！！！
//			socket.BeginReceive(readM, 0, MessageMaxSize, SocketFlags.None, ReceiveCallBack,readM);
//			Debug.Log("unity：连接服务器成功...");
//		}
//		catch (Exception e) {
//			//连接失败 打印异常
//			Debug.Log("unity：连接服务器失败:"+e.Message);
//		}
//	}
//
//
//	//这是读取服务器消息的回调--当有消息过来的时候BgenReceive方法会回调此函数
//	private void ReceiveCallBack(IAsyncResult ar)
//	{
//		int readCount = 0;
//		try
//		{
//			//读取消息长度
//			readCount = socket.EndReceive(ar);//调用这个函数来结束本次接收并返回接收到的数据长度。
//			if(readCount>MessageMaxSize-1)
//			{
//				Debug.LogWarning("收到大尺寸信息，长度:"+readCount);
//			}
//			byte[] bytes = new byte[readCount];//创建长度对等的bytearray用于接收
//			Buffer.BlockCopy(readM, 0, bytes, 0, readCount);//拷贝读取的消息到 消息接收数组
//			readMessage(bytes);//消息读取完成
//		}
//		catch (SocketException)//出现Socket异常就关闭连接 
//		{
//			socket.Close();//这个函数用来关闭客户端连接 
//			return;
//		}
//		//获得服务器消息后 再次继续监听！！！
//		socket.BeginReceive(readM, 0, MessageMaxSize, SocketFlags.None, ReceiveCallBack,readM);	
//	}
//
//	//读取上面方法所收到的信息
//	public void readMessage(byte[] bytes)
//	{
//		//消息读取完成后开始解析 
//		MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
//		ByteArray arr = new ByteArray(ms);
//		int type = arr.ReadInt();//表示消息类型  我们这里有两种
//		int area = arr.ReadInt();//这里表示消息的区域码 在登录这样的服务器单例模块中 没有效果 在地图消息的时候用于区分属于哪张地图来的消息
//		int command = arr.ReadInt();//模块内部协议---具体稍后描述
//		string m = arr.ReadUTFBytes((uint)(bytes.Length - arr.Postion));//这里开始就是读取服务器传过来的消息对象了 是一串json字符串
//		//转换为Socket消息模型
//		SocketModel model= new SocketModel(); //这个模型是与服务器约定好的，包含下面4个属性：type\area\command\message
//		model.Type = type;
//		model.Area = area;
//		model.Command = command;
//		model.Message = m;
//		//消息接收完毕后，存入收到消息队列
//		messages.Add(model);
//	}
//
//
//	public void sendMessage(int type, int area, int command, string message) {
//		ByteArray arr= new ByteArray();
//		arr.WriteInt(type);
//		arr.WriteInt(area);
//		arr.WriteInt(command);
//		if (message != null)
//		{
//			arr.WriteInt(message.Length);
//			arr.WriteUTFBytes(message);
//		}
//		socket.Send(arr.Buffer);
//	}
//	
//
//
//
//
//
//
//
//	public List<SocketModel> getList() {
//		return messages;
//	}
//	
//}
