using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ShortCut : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public KeyCode keyCode;
    public Image icon;
    public Item shortCutItem;//此快捷栏保存的物品数据模型

	void Start () {
        icon = transform.FindChild("Image").GetComponent<Image>();
        icon.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(keyCode))
        {
            if (Knapsack.Instance. useItem(shortCutItem.Name))
            {
                //如果用完了就清空此物品的快捷栏
                if (Knapsack.Instance.CheckItem(shortCutItem.Name,1)==false)
                {
                    ShortCut[]  shortCuts = transform.parent.GetComponentsInChildren<ShortCut>();
                    foreach (var temp in shortCuts)
                    {
                        if (temp.shortCutItem!=null)
                        {
                            if (temp.shortCutItem.Name == Knapsack.Instance.lastUsedItem.Name) {
                                temp.icon.gameObject.SetActive(false);
                                temp.shortCutItem = null;
                            }
                        }
                    }
                }
            }
        }
	}


    public void OnPointerEnter(PointerEventData eventData)
    {
    
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    //把物品放入此快捷栏
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;//以下都是鼠标左键的逻辑
        if (InventoryManager.Instance.isPickedItem == true) {
            setItem();
        }  

    }

    //为快捷栏赋值  TODO:现在快捷栏什么都能放
    public void setItem()
    {
        icon.sprite = InventoryManager.Instance.PickedItem.GetComponent<Image>().sprite;
        icon.gameObject.SetActive(true);
        shortCutItem = InventoryManager.Instance.PickedItem.Item;

        //把拿着的物品放回背包
        for (int i = 0; i < InventoryManager.Instance.PickedItem.Amount; i++)
        {
            Knapsack.Instance.StoreItem(InventoryManager.Instance.PickedItem.Item);
        }
        InventoryManager.Instance.ReducePickedItem(InventoryManager.Instance.PickedItem.Amount);
    }
}
