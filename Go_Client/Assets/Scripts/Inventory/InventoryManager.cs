using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



public class InventoryManager : MonoBehaviour
{
	public static InventoryManager Instance;
    /// <summary>
    ///  物品信息的列表（集合）
    /// </summary>
    private List<Item> itemList;

    #region ToolTip
    private ToolTip toolTip;

    private bool isToolTipShow = false;

    private Vector2 toolTipPosionOffset = new Vector2(30, 0);
    #endregion

    private Canvas canvas;

    #region PickedItem
	public bool isPickedItem = false;

    public bool IsPickedItem
    {
        get
        {
            return isPickedItem;
        }
    }

    private ItemUI pickedItem;//鼠标物体

    public ItemUI PickedItem
    {
        get
        {
            return pickedItem;
        }
    }
    #endregion


    void Awake()
    {
		Instance=this;

    }

    void Start()
    {
        toolTip = GameObject.FindObjectOfType<ToolTip>();
        canvas = transform.GetComponent<Canvas>();
        pickedItem = GameObject.Find("PickedItem").GetComponent<ItemUI>();
        pickedItem.Hide();

		//初始化游戏物品
		NetWorkScript.getInstance().sendMessage(Protocol.ITEM, 0, ItemProtocal.INIT_CREQ, "");
		//获取一个玩家的装备数据
		NetWorkScript.getInstance().sendMessage(Protocol.ITEM, 0, ItemProtocal.PLAYER_EQUIPMENT_CREQ, "");
		//获取一个玩家的物品数据
		NetWorkScript.getInstance().sendMessage(Protocol.ITEM, 0, ItemProtocal.PLAYER_ITEM_CREQ, "");
    }

    void Update()
    {
        if (isPickedItem)
        {
            //如果我们捡起了物品，我们就要让物品跟随鼠标
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            pickedItem.SetLocalPosition(position);
        }
        else if (isToolTipShow)
        {
            //控制提示面板跟随鼠标
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toolTip.SetLocalPotion(position + toolTipPosionOffset);
        }

        //物品丢弃的处理
        if (isPickedItem && Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) == false)
        {
            isPickedItem = false;
            PickedItem.Hide();
        }
    }

    //初始化原生物品
    public void initOriginalItem(string itemsStr)
	{
		this.itemList  = ParseItemJson(itemsStr);
	}
		
    /// <summary>
    /// 解析物品信息
    /// </summary>
	public List<Item> ParseItemJson(string items)
    {
//		Item[] itemArr = Coding<Item[]>.decode (items);
//		print(itemArr.Length);
//		for (int i=0 ;i<itemArr.Length;i++)
//		{
//			itemInfoDict.Add(itemArr[i].ID, itemArr[i]);//添加到字典中，id为key，可以很方便的根据id查找到这个物品信息
//		}
//		return;

		List<Item> _itemList = new List<Item>();
      
		string itemsJson = items;//物品信息的Json格式
        JSONObject j = new JSONObject(itemsJson);
        foreach (JSONObject temp in j.list)
        {
			Item item = null; 

            //下面的事解析这个对象里面的公有属性
            int id = (int)(temp["Id"].n);
            string name = temp["Name"].str;
			int itemType = (int)(temp["ItemType"].n);
			string sprite = temp["Sprite"].str;
			string quality =temp["Quality"].str;
			int capacity = (int)(temp["Capacity"].n);
			int sellPrice = (int)(temp["SellPrice"].n);
			int buyPrice = (int)(temp["BuyPrice"].n);
            string description = temp["Description"].str;
//			//判断类别
//			switch (itemType)
//            {
//			case 0:
//				item = new MaterialItem(id, name, itemType,sprite, quality, capacity,sellPrice,buyPrice,description);
//				break;
//			case 1:        
                int hp = (int)(temp["Hp"].n);
                int mp = (int)(temp["Mp"].n);
//				item = new Consumable(id, name, itemType,sprite, quality, capacity,sellPrice,buyPrice,description, hp, mp);
//				break;
//			default:
				int attact = (int)temp["Attack"].n;
				int def = (int)temp["Def"].n;
				int speed = (int)temp["Speed"].n;                 
//				item = new Equipment(id, name, itemType,sprite, quality, capacity,sellPrice,buyPrice,description,  attact, def, speed);
//				break;
//            }
			item = new Item(id, name, itemType,sprite, quality, capacity,sellPrice,buyPrice,description,hp, mp,attact, def, speed);
			_itemList.Add(item);
        }
		return _itemList;
    }

    public Item GetItemById(int id)
    {
        foreach (Item item in itemList)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
        return null;
    }

    public void ShowToolTip(string content)
    {
        if (this.isPickedItem) return;
        isToolTipShow = true;
        toolTip.Show(content);
    }

    public void HideToolTip()
    {
        isToolTipShow = false;
        toolTip.Hide();
    }

    //捡起物品槽指定数量的物品
    public void PickupItem(Item item,int amount)
    {
        PickedItem.SetItem(item, amount);
        isPickedItem = true;

        PickedItem.Show();
        this.toolTip.Hide();
        //如果我们捡起了物品，我们就要让物品跟随鼠标
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
        pickedItem.SetLocalPosition(position);
    }

    /// <summary>
    /// 从手上拿掉一个物品放在物品槽里面
    /// </summary>
    public void ReducePickedItem(int amount=1)
    {
        PickedItem.ReduceAmount(amount);
        if (PickedItem.Amount <= 0)
        {
            isPickedItem = false;
            PickedItem.Hide();
        }
    }

//    public void SaveInventory()
//    {
//        Knapsack.Instance.SaveInventory();
//        Chest.Instance.SaveInventory();
//        EquipmentPanel.Instance.SaveInventory();
//        Forge.Instance.SaveInventory();
//        PlayerPrefs.SetInt("CoinAmount", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CoinAmount);
//    }
//
//    public void LoadInventory()
//    {
//        Knapsack.Instance.LoadInventory();
//        Chest.Instance.LoadInventory();
//        EquipmentPanel.Instance.LoadInventory();
//        Forge.Instance.LoadInventory();
//        if (PlayerPrefs.HasKey("CoinAmount"))
//        {
//            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CoinAmount = PlayerPrefs.GetInt("CoinAmount");
//        }
//    }

}