using UnityEngine;
using System.Collections.Generic;

public class WindowConstants  {

	public static List<int> windowList=new List<int>();
	public const int INPUT_ERROR=0;
	public const int STATE_ERROR=1;//状态错误，比如在注册界面的操作却不是在注册状态。
	public const int SOCKET_TYPE_FALL=2;//网络消息传输类型错误
	public const int ACC_REG_OK=3;//账号注册成功
	public const int ACC_REG_FALL=4;//账号注册失败
	public const int LOGIN_FALL=5;//登陆失败
	public const int JOB_CREATE_ERR=6;//角色创建失败
	public const int JOB_CREATE_OK=7;//角色创建成功



	public WindowConstants()
	{

	}

}
