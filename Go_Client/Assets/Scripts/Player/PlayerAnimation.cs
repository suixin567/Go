using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    private PlayerDir dir;
    private PlayerMove move;
    private PlayerAttack attack;


	void Start () {
        dir = GetComponent<PlayerDir>();
		move =this.GetComponent<PlayerMove>();
        attack = this.GetComponent<PlayerAttack>();
    }
	


	void LateUpdate () {

        if (dir.Player_Motion_State == PlayerMotionState.NORMAL)
        {
            if (move.state == PlayerMove.ControlMotionState.Moving)
            {
                PlayAnim("Run");
            }
            else if (move.state == PlayerMove.ControlMotionState.Idle)
            {
                PlayAnim("Idle",0.3f);
            }
        }
        else if(dir.Player_Motion_State  == PlayerMotionState.ATTACK)//攻击状态
        {
            if (attack.aniState == PlayerAttack.AniState.Attack)
            {
                PlayAnim("Attack1");
            }
            else if (attack.aniState == PlayerAttack.AniState.Wait)
            {
				PlayAnim("Idle",0.3f);
            }
            else if (attack.aniState == PlayerAttack.AniState.RunTo)
            {
                PlayAnim("Run");
            }
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
