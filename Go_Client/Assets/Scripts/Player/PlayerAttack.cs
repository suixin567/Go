using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
    float lastAttTime = 0;//保存上一次普通攻击的时间
    public float attackInterval = 1f;
    public float attackDistance = 3f;
    public Transform attackTarget;//攻击的敌人对象
    PlayerController playerController;
    public enum AttackState
    {
        RunTo,
        Attack,
        Wait
    }
    public AttackState aniState = AttackState.Wait;
    bool autoStoped = false;
    public int currentSkill = 0;//本次攻击使用的技能

    GameObject normalAttackEffPre;

    void Start () {
        playerController = GetComponent<PlayerController>();
        normalAttackEffPre = Resources.Load<GameObject>("Skills/normalSkill");
    }

    void Update () {
        //主角逻辑
        if (gameObject.tag == Tags.localPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isHit = Physics.Raycast(ray, out hit);
                if ((isHit && hit.collider.tag == Tags.enemy) || (isHit && hit.collider.tag == Tags.player))
                {
                    attackTarget = hit.collider.transform;
                    playerController.playerMotionState = PlayerMotionState.ATTACK;
                }
                else
                {//点击非敌人
                    attackTarget = null;
                }
                autoStoped = false;
            }
        }

        //通用逻辑
        if (playerController.playerMotionState == PlayerMotionState.ATTACK) {
			//如果此怪物已被杀死并销毁
			if(attackTarget ==null ){
                playerController.playerMotionState = PlayerMotionState.IDEL;
				attackTarget = null;
				return;
			}
            float dis = Vector3.Distance(transform.position,attackTarget.position);
            if (dis <= attackDistance)//可以攻击
            {             
                //判断时间间隔
                if (Time.time > lastAttTime + attackInterval){
                    lastAttTime = Time.time;
                    //攻击
                    aniState = AttackState.Attack;
					//面向敌人
					transform.LookAt(attackTarget);
                    AttackLogic();
                }
                else
                {//等待间隔
                    aniState = AttackState.Wait;
                }
            }
            else {//走向敌人
                    playerController.Move(attackTarget.position);
                    aniState = AttackState.RunTo;
            }
        }
        //判断是否是RunTo状态
        if (aniState== AttackState.RunTo) {
            float dis = Vector3.Distance(transform.position, attackTarget.position);
            if (dis <= attackDistance)//可以攻击
            {
                playerController.playerMotionState = PlayerMotionState.ATTACK;
            }
        }
	}

    /// <summary>
    /// 对外接口
    /// </summary>
    /// <param name="attackTar"></param>
    /// <param name="skill"></param>
    public void Attack( Transform attackTar,int skill) {
        currentSkill = skill;
        playerController.playerMotionState = PlayerMotionState.ATTACK;
        attackTarget = attackTar;
        sendAttack();//发送数据
    }

    /// <summary>
    /// 具体攻击逻辑
    /// </summary>
    /// <param name="attackTar"></param>
    /// <param name="skill"></param>
    public void AttackLogic()
    {
        if (currentSkill==0) {//普通攻击
            GameObject skillEff =  Instantiate(normalAttackEffPre);
            skillEff.transform.position = transform.position+ new Vector3(0,1,0);
            skillEff.GetComponent<SkillBase>().tar = attackTarget;
            skillEff.GetComponent<SkillBase>().damage = GetComponent<PlayerProperties>().M_playerModel.Atk;
        }
        sendAttack();
    }

        void sendAttack() {
        if (tag != Tags.localPlayer)
        {
            return;
        }
        //发送攻击的网络协议
        AttackMonDTO dto = new AttackMonDTO();
        dto.Player = "";
        if (attackTarget.tag == Tags.player)//攻击角色
        {
            dto.TarPlayer = attackTarget.GetComponent<PlayerProperties>().M_playerModel.Name;
        }
        else {//攻击怪物
            dto.FirstIndex = attackTarget.GetComponent<MonsterBase>().monModel.FirstIndex;
            dto.SecondIndex = attackTarget.GetComponent<MonsterBase>().monModel.SecondIndex;
        }



        dto.Skill = currentSkill;
        string message = LitJson.JsonMapper.ToJson(dto);
        NetWorkManager.instance.sendMessage(Protocol.MAP, GameInfo.myPlayerModel.Map, MapProtocol.ATTACK_MON_CREQ, message);
        //print("攻击");
    }
}
