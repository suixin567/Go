using UnityEngine;
using System.Collections;
using System;
//using UnityEngine;

namespace Assets.Model{
	[System.Serializable]
public class Vector3 {
		public double X{ get; set;}
		public double Y{ get; set;}
		public double Z { get; set;}

		public Vector3()
		{

		}
		public Vector3(UnityEngine.Vector3 v)
		{
			X = Math.Round(v.x,2);
			Y = Math.Round(v.y,2);
			Z = Math.Round(v.z,2);
		}
}

}