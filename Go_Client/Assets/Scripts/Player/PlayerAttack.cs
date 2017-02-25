using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
    float lastAttTime = 0;//保存上一次普通攻击的时间
    public float attackInterval = 1f;
    public float attackDistance = 3f;
    public Transform attackTarget;
    PlayerDir playerDir;

    public enum AniState
    {
        RunTo,
        Attack,
        Wait
    }
    public AniState aniState = AniState.Wait;

    void Start () {
        playerDir = GetComponent<PlayerDir>();

    }

    bool autoMoved = false;
    bool autoStoped = false;
    void Update () {

        if (gameObject.tag != Tags.localPlayer)
        {
            return;
        }

            if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool isHit = Physics.Raycast(ray,out hit);
            if (isHit && hit.collider.tag == Tags.enemy)
            {
                attackTarget = hit.collider.transform;
                playerDir.Player_Motion_State = PlayerMotionState.ATTACK;
            }
            else {
                playerDir.Player_Motion_State = PlayerMotionState.NORMAL;
                attackTarget = null;
            }
            autoMoved = false;
            autoStoped = false;
        }
        if (playerDir.Player_Motion_State == PlayerMotionState.ATTACK) {
			//如果此怪物已被杀死并销毁
			if(attackTarget ==null ){
				playerDir.Player_Motion_State = PlayerMotionState.NORMAL;
				attackTarget = null;
				return;
			}

            float dis = Vector3.Distance(transform.position,attackTarget.position);
            if (dis <= attackDistance)//可以攻击
            {
                if (autoStoped==false)
                {
                    playerDir.autoMove(transform.position);//如果之前是自动跑向目标，应该停下脚步
                    autoStoped = true;
                }
               
                //判断时间间隔
                if (Time.time > lastAttTime + attackInterval)
                {
                    lastAttTime = Time.time;
                    //攻击
                    aniState = AniState.Attack;
                    //发送攻击的网络协议
                    AttackMonDTO dto = new AttackMonDTO();
					dto.FirstIndex = attackTarget.GetComponent<MonsterBase>().monModel.FirstIndex;
					dto.SecondIndex = attackTarget.GetComponent<MonsterBase>().monModel.SecondIndex;
                    string message = LitJson.JsonMapper.ToJson(dto);
                    NetWorkManager.getInstance().sendMessage(Protocol.MAP, GameInfo.myPlayerModel.Map, MapProtocol.ATTACK_CREQ, message);
               //     print("攻击");
                }
                else
                {//等待间隔
                    aniState = AniState.Wait;
                }
            }
            else {//走向敌人
                if (autoMoved == false) {
                    playerDir.autoMove(attackTarget.position);
                    aniState = AniState.RunTo;
                    autoMoved = true;
                }
            }
        }
	}
}
