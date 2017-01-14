
using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;

public class PlayerDir : MonoBehaviour
{
    public bool isLocalPlayer = false;

    public GameObject effect_click_prefab;

    public Vector3 targetPosition = Vector3.zero;



    void Start()
    {
        targetPosition = transform.position;
        if (gameObject.tag == Tags.localPlayer)
            isLocalPlayer = true;
    }


    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {//点在了ui上
            return;
        }
        LookAtTarget();
        if (isLocalPlayer == false) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.tag == Tags.ground)
            {
                //	ShowClickEffect(hitInfo.point); 
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
    {//int state
     //Infos.state = state;//设置角色当前状态为相应状态
        MoveDTO dto = new MoveDTO();
        dto.Name = GameInfo.myPlayerModel.Name;
        dto.Dir = 0;// state;//传输给其他玩家 此次操作方向 属于角色状态中部分常量
        dto.Point = new Assets.Model.Vector3(targetPosition);
        dto.Rotation = new Assets.Model.Vector4(transform.rotation);
        string message = LitJson.JsonMapper.ToJson(dto);
        NetWorkScript.getInstance().sendMessage(Protocol.MAP, GameInfo.myPlayerModel.Map, MapProtocol.MOVE_CREQ, message);
    }
}
