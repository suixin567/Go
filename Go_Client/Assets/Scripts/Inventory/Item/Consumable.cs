//using UnityEngine;
//using System.Collections;
///// <summary>
///// 消耗品类
///// </summary>
//public class Consumable : Item {
//
//    public int HP { get; set; }
//    public int MP { get; set; }
//
//	public Consumable(int id, string name,int itemType,string sprite, string quality, int capacity,  int sellPrice,int buyPrice, string des ,int hp,int mp)
//		:base(id,name,itemType,sprite,quality,capacity,sellPrice,buyPrice,des)
//    {
//        this.HP = hp;
//        this.MP = mp;
//    }
//
//    public override string GetToolTipText()
//    {
//        string text = base.GetToolTipText();
//
//        string newText = string.Format("{0}\n\n<color=blue>加血：{1}\n加蓝：{2}</color>", text, HP, MP);
//
//        return newText;
//    }
//
//    public override string ToString()
//    {
//        string s = "";
//        s += Id.ToString();
//		s += ItemType;
//		s += Sprite;
//        s += Quality;
//        s += Capacity; 
//		s += SellPrice;
//        s += BuyPrice;
//		s += Description;
//        s += HP;
//        s += MP;
//        return s;
//    }
//
//}
