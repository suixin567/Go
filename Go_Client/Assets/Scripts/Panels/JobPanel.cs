using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JobPanel : MonoBehaviour
{
	public static JobPanel instance;

	public GameObject createPanel;
	public Button[] jobBtns;
	public Image selectedJobBtn;
	public GameObject startButton;
	public GameObject deleteButton;
	PlayerModel[] players;//此账号下的角色数组
	void Awake()
	{
		instance=this;
	}

	void Start()
	{
		//没有获取角色列表之前先关闭这两个按钮
		startButton.gameObject.SetActive(false);
		deleteButton.gameObject.SetActive(false);

		string message = Coding<StringDTO>.encode (new StringDTO(GameInfo.ACC_ID));
		NetWorkScript.getInstance ().sendMessage (Protocol.USER,0,UserProtocol.LIST_CREQ,message);
        print("拉取角色列表");
	}

	//	初始化职业列表的职业按钮：修改职业按钮的图片、昵称、等级
	public void InitJobButton(PlayerModel[] _players)
	{
		players = _players;
		if(players!=null){
		for(int i=0 ;i<_players.Length;i++)
		{
			jobBtns[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/job"+ _players[i].Job);
			jobBtns[i].transform.FindChild("nameText").GetComponent<Text>().text = "昵称：" + _players[i].Name;
			jobBtns[i].transform.FindChild("levelText").GetComponent<Text>().text = "等级：" + _players[i].Level;
		}
		}
		GameInfo.GAME_STATE = GameState.RUN;
	}


	//打开创建角色的面板
	public void createBtnClick() {
		if(GameInfo.GAME_STATE!=GameState.RUN){return;}
		if(players != null)//之前没角色
		{
			if(players.Length>=2)
			{
				return;
			}
		}

		GameInfo.GAME_STATE = GameState.PLAYER_CREATE;
		createPanel.SetActive (true);
	}


	//当职业按钮被点击  切换所选择的角色模型
	public void onJobBtnClick(int btnId)
	{
		if(GameInfo.GAME_STATE!=GameState.RUN){return;}
		if(players == null) return;//之前没角色 ***

		if(btnId ==0 && players.Length>0)//职业1的按钮被选择
		{
			GameInfo.myPlayerModel = players[0];
			selectedJobBtn.GetComponent<Image>().sprite = jobBtns[0].GetComponent<Image>().sprite;
			selectedJobBtn.transform.FindChild("Text").GetComponent<Text>().text="已选择"+players[0].Name;//把尚未选择清空
			startButton.gameObject.SetActive(true);
		}else if(btnId ==1 && players.Length>1)
		{
			GameInfo.myPlayerModel = players[1];
			selectedJobBtn.GetComponent<Image>().sprite = jobBtns[1].GetComponent<Image>().sprite;
			selectedJobBtn.transform.FindChild("Text").GetComponent<Text>().text="已选择"+players[1].Name;//把尚未选择清空
			startButton.gameObject.SetActive(true);
		}
	}

	//游戏开始按钮
	public void GameStart()
	{
		if(GameInfo.GAME_STATE!=GameState.RUN){return;}
		if(GameInfo.myPlayerModel==null){return;}
		string message = Coding<StringDTO>.encode (new StringDTO (GameInfo.myPlayerModel.Name));
		//	message="{\"value\":\"8\"}";//手写json格式
		NetWorkScript.getInstance ().sendMessage (Protocol.USER,0,UserProtocol.SELECT_CREQ,message);
		GameInfo.GAME_STATE = GameState.WAIT;//进入等待状态
	}

}
