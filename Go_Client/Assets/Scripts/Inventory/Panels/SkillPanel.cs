using UnityEngine;
using System.Collections;

class SkillShortcutDTO
{
    public int SkillId { get; set; }
    public int ShortcutIndex { get; set; }
}

public class SkillPanel : MonoBehaviour {
	public static SkillPanel instance;

    CanvasGroup canvasGroup;
    private bool isShow = false;
    public Transform contentPanel;
    public Transform shortCutPanel;//快捷键面板

    [HideInInspector]
    public SkillItem selectedItem;//被选中，准备设置快捷键的item
    int selectedShortcutIndex;//被选中的快捷键 F1、F2....

    void Awake() {
		instance = this;
	}
	void Start () {
        canvasGroup = GetComponent<CanvasGroup>();
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }
		Hide();
        shortCutPanel.gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }
    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
    public void DisplaySwitch()
    {
        if (canvasGroup.alpha == 0)
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




    //设置技能的快捷键
    public void onF1Btn()
    {
        sendShortCut(1);
        selectedShortcutIndex = 1;
    }
    public void onF2Btn()
    {
        sendShortCut(2);
        selectedShortcutIndex = 2;
    }
    public void onF3Btn()
    {
        sendShortCut(3);
        selectedShortcutIndex = 3;
    }
    public void onF4Btn()
    {
        sendShortCut(4);
        selectedShortcutIndex = 4;
    }
    public void onF5Btn()
    {
        sendShortCut(5);
        selectedShortcutIndex = 5;
    }
    public void onF6Btn()
    {
        sendShortCut(6);
        selectedShortcutIndex = 6;
    }
    public void onF7Btn()
    {
        sendShortCut(7);
        selectedShortcutIndex = 7;
    }
    public void onF8Btn()
    {
        sendShortCut(8);
        selectedShortcutIndex = 8;
    }

    //设置快捷键的网络消息
    void sendShortCut(int shortCuntIndex) {
        shortCutPanel.gameObject.SetActive(false);
        //发送网路消息
        SkillShortcutDTO dto = new SkillShortcutDTO();
        dto.SkillId = selectedItem.skill.Id;
        dto.ShortcutIndex = shortCuntIndex;
        string message = LitJson.JsonMapper.ToJson(dto);
        NetWorkManager.instance.sendMessage(Protocol.ITEM, -1, ItemProtocal.SET_SKILL_SHORTCUT_CREQ, message);
    }

    //发送后的结果
    public void onsendShortCutResponsed(string message)
    {
        if (message == "true")
        {
            selectedItem.onSetShortcut(selectedShortcutIndex);
        }
        else {
            Debug.LogError("设置失败");
        }
    }

    //关闭按钮
    public void onCloseBtn()
    {
        DisplaySwitch();
    }
}



