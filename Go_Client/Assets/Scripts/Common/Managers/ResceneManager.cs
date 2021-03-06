﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
/// <summary>
/// 加载进度条、切换场景
/// </summary>
public class ResceneManager : MonoBehaviour {
	public static ResceneManager instance;

	public GameObject loadingPanel;
	private AsyncOperation async;//异步加载
	
	void Awake()
	{
		instance=this;
	}

	void Update () {
		if(NetWorkManager.NET_STATE == NetState.LOADING)
		{
			GameInfo.LOAD_PRORESS=async.progress;
		}
	}
	public void Loading(int level)
	{
		loadingPanel.SetActive (true);//显示进度条
		GameInfo.LOAD_PRORESS = 0f;//进度条的进度为0

        NetWorkManager.NET_STATE = NetState.LOADING;//切换游戏状态
		StartCoroutine ("load",level);
	}

	IEnumerator load(int level)
	{
		async = SceneManager.LoadSceneAsync (level);
		yield return async;
	}


	void OnLevelWasLoaded(int level)
	{
        NetWorkManager.NET_STATE = NetState.RUN;
		loadingPanel.SetActive (false);//关闭进度条
	}
}
