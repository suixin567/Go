using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public    class PlayerDTO
{
	public int Id {get;set;}
	public string Name {get;set;}
	public int Map { get; set; }
//	public ace.Vector3 Point {get;set;}TODO
	//public string toString() {
//		return "id:  " + Id + "    name:  " + Name + "  Map:   " + Map+"  X:   " + Point.X + "  Y:   " + Point.Y + "  Z:   " + Point.Z;
	//}
}
