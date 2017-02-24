using UnityEngine;
using System.Collections;

public class WindowManager : MonoBehaviour {
	public GameObject alertPanel;
	private AlertPanel script;


	void Start () {
		script=alertPanel.GetComponent<AlertPanel>();
	}
	


	void Update () {
	if(WindowConstants.windowList.Count>0)
		{
			int type = WindowConstants.windowList[0];
			OnWindow(type);
			WindowConstants.windowList.RemoveAt(0);
		}
	}

	public void OnWindow(int type)
	{


		switch(type)
		{
		case WindowConstants.INPUT_ERROR:
			script.setMessage("输入错误，请重新输入");
			break;
		case WindowConstants.SOCKET_TYPE_FALL:
			script.setMessage("网络消息传输类型错误");
			break;
		case WindowConstants.ACC_REG_OK:
			script.setMessage("注册成功");
			break;
		case WindowConstants.ACC_REG_FALL:
			script.setMessage("注册失败，此账号已注册");
			break;
		case WindowConstants.LOGIN_FALL:
			script.setMessage("登陆失败");
			break;
		case WindowConstants.JOB_CREATE_ERR:
			script.setMessage("此昵称已存在，角色创建失败");
			break;
		default:
			script.setMessage("未知错误");
			break;
		}
		GameInfo.LAST_STATE = GameInfo.GAME_STATE;
		GameInfo.GAME_STATE = GameState.WINDOW;
		alertPanel.SetActive (true);
	}
}
