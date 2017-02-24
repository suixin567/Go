using UnityEngine;
using System.Collections;

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
			X = v.x;
			Y = v.y;
			Z = v.z;
		}
}

}