using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Knapsack : Inventory
{

    #region 单例模式
    private static Knapsack _instance;
    public static Knapsack Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance =  GameObject.Find("KnapsackPanel").GetComponent<Knapsack>();
            }
            return _instance;
        }
    }
    #endregion
	public List<Item> playerItemList;//角色背包中所拥有的物品

	public Text goldText;

	public Item lastUsedItem;//最近使用的物品

	public override void Start () {
		base.Start();
		DisplaySwitch();
	}

	//读取人物的物品信息
	public void readPlayerItems(string _playerItems)
	{
		print("初始化背包物品");
		//Debug.LogWarning( _playerItems);
		playerItemList =  InventoryManager.Instance.ParseItemJson(_playerItems);
		foreach(var item in playerItemList)
		{
			StoreItem(item);
		}
	}

	//立即刷新背包的金币数文本显示
	public void UpdateGoldText()
	{
		goldText.text =GameInfo.myPlayerModel.Gold.ToString();
	}

    public override void Update()
    {
        base.Update();
        UpdateGoldText();
    }


    //检测是否有足够数量的某物品
    public bool CheckItem(string name, int amount)
    {
        int tempAmount = 0;
        Slot[] slots = transform.GetComponentsInChildren<Slot>();
        for (int i = 0; i < slots.Length; i++)//遍历所有格子
        {
            if (slots[i].GetItemName() == name)
            {
                tempAmount++;
            }
        }
        if (tempAmount >= amount)
        {
            return true;
        }
        return false;
    }

    //使用某物品
    public bool useItem(string name) {
        Slot[] slots = transform.GetComponentsInChildren<Slot>();
        for (int i = 0; i < slots.Length; i++)//遍历所有格子
        {
            if (slots[i].GetItemName() == name)//找到物品
            {
                slots[i].GetComponent<Slot>().useItem();
                return true;
            }
        }
        return false;
    }

    public void onCloseBtn() {
        DisplaySwitch();
    }
}
