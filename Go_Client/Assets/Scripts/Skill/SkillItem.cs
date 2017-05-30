using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour {

    public Skill skill;

    private new Text name;
    private Image icon;
    private Text type ;
	private Text des;
	private Text mp;
    //private GameObject icon_mask;
    public Text shortCutText;
    public int shortCutIndex = 0;//哪个快捷键会释放此技能

    private void Awake()
    {
        InitProperty();
    }

    void InitProperty() {
		icon = transform.FindChild("icon").GetComponent<Image>();
		name = transform.FindChild("name").GetComponent<Text>();
		type = transform.FindChild("type").GetComponent<Text>();
		des = transform.FindChild("des").GetComponent<Text>();
		mp = transform.FindChild("mp").GetComponent<Text>();
    }


    public void Init(Skill _skill) {
        this.skill = _skill;
        //    info = SkillsInfo._instance.GetSkillInfoById(id);
        icon.sprite = Resources.Load<Sprite>("Inventory/Item/"+ skill.Icon); 
		name.text = _skill.Name;
        switch (_skill.ApplyType) {
            case ApplyType.Passive:
			type.text = "增益";
                break;
            case ApplyType.Buff:
			type.text = "增强";
                break;
            case ApplyType.SingleTarget:
			type.text = "单个目标";
                break;
            case ApplyType.MultiTarget:
			type.text = "群体技能";
                break;
        }
		des.text = _skill.Des;
		mp.text = _skill.Mp + "MP";
        if (skill.Shortcut == 0)
        {
            shortCutText.text = "";
        }
        else {
            shortCutText.text = "F" + skill.Shortcut;
        }
        shortCutIndex = skill.Shortcut;
    }


    //    public void UpdateShow(int level) {
    //        if (info.level <= level) {//技能可用
    //            icon_mask.SetActive(false);
    //			icon.GetComponent<SkillItemIcon>().enabled = true;
    //        } else {
    //            icon_mask.SetActive(true);
    //			icon.GetComponent<SkillItemIcon>().enabled = false;
    //        }
    //    }


    //打开设置快捷键面板
    public void onOpenShortcutPanel() {
        SkillPanel.instance.selectedItem = this;
        SkillPanel.instance.shortCutPanel.gameObject.SetActive(true);
    }

    //设置快捷键
    public void onSetShortcut(int index)
    {
        shortCutIndex = index;
        shortCutText.text = "F" + index;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F1) && shortCutIndex==1) {
            releaseSkill();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F2) && shortCutIndex == 2)
        {
            releaseSkill();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F3) && shortCutIndex == 3)
        {
            releaseSkill();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F4) && shortCutIndex == 4)
        {
            releaseSkill();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F5) && shortCutIndex == 5)
        {
            releaseSkill();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F6) && shortCutIndex == 6)
        {
            releaseSkill();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F7) && shortCutIndex == 7)
        {
            releaseSkill();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F8) && shortCutIndex == 8)
        {
            releaseSkill();
            return;
        }
    }
    /// <summary>
    /// 释放技能
    /// </summary>
    void releaseSkill() {
        print("释放技能"+ shortCutIndex + " "+ skill.Id);
        GameObject.FindGameObjectWithTag(Tags.localPlayer).GetComponent<PlayerController>().Attack(skill.Id);
    }
}
