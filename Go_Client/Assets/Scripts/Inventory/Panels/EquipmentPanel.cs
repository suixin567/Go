using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EquipmentPanel : Inventory
{

	public static EquipmentPanel Instance;

	void Awake()
	{
		Instance=this;
	}

    public override void Start()
    {
        base.Start();
        Hide();
	}

	//读取人物的装备信息
	public void readPlayerEquipments(string _equipments)
	{
		List<Item> playerEquipmentList = InventoryManager.Instance.ParseItemJson(_equipments);
		foreach(var item in playerEquipmentList)
		{
			//初始化的穿装备不应该发送穿装备请求，否则会重复增加属性
			foreach (Slot slot in slotList)
			{
				EquipmentSlot equipmentSlot = (EquipmentSlot)slot;//遍历装备面板的每一个格子
				if (equipmentSlot.IsRightItem(item))  //判断item是否适合放在这个位置
				{
					{
						equipmentSlot.StoreItem(item);
					}
					break;
				}
			}
		}
	}


    public void PutOn(Item item)
    {
        foreach (Slot slot in slotList)
        {
            EquipmentSlot equipmentSlot = (EquipmentSlot)slot;//遍历装备面板的每一个格子
			if (equipmentSlot.IsRightItem(item))  //判断item是否适合放在这个位置
            {
				if (equipmentSlot.transform.FindChild("Item(Clone)")!=null)//替换已穿的装备
                {
					////////////////////////////
					sendPutOnMessage(item);
					ItemUI currentItemUI= equipmentSlot.transform.FindChild("Item(Clone)").GetComponent<ItemUI>();
					Knapsack.Instance.StoreItem(currentItemUI.Item);//被换下的装备要放进背包
                    currentItemUI.SetItem(item, 1);
                }
                else//直接穿戴
                {
					//////////////////////////////
					sendPutOnMessage(item);
                    equipmentSlot.StoreItem(item);
                }
                break;
			}
        }
     }


	public void sendPutOnMessage(Item item)
	{
		string message = Coding<Item>.encode(item);
		NetWorkScript.getInstance().sendMessage(Protocol.ITEM, 0, ItemProtocal.PUTON_CREQ, message);
	}

    public void sendPutOffMessage(Item item)
    {
        string message = Coding<Item>.encode(item);
        NetWorkScript.getInstance().sendMessage(Protocol.ITEM, 0, ItemProtocal.PUTOFF_CRES, message);
    }

//    private void UpdatePropertyText()
//    {
//        //Debug.Log("UpdatePropertyText");
//        int strength = 0, intellect = 0, agility = 0, stamina = 0, damage = 0;
//        foreach(EquipmentSlot slot in slotList){
//            if (slot.transform.childCount > 0)
//            {
//                Item item = slot.transform.GetChild(0).GetComponent<ItemUI>().Item;
//                if (item is Equipment)
//                {
//                    Equipment e = (Equipment)item;
//                    strength += e.Strength;
//                    intellect += e.Intellect;
//                    agility += e.Agility;
//                    stamina += e.Stamina;
//                }
//                else if (item is Weapon)
//                {
//                    damage += ((Weapon)item).Damage;
//                }
//            }
//        }
//        strength += player.BasicStrength;
//        intellect += player.BasicIntellect;
//        agility += player.BasicAgility;
//        stamina += player.BasicStamina;
//        damage += player.BasicDamage;
//        string text = string.Format("力量：{0}\n智力：{1}\n敏捷：{2}\n体力：{3}\n攻击力：{4} ", strength, intellect, agility, stamina, damage);
//        propertyText.text = text;
//    }

}
