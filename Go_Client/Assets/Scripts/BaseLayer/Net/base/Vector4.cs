using UnityEngine;
using System.Collections;
using System;

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
//
//		Y = v.y;
//		Z = v.z;
//		W = v.w;
			X = Math.Round(v.x,2);
			Y = Math.Round(v.y,2);
			Z = Math.Round(v.z,2);
			W = Math.Round(v.z,2);
	}
}

}