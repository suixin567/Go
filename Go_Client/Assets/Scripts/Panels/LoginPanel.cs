using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {

	public GameObject regPanel;



	public InputField accText;
	public InputField pwdText;

	public void regClick()
	{
		if(GameInfo.GAME_STATE==GameState.RUN)
		{
			GameInfo.GAME_STATE=GameState.ACC_REG;
			regPanel.SetActive (true);
		}
	}


	public void LoginClick()
	{
//		print(GameInfo.GAME_STATE);
		if(GameInfo.GAME_STATE!=GameState.RUN)
		{
			return;
		}
		if (accText.text != string.Empty && pwdText.text != string.Empty) {
			GameInfo.ACC_ID = accText.text;
			//发送登陆到服务器
			LoginDTO dto =new LoginDTO();
			dto.userName=accText.text;
			dto.passWord=pwdText.text;
			string message = Coding<LoginDTO>.encode(dto);
			NetWorkScript.getInstance().sendMessage(Protocol.LOGIN,0,LoginProtocol.LOGIN_CREQ,message);
			GameInfo.GAME_STATE=GameState.WAIT;
		} else 
		{
			WindowConstants.windowList.Add(WindowConstants.INPUT_ERROR);
		}
	}
}
