using UnityEngine;
using System.Collections;

public class PlayerModel {

//角色id
	public int Id { get; set;}
//角色名
	public string Name{ get; set;}
	//职业
	public int Job { get; set;}
	//等级
	public int Level{ get; set;}

	public int Gold{ get; set;}
	//当前经验
	public int Exp{ get; set;}
	//攻击力
	public int Atk{ get; set;}
	//防御力
	public int Def{ get; set;}
	//hp
	public int Hp{ get; set;}
	//maxhp
	public int MaxHP{ get; set;}
	//坐标
	public Assets.Model.Vector3 Point {get;set;}
	//旋转
	public Assets.Model.Vector4 Rotation {get;set;}
	//所在地图
	public int Map{ get; set;}
	//此人是否激活
	public int Active{get;set;}
	//此人所拥有的物品
	//public string Items{get;set;}

	public PlayerModel()
	{

	}
	public PlayerModel(PlayerModel model)
	{
		this.Job = model.Job;
		this.Level = model.Level;
		this.Exp = model.Exp;
		this.Atk = model.Atk;
		this.Def = model.Def;
		this.Map = model.Map;
		this.Hp = model.Hp;
		this.MaxHP = model.MaxHP;
	}
	public PlayerModel(int job,int level,int exp,int atk,int def,int map,int hp,int maxHp)
	{
		this.Job = job;
		this.Level =level;
		this.Exp = exp;
		this.Atk = atk;
		this.Def = def;
		this.Map = Map;
		this.Hp = hp;
		this.MaxHP = maxHp;
	}
}
