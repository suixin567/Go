using UnityEngine;
using System.Collections;

public class goTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void send()
	{
		//string message = "a8h";
		//NetWorkScript.getInstance().sendMessage(99, GameInfo.myPlayerModel.Map, MapProtocol.ATTACK_CREQ, message);
		for(int i=0;i<20;i++){
			string message = "abcdefghijklmnopqrstuvwxyz" + i.ToString();
			NetWorkManager.instance.sendMessage(99, GameInfo.myPlayerModel.Map, MapProtocol.ATTACK_MON_CREQ, message);
		}
	}
}
