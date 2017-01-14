using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public bool havePlayer =false;

    private Transform player;
    private Vector3 offsetPosition;//位置偏移
    private bool isRotating = false;

    public float distance = 0;
    public float scrollSpeed = 10;//拉近拉远的速度
    public float rotateSpeed = 2;


	public void InitFllow (Transform _player) {
		Vector3 cameraPos = _player.position;
		cameraPos.y+=3f;
		cameraPos.z-=5f;
		transform.position = cameraPos;

		player = _player;
        transform.LookAt(player.position);
        offsetPosition = transform.position - player.position;
		havePlayer =true;
	}
	
	// Update is called once per frame
	void Update () {
		if(havePlayer==false) return;
		//固定视野
        transform.position = offsetPosition + player.position;
        //处理视野的旋转
        RotateView();
        //处理视野的拉近和拉远效果
        ScrollView();
	}

    void ScrollView() {
        //print(Input.GetAxis("Mouse ScrollWheel"));//向后 返回负值 (拉近视野) 向前滑动 返回正值(拉远视野)
		distance = offsetPosition.magnitude;
        distance -= Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;
        distance = Mathf.Clamp(distance, 4, 18);
        offsetPosition = offsetPosition.normalized * distance;
    }

    void RotateView() {
        //Input.GetAxis("Mouse X");//得到鼠标在水平方向的滑动
        //Input.GetAxis("Mouse Y");//得到鼠标在垂直方向的滑动
        if (Input.GetMouseButtonDown(1)) {
            isRotating = true;
        }
        if (Input.GetMouseButtonUp(1)) {
            isRotating = false;
        }
        if (isRotating) {
            transform.RotateAround(player.position,player.up, rotateSpeed * Input.GetAxis("Mouse X"));
            Vector3 originalPos = transform.position;
            Quaternion originalRotation = transform.rotation;
            transform.RotateAround(player.position,transform.right, -rotateSpeed * Input.GetAxis("Mouse Y"));//影响的属性有两个 一个是position 一个是rotation
            float x = transform.eulerAngles.x;
            if (x < 10 || x > 80) {//当超出范围之后，我们将属性归位原来的，就是让旋转无效 
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }
        }
        offsetPosition = transform.position - player.position;
    }
}
