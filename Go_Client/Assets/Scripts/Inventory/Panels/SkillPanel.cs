using UnityEngine;
using System.Collections;

public class SkillPanel : MonoBehaviour {
	public static SkillPanel instance;

	private bool isShow = false;
    public Transform contentPanel;
    public Transform shortCutPanel;//快捷键面板
    bool isSetShortCutState = false;//是否在设置快捷键
    public SkillItem selectedItem;//被选中，准备设置快捷键的item

	void Awake() {
		instance = this;
	}
	void Start () {
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }
		Hide();
        shortCutPanel.gameObject.SetActive(false);
    }
	
	void Update () {
	}

	public void Show()
	{
		isShow=true;
		gameObject.SetActive(true);
	}
	public void Hide()
	{
		isShow =false;
		gameObject.SetActive(false);
	}
	public void DisplaySwitch()
	{
		if (isShow == false)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}
    
    public void creatSkillItem(Skill skill)
    {
        GameObject skillItem = Instantiate(Resources.Load<GameObject>("Inventory/skillItem"));
        skillItem.transform.SetParent(contentPanel);
        skillItem.transform.localPosition = Vector3.zero;
        skillItem.transform.localScale = Vector3.one;
        skillItem.GetComponent<SkillItem>().Init(skill);
    }

    //关闭按钮
    public void onCloseBtn() {
        DisplaySwitch();
    }

    //设置技能的快捷键
    public void onF1Btn()
    {
        selectedItem.onSetShortcut(1);
        shortCutPanel.gameObject.SetActive(false);
    }
    public void onF2Btn()
    {
        selectedItem.onSetShortcut(2);
        shortCutPanel.gameObject.SetActive(false);
    }
    public void onF3Btn()
    {
        selectedItem.onSetShortcut(3);
        shortCutPanel.gameObject.SetActive(false);
    }
    public void onF4Btn()
    {
        selectedItem.onSetShortcut(4);
        shortCutPanel.gameObject.SetActive(false);
    }
    public void onF5Btn()
    {
        selectedItem.onSetShortcut(5);
        shortCutPanel.gameObject.SetActive(false);
    }
    public void onF6Btn()
    {
        selectedItem.onSetShortcut(6);
        shortCutPanel.gameObject.SetActive(false);
    }
    public void onF7Btn()
    {
        selectedItem.onSetShortcut(7);
        shortCutPanel.gameObject.SetActive(false);
    }
    public void onF8Btn()
    {
        selectedItem.onSetShortcut(8);
        shortCutPanel.gameObject.SetActive(false);
    }
}
