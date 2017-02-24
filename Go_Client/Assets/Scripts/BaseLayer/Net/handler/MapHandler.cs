﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapHandler : MonoBehaviour {

	private bool isLoading =false;
	public GameObject[] playerPres;//角色模型
	//保存此场景下所有角色的列表
//	public Dictionary<string ,PlayerModel> playerModelList = new Dictionary<string, PlayerModel>();
	public Dictionary<string ,GameObject> playerGoList = new Dictionary<string, GameObject>();
	public Dictionary<string ,GameObject> MonList = new Dictionary<string, GameObject>();//怪物列表 格式为： 1_2  monModel
    public Transform monsterHolder;


	public void OnMessage(SocketModel model)
	{
		switch (model.Command){
		case MapProtocol.ENTER_SRES://服务器返回当前场景所有玩家集合
			selfEnter(model.Message);
			break;
		case MapProtocol.ENTER_BRO://其他玩家进入地图之后此客户端的逻辑
			otherEnter(model.Message);
			break;
		case MapProtocol.MOVE_BRO:
			move(model.Message);
			break;
		case MapProtocol.LEAVE_BRO:
			leave(model.Message);
			break;
            //初始化怪物
            case MapProtocol.MONSTER_INIT_SRES:
                initMons(model.Message);
                break;
		case MapProtocol.BE_ATTACK_BRO:
			beAttack(model.Message);
			break;

		case MapProtocol.TALK_BRO:
			chat(model.Message);
			break;
		}
	}

	void beAttack(string message)
	{
		print("怪物血量"+message);
		MonsterModel mon = Coding<MonsterModel>.decode(message);
		string monIndex = mon.FirstIndex.ToString() +"_" +mon.SecondIndex.ToString();
		if(mon.Hp<=0)
		{
			Destroy(MonList[monIndex]);
			MonList.Remove(monIndex);
		}else{//更新怪物血条
			MonList[monIndex].GetComponent<MonsterBase>().monModel.Hp = mon.Hp;
			print("更新怪物属性"+mon.Hp);
		}
	}
    //进入新地图时，初始化此地图里的所有怪
    void initMons(string message) {
        MonsterModel mon = Coding<MonsterModel>.decode(message);
        LoadData.loadingMonsterList.Add(mon);
    }

	void chat(string message)
	{
		
	}


	void leave(string message){
		Debug.Log("这个人离开了：" + message);
	//	StringDTO playerName = Coding<StringDTO>.decode("{\"value\":"+message +"}");
		if(/*playerModelList.ContainsKey(message) ==false || */playerGoList.ContainsKey(message)==false)
		{
			return;
		}
		//if(playerModelList[message]!= null)
		//{
		//	playerModelList.Remove(message);
		//}
		if(playerGoList[message]!= null)
		{
			GameObject go = playerGoList[message];
			playerGoList.Remove(message);
			Destroy(go);
		}
	}

	private void selfEnter(string message){
		PlayerModel[] players = Coding<PlayerModel[]>.decode(message);
		Debug.Log("我进入地图，这个图中一共有这么多个人：" + players.Length);
		LoadData.loadingPlayerList.AddRange(players);
		isLoading =true;
		//切换场景
		ResceneManager.instance.Loading(GameInfo.myPlayerModel.Map);
	}

	//其他人进入地图
	private void otherEnter(string message){
		PlayerModel player = Coding<PlayerModel>.decode(message);
		if(isLoading){
			LoadData.loadingPlayerList.Add(player);
		}else{
			//对此角色实例化
			creatPlayer(player);
		}
	}


	void OnLevelWasLoaded(int level){
		if(level <2)
		{
			return;
		}
		if(level != GameInfo.myPlayerModel.Map) return;
		//开始解析角色列表，并生成自己在内的所有对象
		foreach (PlayerModel model in LoadData.loadingPlayerList){
			creatPlayer(model);
		}
		LoadData.loadingPlayerList.Clear();
        //解析新场景里的所有怪物
        StartCoroutine("getMons");
	}

    //从怪物缓存列表中取出怪物
    IEnumerator getMons() {
        while (true) {
            yield return new WaitForSeconds(0.04f);
            if (LoadData.loadingMonsterList.Count > 0)
            {
                MonsterModel model = LoadData.loadingMonsterList[0];
                creatMon(model);
                LoadData.loadingMonsterList.RemoveAt(0);
            }
        }
    }
    //初始化怪物
    void creatMon(MonsterModel model)
    {
        GameInfo.tempCount++;
       // print("实例化" + GameInfo.tempCount);
        GameObject go = Instantiate(Resources.Load<GameObject>("Monsters/"+model.Look));
        go.GetComponent<MonsterBase>().initCommonProperties(model);
        go.transform.position = new Vector3((float)model.OriPoint.X, (float)model.OriPoint.Y, (float)model.OriPoint.Z);
        go.transform.parent = monsterHolder;
        go.transform.localScale = Vector3.one;
     //加入怪物列表
		string monIndex = model.FirstIndex.ToString() + "_" + model.SecondIndex.ToString();
		MonList.Add(monIndex, go);
    }


        void creatPlayer(PlayerModel model)
	{
		//实例化人物
		Assets.Model.Vector3 point = model.Point;
		Assets.Model.Vector4 rotation = model.Rotation;
		GameObject go = (GameObject)Instantiate(playerPres[model.Job] , new Vector3((float)point.X,(float)point.Y,(float)point.Z),new Quaternion((float)rotation.X,(float)rotation.Y,(float)rotation.Z,(float)rotation.W));
		go.name = "人物_"+model.Name;
//        go.layer = 8;//设置Player层，为了面部相机
        //添加到角色列表
    //    playerModelList.Add(model.Name, model);
        playerGoList.Add(model.Name, go);

        //如果是自己,初始相机位置
        print("这个人来了："+model.Name+"我是"+GameInfo.myPlayerModel.Name);
		if(model.Name == GameInfo.myPlayerModel.Name)//如果是自己 
		{
			//绑定相机
			Camera.main.GetComponent<FollowPlayer>().InitFllow(go.transform);
			go.tag=Tags.localPlayer;//设置本地玩家Tag。
		}
		go.GetComponent<PlayerProperties>().initCommonProperties(model);
	}

	void move(string message){
		//更新移动的玩家的信息
		MoveDTO dto = Coding<MoveDTO>.decode(message);
		if(dto.Name == GameInfo.myPlayerModel.Name)
		{
			return;
		}
		GameObject go = playerGoList[dto.Name];
//		print("收到广播" +go.name+"移动了");
		//设置这个移动了的人的目标点
		go.transform.rotation = new Quaternion((float)dto.Rotation.X ,(float)dto.Rotation.Y,(float)dto.Rotation.Z,(float)dto.Rotation.W);
		go.GetComponent<PlayerDir>().targetPosition =new Vector3((float)dto.Point.X,(float)dto.Point.Y,(float)dto.Point.Z);
	}
}