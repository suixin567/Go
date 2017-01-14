using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Forge : Inventory {

    #region 单例模式
    private static Forge _instance;
    public static Forge Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("ForgePanel").GetComponent<Forge>();
            }
            return _instance;
        }
    }
    #endregion
    private List<Formula> formulaList;

    public override void Start()
    {
        base.Start();
        ParseFormulaJson();
    }

    void ParseFormulaJson()
    {
        formulaList = new List<Formula>();
        TextAsset formulasText = Resources.Load<TextAsset>("Formulas");
        string formulasJson = formulasText.text;//配方信息的Json数据
        JSONObject jo = new JSONObject(formulasJson);
        foreach (JSONObject temp in jo.list)
        {
            int item1ID = (int)temp["Item1ID"].n;
            int item1Amount = (int)temp["Item1Amount"].n;
            int item2ID = (int)temp["Item2ID"].n;
            int item2Amount = (int)temp["Item2Amount"].n;
            int resID = (int)temp["ResID"].n;
            Formula formula = new Formula(item1ID, item1Amount, item2ID, item2Amount, resID);
            formulaList.Add(formula);
        }
        //Debug.Log(formulaList[1].ResID);
    }

    public void ForgeItem(){
        // 得到当前有哪些材料
        // 判断满足哪一个秘籍的要求

        List<int> haveMaterialIDList = new List<int>();//存储当前拥有的材料的id
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI currentItemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                for (int i = 0; i < currentItemUI.Amount; i++)
                {
                    haveMaterialIDList.Add(currentItemUI.Item.Id);//这个格子里面有多少个物品 就存储多少个id
                }
            }
        }

        Formula matchedFormula = null;
        foreach (Formula formula in formulaList)
        {
            bool isMatch = formula.Match(haveMaterialIDList);
            if (isMatch)
            {
                matchedFormula = formula; break;
            }
        }
        if (matchedFormula != null)
        {
            Knapsack.Instance.StoreItem(matchedFormula.ResID);
            //去掉消耗的材料
            foreach(int id in matchedFormula.NeedIdList){
                foreach (Slot slot in slotList)
                {
                    if (slot.transform.childCount > 0)
                    {
                        ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                        if (itemUI.Item.Id == id&& itemUI.Amount > 0)
                        {
                            itemUI.ReduceAmount(); 
                            if (itemUI.Amount <= 0)
                            {
                                DestroyImmediate(itemUI.gameObject);
                            }
                            break;
                        }
                    }
                }
            }

        }

    }
}
