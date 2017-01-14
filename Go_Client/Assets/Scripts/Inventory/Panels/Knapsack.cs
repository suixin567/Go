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

	public ItemUI lastUsedItem;//最近使用的物品

	public override void Start () {
		base.Start();
		DisplaySwitch();
	}

	//读取人物的物品信息
	public void readPlayerItems(string _playerItems)
	{
		print("初始化背包物品");
	//	Debug.LogWarning( _playerItems);
		playerItemList =  InventoryManager.Instance.ParseItemJson(_playerItems);
		foreach(var item in playerItemList)
		{
			StoreItem(item);
		}
	}

	public override void Show ()
	{
		base.Show ();
		goldText.text =GameInfo.myPlayerModel.Gold.ToString();//刷新金币
		//初始化背包里的物品
	}

	//立即刷新背包的金币数文本显示
	public void UpdateGoldText()
	{
		goldText.text =GameInfo.myPlayerModel.Gold.ToString();
	}


}
