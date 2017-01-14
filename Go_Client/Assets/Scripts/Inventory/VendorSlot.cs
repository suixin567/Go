using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class VendorSlot : Slot {
    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right &&InventoryManager.Instance.IsPickedItem==false )
        {
            if (transform.childCount > 0)
            {
                Item currentItem = transform.GetChild(0).GetComponent<ItemUI>().Item;
                transform.parent.parent.SendMessage("BuyItem", currentItem);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left && InventoryManager.Instance.IsPickedItem == true)
        {
            transform.parent.parent.SendMessage("SellItem");
        }
        
    }
}
