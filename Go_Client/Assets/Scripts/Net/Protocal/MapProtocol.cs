using UnityEngine;
using System.Collections;

public class MapProtocol {
	public const int ENTER_CREQ = 0;
	public const int ENTER_SRES = 1;
	//服务器群发其他人进入地图
	public const int ENTER_BRO = 2;
	public const int MOVE_CREQ = 3;
	public const int MOVE_BRO = 4;
	public const int LEAVE_CREQ = 5;
	public const int LEAVE_BRO = 6;
    //聊天
	public const int TALK_CREQ = 7;
	public const int TALK_BRO = 8;

    //初始化怪物
    public const int MONSTER_INIT_SRES = 9;
    //怪物掉血
    public const int MONSTER_BLOOD_CREQ = 10;
    //怪物死亡
    public const int MONSTER_DIE_BRO = 11;
    //怪物复活
    public const int MONSTER_RELIVE_BRO = 12;

    //攻击
    public const int ATTACK_CREQ = 12;
	public const int ATTACK_BRO = 13;
    //被攻击
	public const int BE_ATTACK_CREQ = 14;
	public const int BE_ATTACK_BRO = 15;
    //经验
	public const int EXP_UP_SRES = 16;
    //升级
	public const int LEVEL_UP_BRO = 17;
}
