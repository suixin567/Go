using UnityEngine;
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
    //Thread moveThread;


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
			//StartCoroutine(move(model.Message));
			break;
		case MapProtocol.LEAVE_BRO:
			leave(model.Message);
			break;
            //初始化怪物
            case MapProtocol.MONSTER_INIT_SRES:
            initMons(model.Message);
            break;
		case MapProtocol.ATTACK_MON_BRO://攻击怪物广播
			attackMon(model.Message);
			break;
            //case MapProtocol.ATTACK_PLAYER_BRO://攻击人物广播
            //attackPlayer(model.Message);
            //break;
            case MapProtocol.TALK_BRO:
			chat(model.Message);
			break;
		case MapProtocol.MONSTER_RELIVE_BRO://怪物复活
			monRelive(model.Message);
			break;
			default :
			Debug.LogError("un known"+model.Command);
			break;
		}
	}

    ////攻击人物
    //void attackPlayer(string message) {

    //}

    //怪物复活
    void monRelive(string message)
	{
		MonsterModel mon = Coding<MonsterModel>.decode(message);
		LoadData.loadingMonsterList.Add(mon);
	}

    //攻击怪物
	void attackMon(string message)
	{
        AttackMonDTO dto = Coding<AttackMonDTO>.decode(message);

        GameObject playerGo = playerGoList[dto.Player];

        if (dto.TarPlayer == "" && dto.FirstIndex == -1 && dto.SecondIndex == -1)
        {//释放空技能
            playerGo.GetComponent<PlayerController>().Attack(dto.Skill,null,(float) dto.TarPos.X, (float)dto.TarPos.Y, (float)dto.TarPos.Z);
        }
        else {
            if (dto.TarPlayer == "")
            {//攻击怪物
                string monIndex = dto.FirstIndex.ToString() + "_" + dto.SecondIndex.ToString();
                GameObject monGo = MonList[monIndex];
                playerGo.GetComponent<PlayerController>().Attack(dto.Skill, monGo.transform);
            }
            else
            {//攻击人物
                GameObject tarPlayerGo = playerGoList[dto.TarPlayer];
                playerGo.GetComponent<PlayerController>().Attack(dto.Skill, tarPlayerGo.transform);
            }
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
//        GameInfo.tempCount++;
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

	void accessData(JSONObject obj){
		switch(obj.type){
		case JSONObject.Type.OBJECT:
			for(int i = 0; i < obj.list.Count; i++){
				string key = (string)obj.keys[i];
				JSONObject j = (JSONObject)obj.list[i];
				Debug.Log(key);
				accessData(j);
			}
			break;
		case JSONObject.Type.ARRAY:
			foreach(JSONObject j in obj.list){
				accessData(j);
			}
			break;
		case JSONObject.Type.STRING:
			Debug.Log(obj.str);
			break;
		case JSONObject.Type.NUMBER:
			Debug.Log(obj.n);
			break;

		}
	}

	void move(string message){
		//更新移动的玩家的信息
		MoveDTO dto=new MoveDTO();
		JSONObject j = new JSONObject(message);
		dto.Name = j["Name"].str;
		dto.Dir =(int)(j["Dir"].n);

		dto.Point.X = j["Point"].list[0].n;
		dto.Point.Y = j["Point"].list[1].n;
		dto.Point.Z = j["Point"].list[2].n;

		//dto.Rotation.X =((JSONObject)j["Rotation"].list[0]).n;
		//dto.Rotation.Y =((JSONObject)j["Rotation"].list[1]).n;
		//dto.Rotation.Z =((JSONObject)j["Rotation"].list[2]).n;
		//dto.Rotation.W =((JSONObject)j["Rotation"].list[3]).n;

		GameObject go = playerGoList[dto.Name];
		//设置这个移动了的人的目标点
	//	go.transform.rotation = new Quaternion((float)dto.Rotation.X ,(float)dto.Rotation.Y,(float)dto.Rotation.Z,(float)dto.Rotation.W);
		go.GetComponent<PlayerController>().Move(new Vector3((float)dto.Point.X,(float)dto.Point.Y,(float)dto.Point.Z));
	}
}
