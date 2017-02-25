using UnityEngine;
using System.Collections;

public class DrugNPC : NPC {


	public Transform VendorPanel_Prug;//商店

	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0)){
			VendorPanel_Prug.GetComponent<DrugVendorPanel>().DisplaySwitch();
		}
	}
}
