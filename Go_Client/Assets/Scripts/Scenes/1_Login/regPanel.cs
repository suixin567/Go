using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class regPanel : MonoBehaviour {


    public InputField accountInput;
    public InputField passWordInput;


    public Image accTip;
    public Image psdTip;


    bool accRight = false;
    bool psdRight = false;


    void Start()
    {
        gameObject.SetActive(false);
        accTip.gameObject.SetActive(false);
        psdTip.gameObject.SetActive(false);
    }

    private void Update()
    {

        //账号
        if (IsRightFormat(accountInput.text))
        {
            accTip.gameObject.SetActive(true); accRight = true;
        }
        else
        {
            accTip.gameObject.SetActive(false); accRight = false;
        }
        //密码
        if (IsRightFormat(passWordInput.text))
        {
            psdTip.gameObject.SetActive(true); psdRight = true;
        }
        else
        {
            psdTip.gameObject.SetActive(false); psdRight = false;
        }
       

        //if (NetWorkManager.NET_STATE == NetState.ACC_REG)
        //{
        //    //回车键 登录
        //    if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        //    {
        //        OnClick();
        //    }
        //}
    }

    /// <summary>
    /// 提交按钮被点击
    /// </summary>
    public void OnClick()
    {
        if (NetWorkManager.NET_STATE != NetState.ACC_REG)//点击了提交注册按钮，却不是在注册状态。
        {
        //    TipConstants.windowList.Add(TipConstants.STATE_ERROR);
            return;
        }



        if (accRight == true && psdRight == true)
        {
            //T送 注册数据
            LoginDTO dto = new LoginDTO();
            dto.userName = accountInput.text;
            dto.passWord = passWordInput.text;
            string message = Coding<LoginDTO>.encode(dto);
            //			print("json格式的注册账号与密码"+message );
            NetWorkManager.instance.sendMessage(Protocol.LOGIN, 2, LoginProtocol.REG_CREQ, message);
            //变为等待状态
            NetWorkManager.NET_STATE = NetState.WAIT;
        }
        else
        {
            Debug.LogWarning("输入错误");
         //   TipManager._instance.setGameTip(TipConstants.INPUT_ERROR, 0, true, inputErrCallBack);
        }
        NetWorkManager.NET_STATE = NetState.RUN;
        gameObject.SetActive(false);
        transform.parent.GetComponent<LoginPanel>().loginPanel.SetActive(true); ;
    }


    //注册时输入错误的回调
    void inputErrCallBack()
    {
        NetWorkManager.NET_STATE = NetState.RUN;
    }


    //账户密码格式校验
    public bool IsRightFormat(string str_handset)
    {
        bool temp = false;
        bool res = System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^\w{6,13}$");
        if (res == true)
        {
            temp = res;
        }
        //  bool res2 = System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"\(0\d{2.3}\)[- ]?\d{7,8}");
        //  print(res2);
        //bool res3 = System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^0\d{2,3}[- ]?\d{7,8}$");
        //if (res3 == true)
        //{
        //    temp = res3;
        //}
        return temp;
    }
}

//	public InputField accountInput;
//	public InputField passWordInput;

//	void Start () {
//		gameObject.SetActive (false);
//	}
//	public void OnClick()
//	{
//		if(GameInfo.GAME_STATE!=GameState.ACC_REG)//点击了提交注册按钮，却不是在注册状态。
//		{
//			WindowConstants.windowList.Add(WindowConstants.STATE_ERROR);
//			return;
//		}

//		if (accountInput.text != string.Empty && passWordInput.text != string.Empty) {
//			//T送 注册数据
//			LoginDTO dto =new LoginDTO();
//			dto.userName=accountInput.text;
//			dto.passWord=passWordInput.text;
//			string message = Coding<LoginDTO>.encode(dto);
////			print("json格式的注册账号与密码"+message );
//			NetWorkManager.instance.sendMessage(Protocol.LOGIN, 2,LoginProtocol.REG_CREQ,message);
//		} else 
//		{
//			WindowConstants.windowList.Add(WindowConstants.INPUT_ERROR);
//		}
//		gameObject.SetActive (false);
//		GameInfo.GAME_STATE = GameState.RUN;//将游戏状态切换回RUN
//	}

//}
