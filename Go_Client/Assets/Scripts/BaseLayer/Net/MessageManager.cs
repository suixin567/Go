using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageManager : MonoBehaviour {

	private LoginHandler login;
	private UserHandler user;
	private MapHandler map;
	private ItemHandler item;

	void Start () {
	}

	void Update () 
	{
		List<SocketModel> list = NetWorkManager.getInstance ().getList ();
		for(int  i =0;i<8;i++)
		{
			if(list.Count>0)
			{
				SocketModel model = list[0];
				OnMessage(model);
				list.RemoveAt(0);
			}else
			{
				break;
			}
		}
	}


	public void OnMessage(SocketModel model)
	{
        //		Debug.Log ("模型信息"+model.Type); //总是0
        //		Debug.Log ("模型信息"+model.Area); //总是1
        //		Debug.Log ("模型信息"+model.Command); //总是0
        //		Debug.Log ("模型信息"+model.Message);
        if (model == null)
        {
            Debug.LogWarning("这个不该发生");
            return;
        }
		switch(model.Type)
		{
		case Protocol.LOGIN:
			if(login ==null)
			{
				login = Camera.main. GetComponent<LoginHandler> ();
			}
			login.OnMessage(model);
			break;
		case Protocol.USER:
			if(user==null){
				user = Camera.main. GetComponent<UserHandler> ();
			}
			user.OnMessage(model);
			break;
		case Protocol.MAP:
			if(map==null){
				map = Camera.main. GetComponent<MapHandler> ();
			}
			map.OnMessage(model);
			break;
		case Protocol.ITEM:
			if(item==null){
				item = Camera.main. GetComponent<ItemHandler> ();
			}
			item.OnMessage(model);
			break;

		case Protocol.OFFLINE:
			print("被踢下线");
			GameObject panelGo = GameObject.Instantiate( Resources.Load<GameObject>("UI/reLoginPanel"));
			panelGo.transform.SetParent(GameObject.Find("Canvas").transform);
			panelGo.transform.localPosition = new Vector3(0,0,-500);
			break;
		case 99:
			print("收到测试信息"+model.Message);
			break;
		default:
			WindowConstants.windowList.Add(WindowConstants.SOCKET_TYPE_FALL);
			break;
		}
	}



}
