//using UnityEngine;
//using System.Collections;
//
//public class Equipment : Item {
//
//    public int Attack { get; set; }
//    public int Def { get; set; }
//	public int Speed{get;set;}
//
//
//	public Equipment(int id, string name, int itemType,string sprite, string quality,  int capacity, int sellPrice,int buyPrice, string des,
//		int attack,int def,int speed)
//		:base(id,name,itemType,sprite,quality,capacity,sellPrice,buyPrice,des)
//    {
//        this.Attack = attack;
//		this.Def = def;
//		this.Speed =speed;
//    }
//
////    public enum EquipmentType
////    {
////        None,
////        Head,
////        Neck,
////        Chest,
////        Ring,
////        Leg,
////        Bracer,
////        Boots,
////        Shoulder,
////        Belt,
////        OffHand
////    }
//
//    public override string GetToolTipText()
//    {
//        string text = base.GetToolTipText();
//
//        string equipTypeText = "";
//		switch (ItemType)
//	{
//		case 5:
//                equipTypeText="武器";
//         break;
////        case EquipmentType.Neck:
////                equipTypeText="脖子";
////         break;
////        case EquipmentType.Chest:
////                equipTypeText="胸部";
////         break;
////        case EquipmentType.Ring:
////                equipTypeText="戒指";
////         break;
////        case EquipmentType.Leg:
////                equipTypeText="腿部";
////         break;
////        case EquipmentType.Bracer:
////                equipTypeText="护腕";
////         break;
////        case EquipmentType.Boots:
////                equipTypeText="靴子";
////         break;
////        case EquipmentType.Shoulder:
////                equipTypeText="护肩";
////         break;
////        case EquipmentType.Belt:
////                equipTypeText = "腰带";
////         break;
////        case EquipmentType.OffHand:
////                equipTypeText="副手";
////         break;
//	}
//
//		string newText = string.Format("{0}\n\n<color=blue>装备类型：{1}\n攻击：{2}\n防御：{3}\n速度：{4}</color>", text,equipTypeText,Attack,Def,Speed);
//
//        return newText;
//    }
//}
