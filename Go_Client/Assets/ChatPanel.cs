using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour {

	public InputField input;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnSendBtn()
	{
		if(input.text==null || input.text==string.Empty)
		{
			return;
		}
		StringDTO dto = new StringDTO(input.text);
		string message = Coding<StringDTO>.encode(dto);
	//	NetWorkScript.getInstance().sendMessage(Protocol.MAP,GameInfo.myPlayerModel.Map,MapProtocol.TALK_CREQ,message);
	}
}
