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
	public const int TALK_CREQ = 7;
	public const int TALK_BRO = 8;
	public const int ATTACK_CREQ = 9;
	public const int ATTACK_BRO = 10;
	public const int MONSTER_INIT_SRES = 11;
	public const int BE_ATTACK_CREQ = 12;
	public const int BE_ATTACK_BRO = 13;
	public const int MONSTER_DIE_BRO = 14;
	public const int EXP_UP_SRES = 15;
	public const int LEVEL_UP_BRO = 16;
	public const int MONSTER_RELIVE_BRO = 17;



}
