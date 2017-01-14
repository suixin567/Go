using UnityEngine;
using System.Collections;


//物品类别目录
public enum GloableItemType
{
	MaterialItem=0,
	Consumable =1,
	Equipment =5
}

/// <summary>
/// 物品基类
/// </summary>
public class Item  {
	//一些通用属性
    public int Id { get; set; }
    public string Name { get; set; }
	public int ItemType { get; set; }
	public string Sprite { get; set; }
	public string Quality { get; set; }
	public int Capacity { get; set; }
	public int SellPrice { get; set; }
	public int BuyPrice { get; set; }
    public string Description { get; set; }
	public int Hp { get; set; }
	public int Mp { get; set; }
	public int Attack { get; set; }
	public int Def { get; set; }
	public int Speed{get;set;}

    public Item()
    {
        this.Id = -1;
    }

	public Item(int id, string name, int itemType,string sprite, string quality, int capacity,int sellPrice , int buyPrice,string description,
		int hp,int mp,int attack,int def,int speed)
    {
        this.Id = id;
        this.Name = name;
		this.ItemType = itemType;
		this.Sprite = sprite;
        this.Quality = quality;
        this.Capacity = capacity;
		this.SellPrice = sellPrice;
        this.BuyPrice = buyPrice;
		this.Description = description;
		this.Hp = hp;
		this.Mp = mp;
		this.Attack = attack;
		this.Def = def;
		this.Speed =speed;
    }


//    /// <summary>
//    /// 物品类型
//    /// </summary>
//    public enum ItemType
//    {
//        Consumable,
//        Equipment,
//        Weapon,
//        Material
//    }
//    /// <summary>
//    /// 品质
//    /// </summary>
//    public enum ItemQuality
//    {
//        Common,
//        Uncommon,
//        Rare,
//        Epic,
//        Legendary,
//        Artifact
//    }

    /// <summary> 
    /// 得到提示面板应该显示什么样的内容
    /// </summary>
    /// <returns></returns>
    public virtual string GetToolTipText()
    {
        string color = "";
        switch (Quality)
        {
		case "Common":
                color = "white";
                break;
		case "Rare":
                color = "lime";
                break;
        }
        string text = string.Format("<color={4}>{0}</color>\n<size=10><color=green>购买价格：{1} 出售价格：{2}</color></size>\n<color=yellow><size=10>{3}</size></color>", Name, BuyPrice, SellPrice, Description, color);
        return text;
    }
}
