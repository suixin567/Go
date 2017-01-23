using UnityEngine;


public class GameInfo {

	public static int GAME_STATE = 0;
	public static string ACC_ID = "";//保存账号
	public static float LOAD_PRORESS = 0f;//进度条的进度
	
	public static int LAST_STATE = 0;

	public static PlayerModel myPlayerModel = new PlayerModel();//进入游戏后的全局角色对象
    public static int tempCount = 0;
}
