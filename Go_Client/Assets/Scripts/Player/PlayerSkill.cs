using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour {

   public List<Skill> mySkillList = new List<Skill>();


 //   public int currentSkill = 0;//本次攻击使用的技能
    GameObject firBallEffPre;//火球术
    PlayerController playerController;
    PlayerAttack playerAttack;

	void Start () {
        playerController = GetComponent<PlayerController>();
        playerAttack = GetComponent<PlayerAttack>();
        firBallEffPre = Resources.Load<GameObject>("Skills/fireBallSkill");
	}
	

	void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (var item in mySkillList)
            {

                if (item.Shortcut == 1)
                {
                    releaseSkill(item);
                    return;
                }
              
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            foreach (var item in mySkillList)
            {
                if (item.Shortcut == 2)
                {
                    releaseSkill(item);
                    return;
                }
             
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            foreach (var item in mySkillList)
            {
                if (item.Shortcut == 3)
                {
                    releaseSkill(item);
                    return;
                }              
            }
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            foreach (var item in mySkillList)
            {
                if (item.Shortcut == 4)
                {
                    releaseSkill(item);
                    return;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            foreach (var item in mySkillList)
            {
                if (item.Shortcut == 5)
                {
                    releaseSkill(item);
                    return;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            foreach (var item in mySkillList)
            {
                if (item.Shortcut == 6)
                {
                    releaseSkill(item);
                    return;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            foreach (var item in mySkillList)
            {
                if (item.Shortcut == 7)
                {
                    releaseSkill(item);
                    return;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            foreach (var item in mySkillList)
            {
                if (item.Shortcut == 8)
                {
                    releaseSkill(item);
                    return;
                }
            }
        }
	}


    //只有本地玩家才可以按键盘
    void releaseSkill(Skill skill)
    {
        //判断时间间隔，如果可以释放技能则找到目标 TODO:
       
        print("本地玩家释放技能" + skill.Name);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + new Vector3(0, 0, 10));
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 200))
        {
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer(Layers.ground))
            {//释放到地面上
                playerController.Attack(skill.Id, null, hitInfo.point.x,hitInfo.point.y,hitInfo.point.z);
                sendSkill(skill.Id, null, hitInfo.point);
            }
            else { //释放到目标身上
                playerController.Attack(skill.Id ,hitInfo.collider.transform);
                sendSkill(skill.Id, hitInfo.collider.transform);
               
            }
            
        } 
    }


       public void runSkill(int skillID, Transform attackTarget, Vector3 attackTargetPos)
       {
           print("执行技能逻辑");
           //面向敌人
           if (attackTarget!=null)
           {
               transform.LookAt(attackTarget);
           }else{
               transform.LookAt(new Vector3(attackTargetPos.x,transform.position.y, attackTargetPos.z));
           }

           if (skillID == 1)
           {//火球术
               GameObject skillEff = Instantiate(firBallEffPre);
               skillEff.transform.position = transform.position + new Vector3(0, 1, 0);
               skillEff.GetComponent<SkillBase>().skillTar = attackTarget;
               skillEff.GetComponent<SkillBase>().skillTarPos = attackTargetPos;
               skillEff.GetComponent<SkillBase>().damage = GetComponent<PlayerProperties>().M_playerModel.Atk;
               skillEff.GetComponent<SkillBase>().destoryTargetEvent += null;
           }
           Invoke("onSkillComplete", 1);
       }

       void onSkillComplete() {
           playerController.playerMotionState = PlayerMotionState.IDEL;
       }



       /// <summary>
       /// 发送网络消息
       /// </summary>
       void sendSkill(int skillID, Transform attackTarget, Vector3 attackTargetPos = new  Vector3())
       {
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
           else
           {//没有具体目标的空技能
               dto.TarPos = new Assets.Model.Vector3(attackTargetPos);
           }
           dto.Skill = skillID;
           string message = LitJson.JsonMapper.ToJson(dto);
           NetWorkManager.instance.sendMessage(Protocol.MAP, GameInfo.myPlayerModel.Map, MapProtocol.ATTACK_MON_CREQ, message);
       }
}
