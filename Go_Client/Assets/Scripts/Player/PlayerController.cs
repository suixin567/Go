using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public PlayerMotionState playerMotionState = PlayerMotionState.IDEL;

    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        ////主角攻击逻辑
        //if (gameObject.tag == Tags.localPlayer && !EventSystem.current.IsPointerOverGameObject())
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        RaycastHit hit;
        //        bool isHit = Physics.Raycast(ray, out hit);
        //        if ((isHit && hit.collider.tag == Tags.enemy) || (isHit && hit.collider.tag == Tags.player))
        //        {
        //            attackTarget = hit.collider.transform;
        //            playerController.playerMotionState = PlayerMotionState.ATTACK;
        //        }
        //        else
        //        {//点击非敌人，不需要在这里改变状态，move中会去做改变。
        //            attackTarget = null;
        //        }
        //    }
        //}

        ////主角移动逻辑
        //if (gameObject.tag == Tags.localPlayer && !EventSystem.current.IsPointerOverGameObject())
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + new Vector3(0, 0, 10));
        //        RaycastHit hitInfo;
        //        if (Physics.Raycast(ray, out hitInfo, 200) && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer(Layers.ground))
        //        {
        //            // ShowClickEffect(hitInfo.point); 
        //            targetPosition = hitInfo.point;//更新目标点
        //            sendMove();//移动了。发送数据
        //            playerController.playerMotionState = PlayerMotionState.MOVE;
        //        }
        //    }
        //}
    }


    //public void Idel(Vector3 pos,Quaternion rotation) {
    //    GetComponent<PlayerMove>().Idel(pos, rotation);
    //}


    public void Move(Vector3 tarPos)
    {
        GetComponent<PlayerMove>().Move(tarPos);
    }

    public void Attack(int skillId = 0 ,Transform attackTar=null, float tarPosX = 0, float tarPosY = 0, float tarPosZ = 0)
    {
        GetComponent<PlayerAttack>().Attack(skillId, attackTar, tarPosX, tarPosY, tarPosZ);
    }

}
