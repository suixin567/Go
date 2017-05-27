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
            //if (move.state == PlayerMove.ControlMotionState.Moving)
            //{
               
            //}
            //else if (move.state == PlayerMove.ControlMotionState.Idle)
            //{
            
            //}
            PlayAnim("Idle", 0.3f);
        } else if(playerController.playerMotionState == PlayerMotionState.MOVE) {
            PlayAnim("Run");
        }


        else if (playerController.playerMotionState == PlayerMotionState.ATTACK)//攻击状态
        {
            if (attack.aniState == PlayerAttack.AttackState.Attack)
            {
                PlayAnim("Attack1");
            }
            else if (attack.aniState == PlayerAttack.AttackState.Wait)
            {
                PlayAnim("Idle", 0.3f);
            }
            //else if (attack.aniState == PlayerAttack.AttackState.RunTo)
            //{
            //    PlayAnim("Run");
            //}
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
