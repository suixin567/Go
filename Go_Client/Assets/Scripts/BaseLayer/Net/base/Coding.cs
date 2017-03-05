using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;//为了输出log
//using System.Threading;

class Coding<T>
{
	public static string encode(T model)
	{
		return JsonMapper.ToJson (model);
	}
	public static T decode(string message)
	{
		
		try{
			return JsonMapper.ToObject<T> (message);
		}
		catch{
			Debug.LogError("要解包的数据 格式错误："+ message);
		}
		finally{
			
		}
		return default (T);
	}
}
