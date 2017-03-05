using UnityEngine;
using System.Collections;

public class MoveDTO  {
	
	//一个账号里角色的
	public string Name{get;set;}
	public int Dir {get;set;}//传递的是角色的运动状态。。。。
	public Assets.Model.Vector4 Rotation = new Assets.Model.Vector4();
	public Assets.Model.Vector3 Point = new Assets.Model.Vector3();
}
