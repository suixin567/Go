using UnityEngine;
using System.Collections;

public class SkillBase : MonoBehaviour
{

    public float speed = 5;
    public Transform skillTar;
    public Vector3 skillTarPos;
    public int damage = 0;//攻击力

    public delegate void DestoryTargetEvent();
    public DestoryTargetEvent destoryTargetEvent;

    public enum SkillType {
        noTar,
        haveTar
    }
    public SkillType skillType = SkillType.noTar;

    public virtual void Start()
    {
        if (skillTar!=null) {
            skillType = SkillType.haveTar;
        }
    }


    void OnDestroy() {
        if (skillType == SkillType.haveTar) {//有目标的攻击
            if (skillTar == null)
            {//已经被销毁
                sendDieEvent();
            }
            else {//没有被销毁，则去检查血量，小于0则死掉了。
                if (skillTar.tag == Tags.enemy)//攻击怪物
                {
                    if (skillTar.GetComponent<MonsterBase>().monModel.Hp <= 0) {
                        //怪物死亡了
                        sendDieEvent();
                    }
                } else
                {//攻击人物
                    if (skillTar.GetComponent<PlayerProperties>().M_playerModel.Hp <= 0) {
                        //人物死亡了
                        sendDieEvent();
                    }                   
                }
            }
        }
    }

    void sendDieEvent() {
        if (destoryTargetEvent != null)
        {
            destoryTargetEvent();
        }
    }
}
