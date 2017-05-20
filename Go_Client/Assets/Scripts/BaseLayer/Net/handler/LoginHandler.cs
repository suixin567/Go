using UnityEngine;
using System.Collections;

public class LoginHandler : MonoBehaviour {

	public void OnMessage (SocketModel model)
	{
//		Debug.Log ("model.Type："+model.Type);
//		Debug.Log ("model.Area："+model.Area);
//		Debug.Log ("model.Command："+model.Command);
//		Debug.Log ("model.Message："+model.Message);

		switch(model.Command)
		{
		case LoginProtocol.REG_SRES:
			RegResult(model.Message);
			break;
		case LoginProtocol.LOGIN_SRES:
			LoginResult(model.Message);
			break;
		default:
			break;
		}
	}

	public void RegResult(string message)
	{
        //恢复状态
        NetWorkManager.NET_STATE = NetState.RUN;

        if (message == "true")
		{
			print("注册成功");
			WindowConstants.windowList.Add (WindowConstants.ACC_REG_OK);
		}else if(message =="false")
		{
			print("注册失败");
			WindowConstants.windowList.Add (WindowConstants.ACC_REG_FALL);
			GameInfo.GAME_STATE = GameState.RUN;
			return;
		}else{
			print("错误的注册结果");
			WindowConstants.windowList.Add (WindowConstants.ACC_REG_FALL);
			return;
		}
	}

	public void LoginResult(string message)
	{
        NetWorkManager.NET_STATE = NetState.RUN;

        if (message == "true")
		{
			print("登陆成功");
			//登陆成功的话就 加载进度条，并跳转到角色选择场景
			ResceneManager.instance.Loading(1);
		}else if(message=="false"){
			print("登录失败");
			WindowConstants.windowList.Add(WindowConstants.LOGIN_FALL);
			GameInfo.GAME_STATE = GameState.RUN;
			return;
		}else{
			print("错误的登陆结果");
			WindowConstants.windowList.Add(WindowConstants.LOGIN_FALL);
			return;
		}
	}


}
