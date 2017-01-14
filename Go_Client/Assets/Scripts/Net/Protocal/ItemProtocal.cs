using UnityEngine;
using System.Collections;

public class ItemProtocal {

	public const int INIT_CREQ=0;//初始化游戏物品
	public const int INIT_SRES=1;

	public const int PLAYER_ITEM_CREQ=2;//初始化一个角色的物品数据
	public const int PLAYER_ITEM_SRES=3;

	public const int PLAYER_EQUIPMENT_CREQ=4;//初始化一个角色的装备数据
	public const int PLAYER_EQUIPMENT_SRES=5;

	public const int BUY_CREQ=6;//申请购买物品
	public const int BUY_SRES=7;

	public const int USE_CREQ=8;//申请使用物品
	public const int USE_SRES=9;

	public const int PUTON_CREQ=10;//穿戴装备
	public const int PUTON_SRES=11;

	public const int PUTOFF_CRES=12;//脱下装备
	public const int PUTOFF_SRES=13;
}
