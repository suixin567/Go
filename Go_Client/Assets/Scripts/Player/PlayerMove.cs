using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour{

    public bool isLocalPlayer = false;
    public GameObject effect_click_prefab;
    private Vector3 targetPosition = Vector3.zero;
    private CharacterController controller;

    public float speed = 2;
    bool sendIdel = false;
    PlayerController playerController;

    void Start()
    {
        targetPosition = transform.position;
        if (gameObject.tag == Tags.localPlayer)
            isLocalPlayer = true;
        //角色控制器
        controller = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }


    void Update()
    {
        //主角逻辑
        if (isLocalPlayer == true && !EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == Tags.ground)
                {
                    // ShowClickEffect(hitInfo.point); 
                    targetPosition = hitInfo.point;//更新目标点
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
                LookAtTarget();//只要在移动就 实时调整方向
                sendIdel = false;

            }
            else {
                playerController.playerMotionState = PlayerMotionState.IDEL;
            }
        }
        if (playerController.playerMotionState == PlayerMotionState.IDEL) {
            if (isLocalPlayer == true && sendIdel == false)//是本地玩家就发送数据
            {
                sendIdle();
                sendIdel = true;
            }
        }
    }

    //实例化出来点击的效果
    void ShowClickEffect(Vector3 hitPoint)
    {
        hitPoint = new Vector3(hitPoint.x, hitPoint.y + 0.1f, hitPoint.z);
        GameObject.Instantiate(effect_click_prefab, hitPoint, Quaternion.identity);
    }

    //让主角朝向目标位置
    void LookAtTarget()
    {
        targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);//防止面向地面
        if (Vector3.Distance(transform.position, targetPosition) > 0.2f)
        {
            transform.LookAt(targetPosition);
        }
    }



    public void Idel(Vector3 pos, Quaternion rotation)
    {
        playerController.playerMotionState = PlayerMotionState.IDEL;
        targetPosition = pos;
        transform.position = pos;
        transform.rotation = rotation;
        sendIdle();
    }

    //主角移动
    public void Move(Vector3 tar)
    {
        playerController.playerMotionState = PlayerMotionState.MOVE;
        targetPosition = tar;//更新目标点
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

    void sendIdle()
    {
        if (isLocalPlayer == false)
        {
            return;
        }
        MoveDTO dto = new MoveDTO();
        dto.Name = GameInfo.myPlayerModel.Name;
        dto.Dir = 0;// StateConstans.IDLE;
        dto.Point = new Assets.Model.Vector3(transform.position);
        dto.Rotation = new Assets.Model.Vector4(transform.rotation);
        string message = LitJson.JsonMapper.ToJson(dto);
        NetWorkManager.instance.sendMessage(Protocol.MAP, GameInfo.myPlayerModel.Map, MapProtocol.MOVE_CREQ, message);
    }
}
