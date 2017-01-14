using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	private PlayerMove move;

	// Use this for initialization
	void Start () {
		move =this.GetComponent<PlayerMove>();
	}
	


	void LateUpdate () {
		if (move.state == PlayerMove.ControlMotionState.Moving) {
			PlayAnim("Run");
		} else if (move.state == PlayerMove.ControlMotionState.Idle) {
			PlayAnim("Idle");
		}

	
	}

	void PlayAnim(string animName) {
		GetComponent<Animation>().CrossFade(animName);
	}

}
