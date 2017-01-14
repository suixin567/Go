using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Property : MonoBehaviour {

    public static Property _instance;
    private bool isShow = false;

	private Text attackText;
	private Text defText;
	private Text speedText;
	private Text bloodText;
	private Text pointRemainText;
    //    private GameObject attackButton;
    //    private GameObject defButton;
    //    private GameObject speedButton;

    void Awake() {
        _instance = this;
		attackText = transform.Find("attackText").GetComponent<Text>();
		defText = transform.Find("defText").GetComponent<Text>();
		speedText = transform.Find("speedText").GetComponent<Text>();
		bloodText = transform.Find("bloodText").GetComponent<Text>();
		pointRemainText = transform.Find("pointRemainText").GetComponent<Text>();
//		attackButton = transform.Find("attackButton").gameObject;
//		defButton = transform.Find("defButton").gameObject;
//		speedButton = transform.Find("speedButton").gameObject;
    }

    void Start() {
        Hide();
    }

	public void Show()
	{
		UpdateInfo();
		isShow=true;
		gameObject.SetActive(true);
	}
	public void Hide()
	{
		isShow =false;
		gameObject.SetActive(false);
	}
	public void DisplaySwitch()
	{
		if (isShow == false)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

    private void Update()
    {
        UpdateInfo();
    }
    public void UpdateInfo() {// 更新显示 根据ps playerstatus的属性值，去更新显示
        attackText.text = GameInfo.myPlayerModel.Atk.ToString();
		defText.text = GameInfo.myPlayerModel.Def.ToString();
		bloodText.text = GameInfo.myPlayerModel.Hp.ToString()+"/" +  GameInfo.myPlayerModel.MaxHP.ToString();
		speedText.text = "暂时没有速度属性";
		pointRemainText.text = "暂时没有加点功能";
//		if (playerProperties.point_remain > 0) {
//            attackButton.SetActive(true);
//            defButton.SetActive(true);
//            speedButton.SetActive(true);
//        } else {
//            attackButton.SetActive(false);
//            defButton.SetActive(false);
//            speedButton.SetActive(false);
//        }
    }

//    public void OnAttackPlusClick() {
//		bool success = playerProperties.GetPoint();
//        if (success) {
//			playerProperties.attack_plus++;
//            UpdateShow();
//        }
//    }
//    public void OnDefPlusClick() {
//		bool success = playerProperties.GetPoint();
//        if (success) {
//			playerProperties.def_plus++;
//            UpdateShow();
//        }
//    }
//    public void OnSpeedPlusClick() {
//		bool success = playerProperties.GetPoint();
//        if (success) {
//			playerProperties.speed_plus++;
//            UpdateShow();
//        }
//    }
}
