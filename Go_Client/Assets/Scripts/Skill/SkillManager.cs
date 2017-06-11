using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//适用职业
public enum ApplicableRole
{
    None,
    Swordman,
    Magician
}
//作用类型
public enum ApplyType
{
    Passive,
    Buff,
    SingleTarget,
    MultiTarget
}
//作用属性
public enum ApplyProperty
{
    Attack,
    Def,
    Speed,
    AttackSpeed,
    HP,
    MP
}
//释放类型
public enum ReleaseType
{
    Self,
    Enemy,
    Position
}


//技能信息
public class Skill
{
    public int Id;
    public string Name;
    public string Icon;
    public string Des;
    public ApplyType ApplyType;
    public ApplyProperty ApplyProperty;
    public int ApplyValue;
    public float ApplyTime;
    public int Mp;
    public float ColdTime;
    public ApplicableRole Job;
    public int level;
    public ReleaseType ReleaseType;
    public float Distance;
    public int Shortcut;
}




public class SkillManager : MonoBehaviour
{
//    string abc =@"[{""Id"":0, ""Name"":""治愈术""}]";
    public static SkillManager _instance;

  //  private Dictionary<int, Skill> skillInfoDict = new Dictionary<int, Skill>();//技能字典

    void Awake()
    {
        _instance = this;
    }

    ////根据id查找到一个技能信息
    //public Skill GetSkillInfoById(int id)
    //{
    //    Skill info = null;
    //    skillInfoDict.TryGetValue(id, out info);
    //    return info;
    //}


    //json数组转换为技能数组
    public List<Skill> jsons2Skills(string skills)
    {
        JSONObject j = new JSONObject(skills);

        List<Skill> skillList = new List<Skill>();

        foreach (JSONObject temp in j.list)
        {
            Skill skill = new Skill();

            //下面的事解析这个对象里面的公有属性
            skill.Id = (int)(temp["Id"].n);
            skill.Name = temp["Name"].str;
            skill.Icon = temp["Icon"].str;
            skill.Des = temp["Des"].str;
            string applyType = temp["ApplyType"].str;
            switch (applyType)
            {
                case "Passive":
                    skill.ApplyType = ApplyType.Passive;
                    break;
                case "Buff":
                    skill.ApplyType = ApplyType.Buff;
                    break;
                case "SingleTarget":
                    skill.ApplyType = ApplyType.SingleTarget;
                    break;
                case "MultiTarget":
                    skill.ApplyType = ApplyType.MultiTarget;
                    break;
                default:
                    Debug.LogWarning("解析错误" + applyType);
                    break;
            }
            string applyProperty = temp["ApplyProperty"].str;
            switch (applyProperty)
            {
                case "Attack":
                    skill.ApplyProperty = ApplyProperty.Attack;
                    break;
                case "Def":
                    skill.ApplyProperty = ApplyProperty.Def;
                    break;
                case "Speed":
                    skill.ApplyProperty = ApplyProperty.Speed;
                    break;
                case "AttackSpeed":
                    skill.ApplyProperty = ApplyProperty.AttackSpeed;
                    break;
                case "Hp":
                    skill.ApplyProperty = ApplyProperty.HP;
                    break;
                case "Mp":
                    skill.ApplyProperty = ApplyProperty.MP;
                    break;
                default:
                    Debug.LogWarning("解析错误" + applyProperty);
                    break;
            }
            skill.ApplyValue = (int)temp["ApplyValue"].n;
            skill.ApplyTime = temp["ApplyTime"].f;
            skill.Mp = (int)temp["Mp"].n;
            skill.ColdTime = temp["ColdTime"].f;
            string job = temp["Job"].str;
            switch (job)
            {
                case "Swordman":
                    skill.Job = ApplicableRole.Swordman;
                    break;
                case "Magician":
                    skill.Job = ApplicableRole.Magician;
                    break;
                default:
                    skill.Job = ApplicableRole.None;
                    break;
            }
            skill.level = (int)temp["Level"].n;
            string releaseType = temp["ReleaseType"].str;
            switch (releaseType)
            {
                case "self":
                    skill.ReleaseType = ReleaseType.Self;
                    break;
                case "enemy":
                    skill.ReleaseType = ReleaseType.Enemy;
                    break;
                case "position":
                    skill.ReleaseType = ReleaseType.Position;
                    break;
                default:
                    Debug.LogWarning("解析错误"+ releaseType);
                    break;
            }
            skill.Distance = temp["Distance"].f;
            skill.Shortcut = (int)temp["Shortcut"].n;
            //  skillInfoDict.Add(skill.Id, skill);
            skillList.Add(skill);
        }
        return skillList;
    }



