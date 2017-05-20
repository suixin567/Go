using UnityEngine;
using System.Collections;

public class NetState
{

    //public const int RUN = 0;//正常运行状态
    //                         //public const int WINDOW=1;
    //public const int ACC_REG = 2;
    ////	public const int LOADING=3;
    //public const int WAIT = 4;//等待状态，不能做任何操作

    public const int RUN = 0;
    public const int WINDOW = 1;
    public const int ACC_REG = 2;
    public const int LOADING = 3;
    public const int PLAYER_CREATE = 4;
    public const int WAIT = 5;//等待状态，不能做任何操作

}