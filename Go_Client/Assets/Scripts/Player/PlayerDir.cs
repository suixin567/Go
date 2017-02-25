
using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;

public class PlayerDir : MonoBehaviour
{
    public bool isLocalPlayer = false;

    public GameObject effect_click_prefab;

    public Vector3 targetPosition = Vector3.zero;

    public  int Player_Motion_State = 0;


    void Start()
    {
        targetPosition = transform.position;
        if (gameObject.tag == Tags.localPlayer)
            isLocalPlayer = true;
    }


    void Update()
    {
        if (isLocalPlayer == false) return;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //if (GameInfo.Player_Motion_State!= PlayerMotionState.NORMAL)//乱七八糟的状态里不能移动
        //{
        //    return;
        //}
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == Tags.ground)
            {
                //	ShowClickEffect(hitInfo.point); 
                LookAtTarget();
                targetPosition = hitInfo.point;//更新目标点
                sendMove();//移动了。发送数据
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
    public void LookAtTarget()
    {
        targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);//防止面向地面
        if (Vector3.Distance(transform.position, targetPosition) > 0.2f)
        {
            this.transform.LookAt(targetPosition);
        }
    }


    //发送角色移动数据
    private void sendMove()
    {
        MoveDTO dto = new MoveDTO();
        dto.Name = GameInfo.myPlayerModel.Name;
        dto.Dir = 0;// state;//传输给其他玩家 此次操作方向 属于角色状态中部分常量
        dto.Point = new Assets.Model.Vector3(targetPosition);
        dto.Rotation = new Assets.Model.Vector4(transform.rotation);
        string message = LitJson.JsonMapper.ToJson(dto);
        NetWorkManager.getInstance().sendMessage(Protocol.MAP, GameInfo.myPlayerModel.Map, MapProtocol.MOVE_CREQ, message);
    }

    public void autoMove(Vector3 tar)
    {
        LookAtTarget();
        targetPosition = tar;//更新目标点
        sendMove();//移动了。发送数据
    }
}
