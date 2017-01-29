using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonsterBase : MonoBehaviour {

    public MonsterModel monModel;

    Slider bloodSlider;
    Text bloodText;


    public virtual void Start () {
        bloodSlider = transform.FindChild("Canvas/bloodBar").GetComponent<Slider>();
        bloodText = transform.FindChild("Canvas/bloodBar/bloodText").GetComponent<Text>();
    }


    public virtual void initCommonProperties(MonsterModel model)
    {
       this. monModel = model;
        //名字
        transform.FindChild("Canvas/name").GetComponent<Text>().text = model.Name;
    }


    public virtual void Update () {
        updateBlood(0);
    }


    public void updateBlood(int value)
    {
//        model.Hp += value;
//        if (model.Hp >= model.MaxHp)
//        {
//            model.Hp = model.MaxHp;
//        }
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
