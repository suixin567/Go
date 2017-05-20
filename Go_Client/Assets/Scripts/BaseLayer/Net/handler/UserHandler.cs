using UnityEngine;
using System.Collections;


public class UserHandler:MonoBehaviour
	{
	public void OnMessage (SocketModel model)
	{
//		Debug.Log (model.Message);
		//解析角色列表信息
		switch(model.Command)
		{
		case UserProtocol.LIST_SREQ:
			list(model.Message);
			break;
		case UserProtocol.CREATE_SREQ:
			create(model.Message);
			break;
		case UserProtocol.SELECT_SREQ:
			selectPlayer(model.Message);
			break;
			default:
			Debug.LogError("错误协议");
			break;
		}
}


	private void create (string message)
	{
		if (message == "true") {
			//创建成功后重新获取列表
			string m = Coding<StringDTO>.encode (new StringDTO(GameInfo.ACC_ID));
			NetWorkManager.instance.sendMessage (Protocol.USER,0,UserProtocol.LIST_CREQ,m);
		} else {
			WindowConstants.windowList.Add (WindowConstants.JOB_CREATE_ERR);
            NetWorkManager.NET_STATE = NetState.RUN;
		}
	}


	private void list (string message)
	{
//		if(message==string.Empty)
//		{
//			//print("没有角色");
//			return;
//		}
		PlayerModel[] dtos = Coding<PlayerModel[]>.decode (message);
		JobPanel.instance.InitJobButton(dtos);
        print("获得角色列表");
	}


	//点击开始按钮后服务器返回消息的解析函数
	private void selectPlayer(string message)
	{
		PlayerModel dto = Coding<PlayerModel>.decode (message);
		GameInfo.myPlayerModel = dto;
		EnterMapDTO edto = new EnterMapDTO ();
		edto.Name =dto.Name;
		edto.Map = dto.Map;
		edto.Point = dto.Point;
		edto.Rotation = dto.Rotation;
		string m = Coding<EnterMapDTO>.encode (edto);
		NetWorkManager.instance.sendMessage (Protocol.MAP,dto.Map,MapProtocol.ENTER_CREQ,m);
	}

}