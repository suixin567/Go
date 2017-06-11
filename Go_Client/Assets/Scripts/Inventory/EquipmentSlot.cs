using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot {
    public int equipType;
//    public Weapon.WeaponType wpType;


    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)//右键卸下装备
        {
			if (InventoryManager.Instance.IsPickedItem == false && transform.Find("Item(Clone)")!=null)
            {
				ItemUI currentItemUI = transform.Find("Item(Clone)").GetComponent<ItemUI>();
                ////////////////***********************
                EquipmentPanel.Instance.sendPutOffMessage(currentItemUI.Item);
                Knapsack.Instance.StoreItem(currentItemUI.Item);
                DestroyImmediate(currentItemUI.gameObject);
                InventoryManager.Instance.HideToolTip();
            }
        }
        if (eventData.button != PointerEventData.InputButton.Left) return;
        // 手上有 东西
        if (InventoryManager.Instance.IsPickedItem == true)
        {
            ItemUI pickedItem = InventoryManager.Instance.PickedItem;
			if (transform.Find("Item(Clone)")!=null)//装备格子也有东西，也就是替换
            {
                if( IsRightItem(pickedItem.Item) ){//如果可以替换
					///////////////////////////////////////////
					EquipmentPanel.Instance. sendPutOnMessage(InventoryManager.Instance.PickedItem.Item);
					ItemUI currentItemUI  = transform.Find("Item(Clone)").GetComponent<ItemUI>();//当前装备槽里面的物品
                    InventoryManager.Instance.PickedItem.Exchange(currentItemUI);
                }
            }
            else//直接穿戴
            {
                if (IsRightItem(pickedItem.Item))
                {
                    //////////////////////////////////
					EquipmentPanel.Instance. sendPutOnMessage(InventoryManager.Instance.PickedItem.Item);
					this.StoreItem(InventoryManager.Instance.PickedItem.Item);
                    InventoryManager.Instance.ReducePickedItem(1);
                }
            }
        }
		else //手上没东西的情况 ,就是把已穿戴的装备拿起来
        {
			if (transform.Find("Item(Clone)")!=null)
            {
				ItemUI currentItemUI = transform.Find("Item(Clone)").GetComponent<ItemUI>();
                ////////////////***********************
                EquipmentPanel.Instance.sendPutOffMessage(currentItemUI.Item);
                InventoryManager.Instance.PickupItem(currentItemUI.Item, currentItemUI.Amount);
                Destroy(currentItemUI.gameObject);

            }
        }
    }
		

    //判断item是否适合放在这个位置
    public bool IsRightItem(Item item)
    {
	//	if ((item is Equipment && ((Equipment)(item)).ItemType == this.equipType))// ||
                  //  (item is Weapon && ((Weapon)(item)).WpType == this.wpType))
		if (item.ItemType == this.equipType)
        {
            return true;
        }
        return false;
    }
}
