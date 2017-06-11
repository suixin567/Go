using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class PlayerMove : MonoBehaviour{

    public bool isLocalPlayer = false;
    public GameObject effect_click_prefab;
    Vector3 targetPosition;
    Vector3 forwardDir = Vector3.zero;

    private CharacterController controller;

    public float speed = 2;
    bool sendIdel = false;
    PlayerController playerController;


    void Start()
    {
        targetPosition = transform.position;
        if (gameObject.tag == Tags.localPlayer) isLocalPlayer = true;
        controller = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }


    void Update()
    {
        //主角移动逻辑
        if (isLocalPlayer == true && !EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + new Vector3(0, 0, 10));
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 200) && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer(Layers.ground))
                {
                    // ShowClickEffect(hitInfo.point); 
                   //更新目标点
                    targetPosition = new Vector3(hitInfo.point.x , transform.position.y, hitInfo.point.z);
                    forwardDir = targetPosition - transform.position;
                    sendMove();//移动了。发送数据
                    playerController.playerMotionState = PlayerMotionState.MOVE;
                }
            }
        }
        //通用逻辑
        if (playerController.playerMotionState == PlayerMotionState.MOVE)
        {
            float distance = Vector3.Distance(targetPosition, transform.position);
            if (distance > 0.1f)
            {
                controller.SimpleMove(transform.forward * speed);
                transform.forward = forwardDir;//只要在移动就 实时调整方向
                sendIdel = false;            
            }
            else {//走到了
                if (playerController.playerMotionState == PlayerMotionState.MOVE) {
                    playerController.playerMotionState = PlayerMotionState.IDEL;
                    transform.position = targetPosition;
           //         Debug.LogWarning("走到了" + targetPosition + "距离" + distance);
                }

            }
        }
        //if (playerController.playerMotionState == PlayerMotionState.IDEL) {
        //    if (isLocalPlayer == true && sendIdel == false)//是本地玩家就发送数据
        //    {
        //        sendIdle();
        //        sendIdel = true;
        //    }
        //}
    }

    //实例化出来点击的效果
    //void ShowClickEffect(Vector3 hitPoint)
    //{
    //    hitPoint = new Vector3(hitPoint.x, hitPoint.y + 0.1f, hitPoint.z);
    //    GameObject.Instantiate(effect_click_prefab, hitPoint, Quaternion.identity);
    //}

    //主角站立
    //public void Idel(Vector3 pos, Quaternion rotation)
    //{
    //    playerController.playerMotionState = PlayerMotionState.IDEL;
    //    targetPosition = pos;
    //    transform.position = pos;
    //    transform.rotation = rotation;
    //    sendIdle();
    //}

    //主角移动
    public void Move(Vector3 tar)
    {
        playerController.playerMotionState = PlayerMotionState.MOVE;
        targetPosition = tar;//更新目标点
        forwardDir = targetPosition - transform.position;
        sendMove();//发送数据
    }

    ///发送网络数据 ////////////////////////////////////////////////////
    void sendMove()
    {
        if (isLocalPlayer ==false) {
            return;
        }
        MoveDTO dto = new MoveDTO();
        dto.Name = GameInfo.myPlayerModel.Name;
        dto.Dir = 0;// state;//传输给其他玩家 此次操作方向 属于角色状态中部分常量
        dto.Point = new Assets.Model.Vector3(targetPosition);
        dto.Rotation = new Assets.Model.Vector4(transform.rotation);
        string message = LitJson.JsonMapper.ToJson(dto);
        NetWorkManager.instance.sendMessage(Protocol.MAP, GameInfo.myPlayerModel.Map, MapProtocol.MOVE_CREQ, message);
    }

    //void sendIdle()
    //{
    //    if (isLocalPlayer == false)
    //    {
    //        return;
    //    }
    //    MoveDTO dto = new MoveDTO();
    //    dto.Name = GameInfo.myPlayerModel.Name;
    //    dto.Dir = 0;// StateConstans.IDLE;
    //    dto.Point = new Assets.Model.Vector3(transform.position);
    //    dto.Rotation = new Assets.Model.Vector4(transform.rotation);
    //    string message = LitJson.JsonMapper.ToJson(dto);
    //    NetWorkManager.instance.sendMessage(Protocol.MAP, GameInfo.myPlayerModel.Map, MapProtocol.MOVE_CREQ, message);
    //}
}
