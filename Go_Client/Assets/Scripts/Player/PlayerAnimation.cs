using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    private PlayerMove playerMove;
    private PlayerMove move;
    private PlayerAttack attack;
    PlayerController playerController;

    void Start () {
        playerMove = GetComponent<PlayerMove>();
		move = GetComponent<PlayerMove>();
        attack = GetComponent<PlayerAttack>();
        playerController = GetComponent<PlayerController>();
    }
	


	void LateUpdate () {

        if (playerController.playerMotionState == PlayerMotionState.IDEL)
        {
            PlayAnim("Idle", 0.3f);
        }
            //移动动画
        else if (playerController.playerMotionState == PlayerMotionState.MOVE)
        {
            PlayAnim("Run");
        }

            //普通攻击动画
        else if (playerController.playerMotionState == PlayerMotionState.ATTACK)
        {
            if (attack.aniState == PlayerAttack.AttackState.Attack)
            {
                PlayAnim("Attack1");
            }
            else if (attack.aniState == PlayerAttack.AttackState.Wait)
            {
                PlayAnim("Idle", 0.3f);
            }
        }
        //释放技能动画
        else if (playerController.playerMotionState == PlayerMotionState.SKILL)
        {
            PlayAnim("Skill-GroundImpact");
        }

    }

//	void Play(string animName) {
//		GetComponent<Animation>().Play(animName);
//	}
	void PlayAnim(string animName,float time =0)
    {
		GetComponent<Animation>().CrossFade(animName,time);
	//
    }
}
