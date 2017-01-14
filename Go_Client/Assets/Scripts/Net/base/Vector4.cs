using UnityEngine;
using System.Collections;

namespace Assets.Model{
	[System.Serializable]
public class Vector4 {

	public double X{ get; set;}
	public double Y{ get; set;}
	public double Z { get; set;}
	public double W { get; set;}

		public Vector4()
	{
		
	}
	public Vector4(Quaternion v)
	{
		X = v.x;
		Y = v.y;
		Z = v.z;
		W = v.w;
	}
}

}