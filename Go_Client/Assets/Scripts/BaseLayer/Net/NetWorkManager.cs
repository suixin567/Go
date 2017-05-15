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
public  class NetWorkManager
{
	private static NetWorkManager script;//单例
	public static int maxReceiveSize = 5000;
	private Socket socket;
	private string host = "127.0.0.1";
	private int port = 10100;
	private byte[] readM = new byte[maxReceiveSize];
	private List<SocketModel> messages = new List<SocketModel>();
	static Queue<SocketModel> sendQueue = new Queue<SocketModel>();//发送列队
	public Thread sendThread;

	public int temp =0;



	public static NetWorkManager getInstance() {
		if (script == null) {
			script = new NetWorkManager();
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
					arr.WriteInt(sm.Message.Length+16);
					arr.WriteInt(sm.Type);
					arr.WriteInt(sm.Area);
					arr.WriteInt(sm.Command);
					if (sm.Message != null)
					{
						arr.WriteInt(sm.Message.Length);//20
						arr.WriteUTFBytes(sm.Message);
					}
					//Debug.Log(arr.Length);
					//Debug.Log(arr.Buffer.Length);
					byte[] removeZero = new byte[arr.Length];

					for(int i=0;i<arr.Length;i++ )
					{
						removeZero[i]=arr.Buffer[i];
						//Debug.LogWarning(removeZero[i]);
					}
					socket.Send(removeZero);
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