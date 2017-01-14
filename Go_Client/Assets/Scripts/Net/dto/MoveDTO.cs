using UnityEngine;
using System.Collections;

public class MoveDTO  {
	
	//一个账号里角色的
	public string Name{get;set;}
	public int Dir {get;set;}//A神传递的是角色的运动状态。。。。
	public Assets.Model.Vector4 Rotation{get;set;}
	public Assets.Model.Vector3 Point{get;set;}
}
