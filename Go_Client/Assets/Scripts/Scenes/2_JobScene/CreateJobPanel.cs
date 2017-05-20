using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CreateJobPanel : MonoBehaviour {


	public Text jobText;
	public InputField inputText;
	public int job=-1;

	void Start () {
		gameObject.SetActive (false);
	}
	
	public void selectJob(int job)
	{
		this.job = job;
		switch (job) {
		case 0:
			jobText.text="法师";
			break;
		case 1:
			jobText.text="战士";
			break;
		}
	}
	public void  CreateBtnClick()
	{
		if(GameInfo.GAME_STATE!=GameState.PLAYER_CREATE)
		{
			return;
		}
		if(job==-1 || inputText.text==string.Empty)//没有选择职业或者没有输入昵称直接return;
		{
			WindowConstants.windowList.Add(WindowConstants.JOB_CREATE_ERR);
			return;
		}
		CreateDTO dto =new CreateDTO();
		dto.Acc = GameInfo.ACC_ID;
		dto.Job=job;
		dto.Name=inputText.text;
		string message= Coding<CreateDTO>.encode(dto);
		print("申请创建角色" + message);
		NetWorkManager.instance.sendMessage(Protocol.USER,0,UserProtocol.CREATE_CREQ,message);
		gameObject.SetActive (false);
	//	GameInfo.GAME_STATE = GameState.RUN;
	}
}











