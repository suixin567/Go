using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour {

	//public UISprite sprite;
	Slider loadSlider;

	void Start () {
		loadSlider= GetComponent<Slider>();
		gameObject.SetActive (false);
	//	sprite.fillAmount = 0f;//初始时进度为零
		loadSlider.value=0;
	}
	

	void Update () {
	if(GameInfo.GAME_STATE==GameState.LOADING)
		{
		//	sprite.fillAmount=GameInfo.LOAD_PRORESS;
			loadSlider.value=GameInfo.LOAD_PRORESS;
		}
	}
}
