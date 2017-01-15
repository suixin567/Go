using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillsInfo : MonoBehaviour {

    public static SkillsInfo _instance;
    public TextAsset skillsInfoText;

    private Dictionary<int, SkillInfo> skillInfoDict = new Dictionary<int, SkillInfo>();
    
    void Awake() {
        _instance = this;
        InitSkillInfoDict();//初始化技能信息字典
    }

    //我们可以通过在这个方法，根据id查找到一个技能信息
    public SkillInfo GetSkillInfoById(int id) {
        SkillInfo info = null;
        skillInfoDict.TryGetValue(id, out info);
        return info;
    }

    //初始化技能信息字典
    void InitSkillInfoDict() {
        string text = skillsInfoText.text;
        string[] skillinfoArray = text.Split('\n');
        foreach (string skillinfoStr in skillinfoArray) {
            string[] pa = skillinfoStr.Split(',');
            SkillInfo info = new SkillInfo();
            info.id = int.Parse(pa[0]);
            info.name = pa[1];
            info.icon = pa[2];
            info.des = pa[3];
            string str_applytype = pa[4];
            switch (str_applytype) {
                case "Passive":
                    info.applyType = ApplyType.Passive;
                    break;
                case "Buff":
                    info.applyType = ApplyType.Buff;
                    break;
                case "SingleTarget":
                    info.applyType = ApplyType.SingleTarget;
                    break;
                case "MultiTarget":
                    info.applyType = ApplyType.MultiTarget;
                    break;
            }
            string str_applypro = pa[5];
            switch (str_applypro) {
                case "Attack":
                    info.applyProperty = ApplyProperty.Attack;
                    break;
                case "Def":
                    info.applyProperty = ApplyProperty.Def;
                    break;
                case "Speed":
                    info.applyProperty = ApplyProperty.Speed;
                    break;
                case "AttackSpeed":
                    info.applyProperty = ApplyProperty.AttackSpeed;
                    break;
                case "HP":
                    info.applyProperty = ApplyProperty.HP;
                    break;
                case "MP":
                    info.applyProperty = ApplyProperty.MP;
                    break;
            }
            info.applyValue = int.Parse(pa[6]);
            info.applyTime = int.Parse(pa[7]);
            info.mp = int.Parse(pa[8]);
            info.coldTime = int.Parse(pa[9]);
            switch (pa[10]) {
                case "Swordman":
                    info.applicableRole = ApplicableRole.Swordman;
                    break;
                case "Magician":
                    info.applicableRole = ApplicableRole.Magician;
                    break;
            }
            info.level = int.Parse(pa[11]);
            switch (pa[12]) {
                case "Self":
                    info.releaseType = ReleaseType.Self;
                    break;
                case "Enemy":
                    info.releaseType = ReleaseType.Enemy;
                    break;
                case "Position":
                    info.releaseType = ReleaseType.Position;
                    break;
            }
            info.distance = float.Parse(pa[13]);
            info.efx_name = pa[14];
            info.aniname = pa[15];
            info.anitime = float.Parse(pa[16]);
            skillInfoDict.Add(info.id, info);
        }
    }
}


//适用角色
public enum ApplicableRole {
    Swordman,
    Magician
}
//作用类型
public enum ApplyType {
    Passive,
    Buff,
    SingleTarget,
    MultiTarget
}
//作用属性
public enum ApplyProperty {
    Attack,
    Def,
    Speed,
    AttackSpeed,
    HP,
    MP
}
//释放类型
public enum ReleaseType {
    Self,
    Enemy,
    Position
}
//技能信息
public class SkillInfo {
    public int id;
    public string name;
    public string icon;
    public string des;
    public ApplyType applyType;
    public ApplyProperty applyProperty;
    public int applyValue;
    public int applyTime;
    public int mp;
    public int coldTime;
    public ApplicableRole applicableRole;
    public int level;
    public ReleaseType releaseType;
    public float distance;
    public string efx_name;
    public string aniname;
    public float anitime = 0;
}