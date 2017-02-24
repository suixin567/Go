using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class regPanel : MonoBehaviour {

	public InputField accountInput;
	public InputField passWordInput;

	void Start () {
		gameObject.SetActive (false);
	}
	public void OnClick()
	{
		if(GameInfo.GAME_STATE!=GameState.ACC_REG)//点击了提交注册按钮，却不是在注册状态。
		{
			WindowConstants.windowList.Add(WindowConstants.STATE_ERROR);
			return;
		}

		if (accountInput.text != string.Empty && passWordInput.text != string.Empty) {
			//T送 注册数据
			LoginDTO dto =new LoginDTO();
			dto.userName=accountInput.text;
			dto.passWord=passWordInput.text;
			string message = Coding<LoginDTO>.encode(dto);
//			print("json格式的注册账号与密码"+message );
			NetWorkScript.getInstance().sendMessage(Protocol.LOGIN, 2,LoginProtocol.REG_CREQ,message);
		} else 
		{
			WindowConstants.windowList.Add(WindowConstants.INPUT_ERROR);
		}
		gameObject.SetActive (false);
		GameInfo.GAME_STATE = GameState.RUN;//将游戏状态切换回RUN
	}

}
