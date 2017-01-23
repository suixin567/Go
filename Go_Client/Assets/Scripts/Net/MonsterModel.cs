using UnityEngine;
using System.Collections;

public class MonsterModel {


    //名
    public string Name { get; set; }
    //类型
    public int MonsterType { get; set; }
    //外观
    public string Look { get; set; }
    //等级
    public int Level { get; set; }
    //经验
    public int Exp { get; set; }
    //Hp
    public int Hp { get; set; }
    //MaxHp
    public int MaxHp { get; set; }
    //Def
    public int Def { get; set; }
    //Atk
    public int Atk { get; set; }
    //行走速度
    public int WalkSpeed { get; set; }
    //攻击速度
    public int AtkSpeed { get; set; }
    //怪物标号
    public int FirstIndex { get; set; }
    public int SecondIndex { get; set; }
    public Assets.Model.Vector3 OriPoint { get; set; }
    //public float X { get; set; }
    //public float Y { get; set; }
    //public float Z { get; set; }
}
