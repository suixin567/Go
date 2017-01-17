using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShortCut : MonoBehaviour {

    public KeyCode keyCode;
    public Image icon;

	void Start () {
        icon = transform.FindChild("Image").GetComponent<Image>();
        icon.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(keyCode))
        {
            print(gameObject.name);
        }
	}

    //为快捷栏赋值
    public void setItem()
    {

    }
}
