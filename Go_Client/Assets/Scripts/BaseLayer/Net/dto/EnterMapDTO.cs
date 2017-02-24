using UnityEngine;
using System.Collections;

public class EnterMapDTO  {
	
	//账号下的哪个角色
	public string Name;
	//地图
	public int Map;
	//切换地图后在哪个地方出现
	public Assets.Model.Vector3 Point { get; set;}
	//旋转
	public Assets.Model.Vector4 Rotation { get; set;}
	
}
