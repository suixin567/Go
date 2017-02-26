using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour {

	Slider loadSlider;

	void Start () {
		loadSlider= GetComponent<Slider>();
		gameObject.SetActive (false);
		loadSlider.value=0;
	}
	

	void Update () {
	if(GameInfo.GAME_STATE==GameState.LOADING)
		{
			loadSlider.value=GameInfo.LOAD_PRORESS;
		//	print("进度:"+GameInfo.LOAD_PRORESS);
		}
	}
}
