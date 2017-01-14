using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerProperties : MonoBehaviour {

    private PlayerModel m_playerModel;
    public PlayerModel M_playerModel
    {
        get {
            return m_playerModel;
        }
        set {
            m_playerModel = value;
            if (gameObject.tag == Tags.localPlayer)
            {
                GameInfo.myPlayerModel = m_playerModel;
            }
        }
    }

    Slider bloodSlider;
    Text bloodText;


    private void Awake()
    {
        bloodSlider = transform.FindChild("Canvas/bloodBar").GetComponent<Slider>();
        bloodText = transform.FindChild("Canvas/bloodText").GetComponent<Text>();
    }
    public void initCommonProperties(PlayerModel model)
	{  

        m_playerModel = model;
        if (gameObject.tag == Tags.localPlayer)
        {
            GameInfo.myPlayerModel = m_playerModel;
        }
        //人物名字
		transform.FindChild("Canvas/name").GetComponent<Text>().text = model.Name;
		updateBlood(0);
	}


    private void Update()
    {
        updateBlood(0);
    }


    //更新人物血条
    public void updateBlood(int value)
    {
        m_playerModel.Hp += value;
        if (m_playerModel.Hp >= m_playerModel.MaxHP)
        {
            m_playerModel.Hp = m_playerModel.MaxHP;
        }
        bloodSlider.value = (float)m_playerModel.Hp / m_playerModel.MaxHP;
        bloodText.text = m_playerModel.Hp + "/" + m_playerModel.MaxHP;
    }
}



//
//public void GetDrug(int hp,int mp) {//获得治疗
//	hp_remain += hp;
//	mp_remain += mp;
//	if (hp_remain > this.hp) {
//		hp_remain = this.hp;
//	}
//	if (mp_remain > this.mp) {
//		mp_remain = this.mp;
//	}
//	HeadStatusUI._instance.UpdateShow();
//}
//
//public bool GetPoint(int point=1) {//获得点数
//	if (point_remain >= point) {
//		point_remain -= point;
//		return true;
//	}
//	return false;
//}
//
//public void GetExp(int exp) {
//	this.exp += exp;
//	int total_exp = 100 + level * 30;
//	while (this.exp >= total_exp) {
//		//TODO 升级
//		this.level++;
//		this.exp -= total_exp;
//		total_exp = 100 + level * 30;
//	}
//	ExpBar._instance.SetValue(this.exp/total_exp );
//}
//
//public bool TakeMP(int count) {
//	if (mp_remain >= count) {
//		mp_remain -= count;
//		HeadStatusUI._instance.UpdateShow();
//		return true;
//	} else {
//		return false;
//	}
//}


