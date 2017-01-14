using UnityEngine;
using System.Collections;

public class ReLoginPanel : MonoBehaviour {

	void Start () {

	}


	public void onReLosginBtnClic()
	{
		//切换场景
		Destroy(gameObject);
		ResceneScript.instance.Loading(0);
		print("返回登录场景");
	}
	public void onQuitBtnClic()
	{
		print("退出");
		Application.Quit();
	}
}
