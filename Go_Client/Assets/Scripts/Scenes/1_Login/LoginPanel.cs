using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {


    public GameObject regPanel;
    public GameObject loginPanel;

    public InputField accInput;
    public InputField pwdInput;
    public Image accTip;
    public Image psdTip;

    bool accRight = false;
    bool psdRight = false;

    public Toggle remeberToggle;

    void Start()
    {

        if (GameInfo.IS_SETUP == false)
        {
            GameInfo.IS_SETUP = true;
            Instantiate(Resources.Load<GameObject>("GlobalManager"));
        }

        accTip.gameObject.SetActive(false);
        psdTip.gameObject.SetActive(false);
        if (PlayerPrefs.HasKey("remeberAcc"))
        {
            if (PlayerPrefs.GetInt("remeberAcc") == 0)
            {
                remeberToggle.isOn = false;
            }
            else
            {
                remeberToggle.isOn = true;
            }
        }
        else
        {
            remeberToggle.isOn = false;
        }

        if (remeberToggle.isOn)
        {
            if (PlayerPrefs.HasKey("account"))
                accInput.text = PlayerPrefs.GetString("account");
            if (PlayerPrefs.HasKey("passWord"))
                pwdInput.text = PlayerPrefs.GetString("passWord");
        }
    }

    private void Update()
    {

        //账号
        if (IsRightFormat(accInput.text))
        {
            accTip.gameObject.SetActive(true); accRight = true;
        }
        else
        {
            accTip.gameObject.SetActive(false); accRight = false;
        }
        //密码
        if (IsRightFormat(pwdInput.text))
        {
            psdTip.gameObject.SetActive(true); psdRight = true;
        }
        else
        {
            psdTip.gameObject.SetActive(false); psdRight = false;
        }
        //回车键 登录
        if (NetWorkManager.NET_STATE == NetState.RUN)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                LoginClick();
            }
        }
    }

    /// <summary>
    /// 注册按钮被点击
    /// </summary>
    public void regClick()
    {
        if (NetWorkManager.NET_STATE == NetState.RUN)
        {
            NetWorkManager.NET_STATE = NetState.ACC_REG;
            regPanel.SetActive(true);
            loginPanel.SetActive(false);

        }
    }

    /// <summary>
    /// 登录按钮被点击
    /// </summary>
    public void LoginClick()
    {
        if (NetWorkManager.NET_STATE != NetState.RUN)
        {
            Debug.LogError("登录状态不对！");
            return;
        }
        

        if (accRight == true && psdRight == true)
        {
            print("发送登录请求");
            GameInfo.ACC_ID = accInput.text;
            //发送登陆到服务器
            LoginDTO dto = new LoginDTO();
            dto.userName = accInput.text;
            dto.passWord = pwdInput.text;
            string message = Coding<LoginDTO>.encode(dto);
            NetWorkManager.instance.sendMessage(Protocol.LOGIN, 0, LoginProtocol.LOGIN_CREQ, message);
            NetWorkManager.NET_STATE = NetState.WAIT;
            PlayerPrefs.SetString("account", accInput.text);
            PlayerPrefs.SetString("passWord", pwdInput.text);
            if (remeberToggle.isOn == true)
            {
                PlayerPrefs.SetInt("remeberAcc", 1);
            }
            else
            {
                PlayerPrefs.SetInt("remeberAcc", 0);
            }
            //变为等待状态
            NetWorkManager.NET_STATE = NetState.WAIT;
            //增加菊花
            //  LoadingPanelManager.instance.setLoadingPanel("login", transform, Vector2.zero, Vector2.one);
        }
        else
        {
            Debug.LogWarning("输入格式错误");
         //   TipManager._instance.setGameTip(TipConstants.INPUT_ERROR, 0, true, inputErrCallBack);
        }
    }

    //登陆时输入错误的回调
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
    // //对开头或结尾替换
    // string testStr = "i am blue cat";
    //string res = Regex.Replace(testStr, "^","开始");
    // res = Regex.Replace(res, "$", "结束");
    // print(res);
    // //检测是否纯数字
    // string pattern = @"^\d*$"; //\d是数字，就是以数字开始，*代表多个 ，$是以\d结束  
    // bool res3 = Regex.IsMatch("1a23",pattern);
    // print(res3);
    // //检测是否包含某个字符
    // string pattern1 = @"a*"; //包含一个a就可以
    // //只可以数字 _ 字母
    // string patternok = @"^\w*$";
    //     验证手机号码的主要代码如下：
}

//	public GameObject regPanel;

//	void Start()
//	{
//		if(GameInfo.IS_SETUP ==false)
//		{
//			GameInfo.IS_SETUP =true;
//			Instantiate(Resources.Load<GameObject>("GlobalManager"));
//		}
//	}


//	public InputField accText;
//	public InputField pwdText;

//	public void regClick()
//	{
//		if(GameInfo.GAME_STATE==GameState.RUN)
//		{
//			GameInfo.GAME_STATE=GameState.ACC_REG;
//			regPanel.SetActive (true);
//		}
//	}


//	public void LoginClick()
//	{
////		print(GameInfo.GAME_STATE);
//		if(GameInfo.GAME_STATE!=GameState.RUN)
//		{
//			return;
//		}
//		if (accText.text != string.Empty && pwdText.text != string.Empty) {
//			GameInfo.ACC_ID = accText.text;
//			//发送登陆到服务器
//			LoginDTO dto =new LoginDTO();
//			dto.userName=accText.text;
//			dto.passWord=pwdText.text;
//			string message = Coding<LoginDTO>.encode(dto);
//			NetWorkManager.instance.sendMessage(Protocol.LOGIN,0,LoginProtocol.LOGIN_CREQ,message);
//			GameInfo.GAME_STATE=GameState.WAIT;
//		} else 
//		{
//			WindowConstants.windowList.Add(WindowConstants.INPUT_ERROR);
//		}
//	}
//}
