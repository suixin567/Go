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
    public Text shortCutText;

    private void Awake()
    {
        InitProperty();
    }

    void InitProperty() {
		icon = transform.Find("icon").GetComponent<Image>();
		name = transform.Find("name").GetComponent<Text>();
		type = transform.Find("type").GetComponent<Text>();
		des = transform.Find("des").GetComponent<Text>();
		mp = transform.Find("mp").GetComponent<Text>();
    }


    public void Init(Skill _skill) {
        this.skill = _skill;
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
    }

    //打开设置快捷键面板
    public void onOpenShortcutPanel() {
        SkillPanel.instance.selectedItem = this;
        SkillPanel.instance.shortCutPanel.gameObject.SetActive(true);
    }

    //设置快捷键
    public void onSetShortcut(int index)
    {
        shortCutText.text = "F" + index;
        //更换快捷键
        PlayerSkill playerSkill = GameObject.FindGameObjectWithTag(Tags.localPlayer).GetComponent<PlayerSkill>();
        for (int i = 0; i < playerSkill.mySkillList.Count; i++)
        {
            if (playerSkill.mySkillList[i].Id == skill.Id)
            {
                playerSkill.mySkillList[i].Shortcut = index;
                print(playerSkill.mySkillList[i].Name + "被设置快捷键盘：" + index);
            }
        }
    }

}
