using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour {

    public int id;
    private SkillInfo info;

	private Sprite icon;
	private Text name;
	private Text type ;
	private Text des;
	private Text mp;

    private GameObject icon_mask;


    void InitProperty() {
		icon = transform.FindChild("icon").GetComponent<Sprite>();
		name = transform.FindChild("name").GetComponent<Text>();
		type = transform.FindChild("type").GetComponent<Text>();
		des = transform.FindChild("des").GetComponent<Text>();
		mp = transform.FindChild("mp").GetComponent<Text>();

        icon_mask = transform.Find("icon_mask").gameObject;
        icon_mask.SetActive(false);
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

    //通过调用这个方法，来更新显示
    public void SetId(int id) {
        InitProperty();
        this.id = id;
        info = SkillsInfo._instance.GetSkillInfoById(id);
       // icon = info.icon;
		name.text = info.name;
        switch (info.applyType) {
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
		des.text = info.des;
		mp.text = info.mp + "MP";
    }

}
