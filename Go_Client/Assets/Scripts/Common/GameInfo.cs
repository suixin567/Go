using UnityEngine;


public class GameInfo {

    public static bool IS_SETUP = false;
    public static string ACC_ID = "";//保存账号
	public static float LOAD_PRORESS = 0f;//进度条的进度
	

	public static PlayerModel myPlayerModel = new PlayerModel();//进入游戏后的全局角色对象

}
