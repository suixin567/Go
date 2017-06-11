using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonsterBase : MonoBehaviour {

    public MonsterModel monModel;

    Slider bloodSlider;
    Text bloodText;
    public string monCode = "";

    public virtual void Start () {
        bloodSlider = transform.Find("Canvas/bloodBar").GetComponent<Slider>();
        bloodText = transform.Find("Canvas/bloodBar/bloodText").GetComponent<Text>();
        monCode = monModel.FirstIndex + "_" + monModel.SecondIndex;
    }


    public virtual void initCommonProperties(MonsterModel model)
    {
       this. monModel = model;
        //名字
        transform.Find("Canvas/name").GetComponent<Text>().text = model.Name;
    }


    public virtual void Update () {
        updateBlood(0);
    }

    /// <summary>
    /// 更新怪物血量
    /// </summary>
    /// <param name="value"></param>
    public void updateBlood(int value)
    {
        monModel.Hp += value;
        if (monModel.Hp >= monModel.MaxHp)
        {
            monModel.Hp = monModel.MaxHp;
        }
        if (monModel.Hp<=0) {//死亡
            if (GameObject.Find("MainCamera").GetComponent<MapHandler>().MonList.ContainsKey(monCode)) {
                print("移除怪物列表");
                GameObject.Find("MainCamera").GetComponent<MapHandler>().MonList.Remove(monCode);
            }            
            Destroy(gameObject);
        }
        bloodSlider.value = (float)monModel.Hp / monModel.MaxHp;
        bloodText.text = monModel.Hp + "/" + monModel.MaxHp;
    }

    protected void OnMouseEnter()
    {
        CursorManager._instance.SetAttack();
    }

    protected void OnMouseExit()
    {
        CursorManager._instance.SetNormal();
    }
}
