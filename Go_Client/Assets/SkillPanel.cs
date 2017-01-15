using UnityEngine;
using System.Collections;

public class SkillPanel : MonoBehaviour {
	public static SkillPanel _instance;

	private bool isShow = false;

	void Awake() {
		_instance = this;
	}
	void Start () {
		Hide();
	}
	

	void Update () {
	
	}


	public void Show()
	{
		isShow=true;
		gameObject.SetActive(true);
	}
	public void Hide()
	{
		isShow =false;
		gameObject.SetActive(false);
	}
	public void DisplaySwitch()
	{
		if (isShow == false)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}
}
