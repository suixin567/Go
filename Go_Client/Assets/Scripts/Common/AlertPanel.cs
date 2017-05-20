using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlertPanel : MonoBehaviour {
//	public UILabel label;

	public Text content;
	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}
	public void setMessage (string value)
	{
		content.text = value;
	}
	public void OnClick()
	{
		gameObject.SetActive (false);
    //    NetWorkManager.NET_STATE = GameInfo.LAST_STATE;
	}

}
