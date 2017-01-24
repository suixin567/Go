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
        }
        if (playerDir.Player_Motion_State == PlayerMotionState.ATTACK) {

            float dis = Vector3.Distance(transform.position,attackTarget.position);
            if (dis <= attackDistance)//可以攻击
            {
                playerDir.autoMove(transform.position);//如果之前是自动跑向目标，应该停下脚步
                //判断时间间隔
                if (Time.time > lastAttTime + attackInterval)
                {
                    lastAttTime = Time.time;
                    //攻击
                    print("攻击");
                    aniState = AniState.Attack;
                    //发送攻击的网络协议
                    AttackMonDTO dto = new AttackMonDTO();
                    dto.FirstIndex = attackTarget.GetComponent<MonsterBase>().model.FirstIndex;
                    dto.SecondIndex = attackTarget.GetComponent<MonsterBase>().model.SecondIndex;
                    string message = LitJson.JsonMapper.ToJson(dto);
                    NetWorkScript.getInstance().sendMessage(Protocol.MAP, GameInfo.myPlayerModel.Map, MapProtocol.ATTACK_CREQ, message);

                }
                else {//等待间隔
                    aniState = AniState.Wait;
                }
            }
            else {//走向敌人
                playerDir.autoMove(attackTarget.position);
                aniState = AniState.RunTo;
            }
        }
	}
}
