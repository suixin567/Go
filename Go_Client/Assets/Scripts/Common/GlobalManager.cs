using UnityEngine;
using System.Collections;

public class GlobalManager : MonoBehaviour {

	public static GlobalManager instance;

	void Awake()
	{
		instance=this;
	}

	void Start () {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
