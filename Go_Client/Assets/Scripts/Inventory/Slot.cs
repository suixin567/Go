using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 物品槽
/// </summary>
public class Slot : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler{

    public GameObject itemPrefab;
	float mouseTimer=0;
	public float doubleInterval =0.3f;//断定为双击的时间间隔
    /// <summary>
    /// 把item放在自身下面
    /// 如果自身下面已经有item了，amount++
    /// 如果没有 根据itemPrefab去实例化一个item，放在下面
    /// </summary>
    /// <param name="item"></param>
    public void StoreItem(Item item)
    {
        if (transform.childCount == 0)
        {
            GameObject itemGameObject = Instantiate(itemPrefab) as GameObject;
            itemGameObject.transform.SetParent(this.transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI>().SetItem(item);
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }


//    /// <summary>
//    /// 得到当前物品槽存储的物品类型
//    /// </summary>
//    /// <returns></returns>
//    public Item.ItemType GetItemType()
//    {
//        return transform.GetChild(0).GetComponent<ItemUI>().Item.Type;
//    }

    /// <summary>
    /// 得到物品的id
    /// </summary>
    /// <returns></returns>
    public int GetItemId()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Id;
    }

    internal string GetItemName()
    {
        if (transform.Find("Item(Clone)")!=null)
        {
            return transform.Find("Item(Clone)").GetComponent<ItemUI>().Item.Name;
        }

        return string.Empty;
    }

    public bool IsFilled()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.Item.Capacity;//当前的数量大于等于容量
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(transform.childCount>0)
            InventoryManager.Instance.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            string toolTipText = transform.GetChild(0).GetComponent<ItemUI>().Item.GetToolTipText();
            InventoryManager.Instance.ShowToolTip(toolTipText);
        }
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)//右键穿戴
        {
			if (InventoryManager.Instance.IsPickedItem==false&& transform.Find("Item(Clone)")!=null)
            {
				ItemUI currentItemUI = transform.Find("Item(Clone)").GetComponent<ItemUI>();//想穿戴的物品
				if (currentItemUI.Item.ItemType>=5)//5以上代表可穿戴物品
                {
                    currentItemUI.ReduceAmount(1);
					EquipmentPanel.Instance.PutOn(currentItemUI.Item);   
                }
            }
        }//穿戴装备结束

        if (eventData.button != PointerEventData.InputButton.Left) return;//以下都是鼠标左键的逻辑
		moveItem();
		bool doubleClic =HaveClickMouseTwice(doubleInterval,ref mouseTimer,0);
        //使用物品
        if (doubleClic && transform.Find("Item(Clone)")!=null)
		{
            //Knapsack.Instance.lastUsedItem = transform.FindChild("Item(Clone)").GetComponent<ItemUI>().Item;
            //Item usedItem = Knapsack.Instance.lastUsedItem;
            //string message= Coding<Item>.encode(usedItem);
            //NetWorkScript.getInstance().sendMessage(Protocol.ITEM, 0, ItemProtocal.USE_CREQ,message);
            //         transform.FindChild("Item(Clone)").GetComponent<ItemUI>().ReduceAmount();//使用后减少一个物品
            useItem();

        }
	}

    //使用物品
    public void useItem()
    {
        Knapsack.Instance.lastUsedItem = transform.Find("Item(Clone)").GetComponent<ItemUI>().Item;
        Item usedItem = Knapsack.Instance.lastUsedItem;
        string message = Coding<Item>.encode(usedItem);
        NetWorkManager.instance.sendMessage(Protocol.ITEM, 0, ItemProtocal.USE_CREQ, message);
        transform.Find("Item(Clone)").GetComponent<ItemUI>().ReduceAmount();//使用后减少一个物品
    }

	void moveItem()
	{
		if (transform.childCount > 0)
		{
			ItemUI currentItem = transform.Find("Item(Clone)").GetComponent<ItemUI>();//获取格子里的物品
			if (InventoryManager.Instance.IsPickedItem == false)//拿取逻辑
			{
					InventoryManager.Instance.PickupItem(currentItem.Item,currentItem.Amount);
					Destroy(currentItem.gameObject);//销毁当前物品
			}else//交换逻辑
			{      
				if (currentItem.Item.Id == InventoryManager.Instance.PickedItem.Item.Id)//拿取的物品与要交换的物品同类型
				{
						if (currentItem.Item.Capacity > currentItem.Amount)//这个格子没有满
						{
							int amountRemain = currentItem.Item.Capacity - currentItem.Amount;//当前物品槽剩余的空间
							if (amountRemain >= InventoryManager.Instance.PickedItem.Amount)//有足够容量进行合并
							{
								currentItem.SetAmount(currentItem.Amount + InventoryManager.Instance.PickedItem.Amount);
								InventoryManager.Instance.ReducePickedItem(InventoryManager.Instance.PickedItem.Amount);
							}
							else//合并到满格，剩下的还是剩下的
							{
								currentItem.SetAmount(currentItem.Amount + amountRemain);
								InventoryManager.Instance.ReducePickedItem(amountRemain);
							}
						}
						else
						{
							return;
						}
				}
				else//直接交换
				{
					Item item = currentItem.Item;
					int amount = currentItem.Amount;
					currentItem.SetItem(InventoryManager.Instance.PickedItem.Item, InventoryManager.Instance.PickedItem.Amount);
					InventoryManager.Instance.PickedItem.SetItem(item, amount);
				}
			}
		}
		else//点了一个空格子
		{
			if (InventoryManager.Instance.IsPickedItem == true)//如果拿了物品就放进这个空格子
			{
					for (int i = 0; i < InventoryManager.Instance.PickedItem.Amount; i++)
					{
						this.StoreItem(InventoryManager.Instance.PickedItem.Item);
					}
					InventoryManager.Instance.ReducePickedItem(InventoryManager.Instance.PickedItem.Amount);
			}
			else
			{
				return;
			}
		}
	}



public bool HaveClickMouseTwice(float offsetTime,ref float Timer, int keyNum)
{
	if (Input.GetMouseButtonDown(keyNum))
	{
		return HaveExecuteTwiceAtTime(offsetTime, ref Timer);
	}
	return false;
}
public static bool HaveExecuteTwiceAtTime(float offsetTime,ref float timer)
{
	if (Time.time - timer < offsetTime)
	{
		return true;
	}
	else
	{
		timer = Time.time;
		return false;
	}
}
}
