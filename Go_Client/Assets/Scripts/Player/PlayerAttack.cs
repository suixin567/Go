using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour {
    float lastAttTime = 0;//保存上一次普通攻击的时间
    public float attackInterval = 1f;
    public float attackDistance = 3f;
    public Transform attackTarget;//攻击的敌人对象
    public Vector3 attackTargetPos;//攻击的具体位置
    PlayerController playerController;
    public enum AttackState
    {
        RunTo,
        Attack,
        Wait
    }
    public AttackState aniState = AttackState.Wait;
 //   public int currentSkill = 0;//本次攻击使用的技能
    GameObject normalAttackEffPre;//普通攻击
    GameObject firBallEffPre;//火球术

    public PlayerSkill playerSkill;

    void Start () {
        playerController = GetComponent<PlayerController>();
        normalAttackEffPre = Resources.Load<GameObject>("Skills/normalSkill");
        firBallEffPre = Resources.Load<GameObject>("Skills/fireBallSkill");
        playerSkill = GetComponent<PlayerSkill>();
    }

    public void localPalyerAttack(Transform _attackTarget) {
        attackTarget = _attackTarget;
    }


    void Update () {
        //主角普通攻击逻辑
        if (gameObject.tag == Tags.localPlayer && !EventSystem.current.IsPointerOverGameObject())
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
                //else
                //{//点击非敌人，不需要在这里改变状态，move中会去做改变。
                //    attackTarget = null;
                //}
            }
        }

        //普通攻击逻辑
        if (playerController.playerMotionState == PlayerMotionState.ATTACK) {
            //有具体目标
                float dis = Vector3.Distance(transform.position, attackTarget.position);
                if (dis <= attackDistance)//可以攻击
                {
                    //判断时间间隔
                    if (Time.time > lastAttTime + attackInterval)
                    {
                        lastAttTime = Time.time;
                        //攻击
                        aniState = AttackState.Attack;
                        AttackLogic();
                    }
                    else
                    {//等待间隔
                        aniState = AttackState.Wait;
                    }
                }
                else
                {//走向敌人
                    playerController.Move(attackTarget.position);
                    aniState = AttackState.RunTo;
                }
            }
        
        //判断是否是RunTo状态
        if (aniState== AttackState.RunTo &&attackTarget!=null) {
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
    public void Attack(int skillId , Transform attackTar=null ,float tarPosX=0, float tarPosY = 0, float tarPosZ = 0) {
        attackTarget = attackTar;
        if (tarPosX!=0 && tarPosZ!=0) {
            attackTargetPos = new Vector3(tarPosX, tarPosY, tarPosZ);
        }

        if (skillId!=0)//带技能的攻击
        {       
            playerController.playerMotionState = PlayerMotionState.SKILL;
            playerSkill.runSkill(skillId, attackTarget, attackTargetPos);
        }
        else//普通攻击
        {
            playerController.playerMotionState = PlayerMotionState.ATTACK;
        }
    }


    /// <summary>
    /// 具体攻击逻辑
    /// </summary>
    /// <param name="attackTar"></param>
    /// <param name="skill"></param>
    public void AttackLogic()
    {
        //面向敌人
        transform.LookAt(attackTarget);

        //普通攻击
        GameObject skillEff = Instantiate(normalAttackEffPre);
        skillEff.transform.position = transform.position + new Vector3(0, 1, 0);
        skillEff.GetComponent<SkillBase>().skillTar = attackTarget;
        skillEff.GetComponent<SkillBase>().skillTarPos = attackTargetPos;
        skillEff.GetComponent<SkillBase>().damage = GetComponent<PlayerProperties>().M_playerModel.Atk;
        skillEff.GetComponent<SkillBase>().destoryTargetEvent += skillCallBack;
        sendAttack(0);//无技能的普通攻击消息
    }


    /// <summary>
    /// 发送网络消息
    /// </summary>
        void sendAttack(int skillID) {
        if (tag != Tags.localPlayer)
        {
            return;
        }
        //发送攻击的网络协议
        AttackMonDTO dto = new AttackMonDTO();
        dto.Player = "";
        if (attackTarget != null)
        {//有具体目标
            if (attackTarget.tag == Tags.player)//攻击角色
            {
                dto.TarPlayer = attackTarget.GetComponent<PlayerProperties>().M_playerModel.Name;
            }
            else
            {//攻击怪物
                dto.FirstIndex = attackTarget.GetComponent<MonsterBase>().monModel.FirstIndex;
                dto.SecondIndex = attackTarget.GetComponent<MonsterBase>().monModel.SecondIndex;
            }
        }
        else {//没有具体目标的空技能
            dto.TarPos = new Assets.Model.Vector3(attackTargetPos);
        }
        dto.Skill = skillID;
        string message = LitJson.JsonMapper.ToJson(dto);
        NetWorkManager.instance.sendMessage(Protocol.MAP, GameInfo.myPlayerModel.Map, MapProtocol.ATTACK_MON_CREQ, message);
    }

        void skillCallBack()
        {
            playerController.playerMotionState = PlayerMotionState.IDEL;
            print("杀死了怪物，攻击停止");
        }
}
