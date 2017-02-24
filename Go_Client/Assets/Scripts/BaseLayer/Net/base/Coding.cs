using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;//为了输出log

class Coding<T>
{
	public static string encode(T model)
	{
		return JsonMapper.ToJson (model);
	}
	public static T decode(string message)
	{
//		Debug.Log("要解包的数据："+ message);
		return JsonMapper.ToObject<T> (message);
	}
}
