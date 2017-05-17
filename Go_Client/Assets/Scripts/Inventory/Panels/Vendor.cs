using UnityEngine;
using System.Collections;

public class Vendor : Inventory {

//    #region 单例模式
//    private static Vendor _instance;
//    public static Vendor Instance
//    {
//        get
//        {
//            if (_instance == null)
//            {
//                _instance = GameObject.Find("VendorPanel").GetComponent<Vendor>();
//            }
//            return _instance;
//        }
//    }
//    #endregion

    public int[] itemIdArray;

	bool isInited=false;

    public override void Start()
    {
        base.Start();
        Hide();
    }

	//获取数据库物品数据后初始化商店。
    public void InitShop()
    {
        foreach (int itemId in itemIdArray)
        {
            StoreItem(itemId);
        }
    }

	public override void Show ()
	{
		base.Show ();
		if(isInited==false)
		{
			InitShop();
			isInited =true;
		}
	}
    /// <summary>
    /// 主角购买
    /// </summary>
    /// <param name="item"></param>
    public void BuyItem(Item item)
    {
		if (GameInfo.myPlayerModel.Gold >= item.BuyPrice) //TODO:判断是否背包有空位置。
			{
			string message= Coding<Item>.encode(item);
		//	print(message);
			NetWorkManager.getInstance().sendMessage(Protocol.ITEM, 0, ItemProtocal.BUY_CREQ,message);
			}else{
			print("钱不够啊");
		}
    }
    /// <summary>
    /// 主角出售物品
    /// </summary>
    //public void SellItem()
    //{
    //    int sellAmount = 1;
    //    if (Input.GetKey(KeyCode.LeftControl))
    //    {
    //        sellAmount = 1;
    //    }
    //    else
    //    {
    //        sellAmount = InventoryManager.Instance.PickedItem.Amount;
    //    }

    //    int coinAmount = InventoryManager.Instance.PickedItem.Item.SellPrice * sellAmount;
    //  //  player.EarnCoin(coinAmount);
    //    InventoryManager.Instance.ReducePickedItem(sellAmount);
    //}
}
