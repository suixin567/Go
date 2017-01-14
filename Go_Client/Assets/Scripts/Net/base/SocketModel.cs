using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public class SocketModel
	{
		public int Type { get; set;}//表示消息类型
		public int Area { get; set;} //区域码，子模块的标识
		public int Command{ get; set;}
	public string Message{ get; set;}

	public SocketModel(){}

	public SocketModel(int type, int area, int command ,string message)
	{
		this.Type= type;
		this.Area =area;
		this.Command = command;
		this.Message = message;
	}



	}

