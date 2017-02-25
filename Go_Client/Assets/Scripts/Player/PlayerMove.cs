using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public enum ControlMotionState {
		Moving,
		Idle
	}
	public ControlMotionState state = ControlMotionState.Idle;
	public float speed = 2;
	private PlayerDir dir;
	private CharacterController controller;

	//是否已经发送过Idel数据
	bool sendIdel =false;


	void Start () {
		dir = this.GetComponent<PlayerDir>();
		controller = this.GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance(dir.targetPosition, transform.position);
		if (distance > 0.1f) {
			state = ControlMotionState.Moving;
			controller.SimpleMove(transform.forward * speed);
			dir.LookAtTarget();//只要在移动就 实时调整方向
			sendIdel =false;
		} else {
			state = ControlMotionState.Idle;
			if(dir.isLocalPlayer ==true && sendIdel==false)//是本地玩家就发送数据
			{
				sendIdle();
				sendIdel =true;
			}
		}

	}




	private void sendIdle() {
		//Infos.state = StateConstans.IDLE;
		MoveDTO dto = new MoveDTO();
		dto.Name =GameInfo.myPlayerModel.Name;
		dto.Dir =0;// StateConstans.IDLE;
		dto.Point = new Assets.Model.Vector3(transform.position);
		dto.Rotation = new Assets.Model.Vector4(transform.rotation);
		string message = LitJson.JsonMapper.ToJson(dto);
		NetWorkManager.getInstance().sendMessage(Protocol.MAP, GameInfo.myPlayerModel.Map, MapProtocol.MOVE_CREQ, message);
	}

}