    public  Skill json2Skill(string value)
    {
        Skill skill = new Skill();
        JSONObject temp = new JSONObject(value);

        skill.Id = (int)(temp["Id"].n);
        skill.Name = temp["Name"].str;
        skill.Icon = temp["Icon"].str;
        skill.Des = temp["Des"].str;
        string applyType = temp["ApplyType"].str;
        switch (applyType)
        {
            case "Passive":
                skill.ApplyType = ApplyType.Passive;
                break;
            case "Buff":
                skill.ApplyType = ApplyType.Buff;
                break;
            case "SingleTarget":
                skill.ApplyType = ApplyType.SingleTarget;
                break;
            case "MultiTarget":
                skill.ApplyType = ApplyType.MultiTarget;
                break;
            default:
                Debug.LogWarning("解析错误");
                break;
        }
        string applyProperty = temp["ApplyProperty"].str;
        switch (applyProperty)
        {
            case "Attack":
                skill.ApplyProperty = ApplyProperty.Attack;
                break;
            case "Def":
                skill.ApplyProperty = ApplyProperty.Def;
                break;
            case "Speed":
                skill.ApplyProperty = ApplyProperty.Speed;
                break;
            case "AttackSpeed":
                skill.ApplyProperty = ApplyProperty.AttackSpeed;
                break;
            case "HP":
                skill.ApplyProperty = ApplyProperty.HP;
                break;
            case "MP":
                skill.ApplyProperty = ApplyProperty.MP;
                break;
            default:
                Debug.LogWarning("解析错误" + applyProperty);
                break;
        }
        skill.ApplyValue = (int)temp["ApplyValue"].n;
        skill.ApplyTime = temp["ApplyTime"].f;
        skill.Mp = (int)temp["Mp"].n;
        skill.ColdTime = temp["ColdTime"].f;
        string job = temp["Job"].str;
        switch (job)
        {
            case "Swordman":
                skill.Job = ApplicableRole.Swordman;
                break;
            case "Magician":
                skill.Job = ApplicableRole.Magician;
                break;
            default:
                skill.Job = ApplicableRole.None;
                break;
        }
        skill.level = (int)temp["Level"].n;
        string releaseType = temp["ReleaseType"].str;
        switch (releaseType)
        {
            case "Self":
                skill.ReleaseType = ReleaseType.Self;
                break;
            case "Enemy":
                skill.ReleaseType = ReleaseType.Enemy;
                break;
            case "Position":
                skill.ReleaseType = ReleaseType.Position;
                break;
            default:
                Debug.LogWarning("解析错误"+ releaseType);
                break;
        }
        skill.Distance = temp["Distance"].f;
        skill.Shortcut = (int)temp["Shortcut"].n;
        return skill;
    }
}



//            string sprite = temp["Sprite"].str;
//            string quality = temp["Quality"].str;
//            int capacity = (int)(temp["Capacity"].n);
//            int sellPrice = (int)(temp["SellPrice"].n);
//            int buyPrice = (int)(temp["BuyPrice"].n);
//            string description = temp["Description"].str;
//            //			//判断类别
//            //			switch (itemType)
//            //            {
//            //			case 0:
//            //				item = new MaterialItem(id, name, itemType,sprite, quality, capacity,sellPrice,buyPrice,description);
//            //				break;
//            //			case 1:        
//            int hp = (int)(temp["Hp"].n);
//            int mp = (int)(temp["Mp"].n);
//            //				item = new Consumable(id, name, itemType,sprite, quality, capacity,sellPrice,buyPrice,description, hp, mp);
//            //				break;
//            //			default:
//            int attact = (int)temp["Attack"].n;
//            int def = (int)temp["Def"].n;
//            int speed = (int)temp["Speed"].n;
//            //				item = new Equipment(id, name, itemType,sprite, quality, capacity,sellPrice,buyPrice,description,  attact, def, speed);
//            //				break;
//            //            }
//            skill = new Item(id, name, itemType, sprite, quality, capacity, sellPrice, buyPrice, description, hp, mp, attact, def, speed);
//            skillInfoDict.Add(0,skill);
//        }

//        ///旧的解析
//     //   string text = skillsInfoText.text;
//   //     string[] skillinfoArray = text.Split('\n');
//    //    foreach (string skillinfoStr in skillinfoArray) {
//  //          string[] pa = skillinfoStr.Split(',');
//            Skill info = new Skill();
//            info.Id = int.Parse(pa[0]);
//            info.Name = pa[1];
//            info.Icon = pa[2];
//            info.Des = pa[3];
//            string str_applytype = pa[4];
//            switch (str_applytype) {
//                case "Passive":
//                    info.ApplyType = ApplyType.Passive;
//                    break;
//                case "Buff":
//                    info.ApplyType = ApplyType.Buff;
//                    break;
//                case "SingleTarget":
//                    info.ApplyType = ApplyType.SingleTarget;
//                    break;
//                case "MultiTarget":
//                    info.ApplyType = ApplyType.MultiTarget;
//                    break;
//            }
//            string str_applypro = pa[5];
//            switch (str_applypro) {
//                case "Attack":
//                    info.ApplyProperty = ApplyProperty.Attack;
//                    break;
//                case "Def":
//                    info.ApplyProperty = ApplyProperty.Def;
//                    break;
//                case "Speed":
//                    info.ApplyProperty = ApplyProperty.Speed;
//                    break;
//                case "AttackSpeed":
//                    info.ApplyProperty = ApplyProperty.AttackSpeed;
//                    break;
//                case "HP":
//                    info.ApplyProperty = ApplyProperty.HP;
//                    break;
//                case "MP":
//                    info.ApplyProperty = ApplyProperty.MP;
//                    break;
//            }
//            info.ApplyValue = int.Parse(pa[6]);
//            info.ApplyTime = int.Parse(pa[7]);
//            info.Mp = int.Parse(pa[8]);
//            info.ColdTime = int.Parse(pa[9]);
//            switch (pa[10]) {
//                case "Swordman":
//                    info.ApplicableRole = ApplicableRole.Swordman;
//                    break;
//                case "Magician":
//                    info.ApplicableRole = ApplicableRole.Magician;
//                    break;
//            }
//            info.level = int.Parse(pa[11]);
//            switch (pa[12]) {
//                case "Self":
//                    info.ReleaseType = ReleaseType.Self;
//                    break;
//                case "Enemy":
//                    info.ReleaseType = ReleaseType.Enemy;
//                    break;
//                case "Position":
//                    info.ReleaseType = ReleaseType.Position;
//                    break;
//            }
//            info.Distance = float.Parse(pa[13]);
//            skillInfoDict.Add(info.Id, info);
//        }
//    }
//}



