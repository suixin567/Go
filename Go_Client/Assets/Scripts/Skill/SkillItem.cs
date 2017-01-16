using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour {

    private Skill skill;

    private new Text name;
    private Image icon;
    private Text type ;
	private Text des;
	private Text mp;
    //private GameObject icon_mask;

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
        //     icon_mask = transform.Find("icon_mask").gameObject;
        //     icon_mask.SetActive(false);
    }


    public void SetId(Skill _skill) {
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
}
