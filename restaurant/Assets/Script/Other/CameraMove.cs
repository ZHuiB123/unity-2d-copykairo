using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float MaxSize = 6;
    public float MinSize = 3;
    public static CameraMove _instance = null;
    //public BoxCollider2D Bounds = null; //移动的边界
    public Vector3 deceleration = new Vector3(1, 1, 0);//减速度
    public Vector3
        minVec3,
        maxVec3;
    private Vector2 beginP = Vector2.zero;//鼠标第一次落下点  
    private Vector2 endP = Vector2.zero;//鼠标第二次位置（拖拽位置）  
    private Vector3 speed = Vector3.zero;
    public Camera eyeCamera = null; // 视图相机
    public bool isUpdateTouch = true; //是否更新touch 

    void Awake()
    {
        _instance = this;
    }
    public void Start()
    {

        if (eyeCamera == null)
        {
            eyeCamera = Camera.main;
        }
        //if (Bounds)
        //{
        //    minVec3 = Bounds.bounds.min;//包围盒  
        //    maxVec3 = Bounds.bounds.max;
        //}

    }

    public void OnGUI()
    {
        #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
                return;
        #endif

        if (Event.current.type == EventType.MouseDown)
        {
            MoveBegin(Input.mousePosition);
        }
        else if (Event.current.type == EventType.MouseDrag)
        {
            Moveing(Input.mousePosition);
        }
    }
    //移动对象
    void UpdateTargetPositon()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        if (!isUpdateTouch)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Canceled || Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    isUpdateTouch = true;
                    break;
                }
            }
        }
        if (Input.touchCount == 1)
        {
            if (isUpdateTouch)
            {
                MoveBegin(Input.GetTouch(0).position);
                isUpdateTouch = false;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Moveing(Input.GetTouch(0).position);
            }
        }

    }
    ///初始化位置，为接下来的move做准备
    void MoveBegin(Vector3 point)
    {
        beginP = point;
        speed = Vector3.zero;
    }
    ///更新目标位置
    void Moveing(Vector3 point)
    {
        //记录鼠标拖动的位置 　　  
        endP = point;
        Vector3 fir = eyeCamera.ScreenToWorldPoint(new Vector3(beginP.x, beginP.y, eyeCamera.nearClipPlane));//转换至世界坐标  
        Vector3 sec = eyeCamera.ScreenToWorldPoint(new Vector3(endP.x, endP.y, eyeCamera.nearClipPlane));
        speed = sec - fir;//需要移动的 向量  
    }
    ///Move结束，清除数据
    void MoveEnd(Vector3 point)
    {
        MoveBegin(point);
    }

    public void Update()
    {
        #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
            UpdateTargetPositon();
        #endif
        float MouseScroolWheel = Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log(MouseScroolWheel);
        if (MouseScroolWheel != 0)
        {
            Camera.main.orthographicSize = Camera.main.orthographicSize - MouseScroolWheel;
            if (Camera.main.orthographicSize < MinSize) Camera.main.orthographicSize = MinSize;
            if (Camera.main.orthographicSize > MaxSize) Camera.main.orthographicSize = MaxSize;
        }
        if (speed == Vector3.zero)
        {
            return;
        }
        var x = transform.position.x;
        var y = transform.position.y;
        x = x - speed.x;//向量偏移  
        y = y - speed.y;

        //范围
        float cameraHeight = Camera.main.orthographicSize * 2;
        var cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        var cameraHalfWidth = eyeCamera.orthographic ? cameraSize.x / 2 : 0;
        var cameraHalfHeight = eyeCamera.orthographic ? cameraSize.y / 2 : 0;
        //保证不会移除包围盒  

        x = Mathf.Clamp(x, minVec3.x + cameraHalfWidth, maxVec3.x - cameraHalfWidth);
        y = Mathf.Clamp(y, minVec3.y + cameraHalfHeight, maxVec3.y - cameraHalfHeight);
        //if (Bounds)
        //{
        //    float cameraHeight = Camera.main.orthographicSize * 2;
        //    var cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        //    var cameraHalfWidth = eyeCamera.orthographic ? cameraSize.x / 2 : 0;
        //    var cameraHalfHeight = eyeCamera.orthographic ? cameraSize.y / 2 : 0;
        //    //保证不会移除包围盒  

        //    x = Mathf.Clamp(x, minVec3.x + cameraHalfWidth, maxVec3.x - cameraHalfWidth);
        //    y = Mathf.Clamp(y, minVec3.y + cameraHalfHeight, maxVec3.y - cameraHalfHeight);
        //}
        transform.position = new Vector3(x, y, transform.position.z);

        if (System.Math.Abs(speed.x) < 0.01f)
        {
            speed.x = 0;
        }
        else
        {
            if (speed.x > 0)
            {
                speed.x -= deceleration.x * Time.deltaTime;
                if (speed.x < 0)
                {
                    speed.x = 0;
                }
            }
            else
            {
                speed.x += deceleration.x * Time.deltaTime;
                if (speed.x > 0)
                {
                    speed.x = 0;
                }
            }
        }
        if (System.Math.Abs(speed.y) < 0.01f)
        {
            speed.y = 0;
        }
        else
        {
            if (speed.y > 0)
            {
                speed.y -= deceleration.y * Time.deltaTime;
                if (speed.y < 0)
                {
                    speed.y = 0;
                }
            }
            else
            {
                speed.y += deceleration.y * Time.deltaTime;
                if (speed.y > 0)
                {
                    speed.y = 0;
                }
            }
        }
        beginP = endP;
        if (speed.x == 0 && speed.y == 0)
        {
            speed = Vector3.zero;
        }
    }
}
