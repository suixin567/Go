using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public PlayerMotionState playerMotionState = PlayerMotionState.IDEL;

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z)) {
            Idel(new Vector3(-10,0,-10),new Quaternion(0,0,0,0));
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Move(new Vector3(-10, 0, -10));
        }
    }


    public void Idel(Vector3 pos,Quaternion rotation) {
        GetComponent<PlayerMove>().Idel(pos, rotation);
    }


    public void Move(Vector3 tarPos)
    {
        GetComponent<PlayerMove>().Move(tarPos);
    }

    public void Attack(int skillId = 0 ,Transform attackTar=null)
    {
        GetComponent<PlayerAttack>().Attack(skillId, attackTar);
    }

}
