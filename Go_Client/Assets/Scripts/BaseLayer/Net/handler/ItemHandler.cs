using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemHandler : MonoBehaviour {

    MapHandler mapHandler;
    private PlayerProperties localPlayerProperties;//本地角色模型
    private void Start()
    {
        mapHandler = Camera.main.GetComponent<MapHandler>();
    }
    public void OnMessage (SocketModel model)
	{
		switch(model.Command)
		{
		case ItemProtocal.INIT_SRES://初始化游戏物品
            print("初始化游戏物品");
			InventoryManager.Instance.initOriginalItem(model.Message);
			break;
		case ItemProtocal.PLAYER_ITEM_SRES://初始化人物物品
			initKnapsack(model.Message);
			break;
		case ItemProtocal.PLAYER_EQUIPMENT_SRES://初始化人物装备
            initEquipment(model.Message);
			break;
		case ItemProtocal.BUY_SRES://购买物品
			BuyItem(model.Message);
			break;
		case ItemProtocal.USE_SRES://使用物品
			UseItem(model.Message);
			break;
		case ItemProtocal.PUTON_SRES://穿装备后 响应回来的是人物的新属性
			putOnEquipment(model.Message);
			break;
        case ItemProtocal.PUTOFF_SRES://卸下装备的响应
            putOffEquipment(model.Message);
            break;
            case ItemProtocal.LEARN_SKILL_SRES://学技能的响应
            learnSkill(model.Message);
            break;
            case ItemProtocal.PLAYER_SKILL_SRES://初始化人物技能
                initSkill(model.Message);
                break;
            default:
			break;
		}
	}

    void learnSkill(string message)
    {
        print("学会技能："+message);
        if (message=="false") {
            Debug.LogError("已学会此技能");
          //  Knapsack.Instance.lastUsedItem.addAmount(1);
            return;
        }
        Skill newSkill = SkillManager._instance.json2Skill(message);
        SkillPanel.instance.creatSkillItem(newSkill);

    }

    //初始化技能栏
    void initSkill(string message)
    {
        print("我有这么多的技能"+message);
    //    Skill[] mySkills = Coding<Skill[]>.decode(message);
        List<Skill> skills = SkillManager._instance.jsons2Skills(message);

        for (int i = 0; i < skills.Count; i++)
        {
            SkillPanel.instance.creatSkillItem(skills[i]);
        }
    }

    //初始化背包
    void initKnapsack(string message)
	{
		if(message!=string.Empty)
		{
			Knapsack.Instance.readPlayerItems(message);
		}
	}

	void initEquipment(string message)
	{
		if(message==string.Empty)
		{
			return;
		}
		print("初始化装备面板物品");
		EquipmentPanel.Instance.readPlayerEquipments(message);

	}


	//购买物品的反馈
	void BuyItem(string message)
	{
		if(message==string.Empty)
		{
			Debug.LogError("客户端买得起，而服务器买不起，这不应该发生！");
		}else{
			Item buyedItem =  Coding<Item>.decode(message);
			GameInfo.myPlayerModel.Gold -= buyedItem.BuyPrice;
			Knapsack.Instance.StoreItem(buyedItem);
		}
	}

	void UseItem(string message)  //如何区分使用的什么物品  dto: 人物名字 物品名字 
	{
        UseItemDto ud = Coding<UseItemDto>.decode (message);
        //if (ud.Name == GameInfo.myPlayerModel.Name) //如果是自己使用的
        //{
        //    Knapsack.Instance.lastUsedItem.ReduceAmount();
        //}
        switch (ud.ItemId)
        {
            case 1000://小瓶红药
                mapHandler.playerGoList[ud.Name].GetComponent<PlayerProperties>().updateBlood(InventoryManager.Instance.GetItemById(ud.ItemId).Hp);
                break;
            case 1001://大瓶红药
                mapHandler.playerGoList[ud.Name].GetComponent<PlayerProperties>().updateBlood(InventoryManager.Instance.GetItemById(ud.ItemId).Hp);
                break;
            default:
                break;
        }
    }

	void putOnEquipment(string message)
	{
		PlayerModel pdto = Coding<PlayerModel>.decode(message);
        mapHandler.playerGoList[pdto.Name].GetComponent<PlayerProperties>().M_playerModel = pdto;
    }

    void putOffEquipment(string message)
    {
        PlayerModel pdto = Coding<PlayerModel>.decode(message);
        mapHandler.playerGoList[pdto.Name].GetComponent<PlayerProperties>().M_playerModel = pdto;
    }

}
