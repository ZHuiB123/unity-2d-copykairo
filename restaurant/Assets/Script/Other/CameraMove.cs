using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float MaxSize = 6;
    public float MinSize = 3;
    public static CameraMove _instance = null;
    //public BoxCollider2D Bounds = null; //�ƶ��ı߽�
    public Vector3 deceleration = new Vector3(1, 1, 0);//���ٶ�
    public Vector3
        minVec3,
        maxVec3;
    private Vector2 beginP = Vector2.zero;//����һ�����µ�  
    private Vector2 endP = Vector2.zero;//���ڶ���λ�ã���קλ�ã�  
    private Vector3 speed = Vector3.zero;
    public Camera eyeCamera = null; // ��ͼ���
    public bool isUpdateTouch = true; //�Ƿ����touch 

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
        //    minVec3 = Bounds.bounds.min;//��Χ��  
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
    //�ƶ�����
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
    ///��ʼ��λ�ã�Ϊ��������move��׼��
    void MoveBegin(Vector3 point)
    {
        beginP = point;
        speed = Vector3.zero;
    }
    ///����Ŀ��λ��
    void Moveing(Vector3 point)
    {
        //��¼����϶���λ�� ����  
        endP = point;
        Vector3 fir = eyeCamera.ScreenToWorldPoint(new Vector3(beginP.x, beginP.y, eyeCamera.nearClipPlane));//ת������������  
        Vector3 sec = eyeCamera.ScreenToWorldPoint(new Vector3(endP.x, endP.y, eyeCamera.nearClipPlane));
        speed = sec - fir;//��Ҫ�ƶ��� ����  
    }
    ///Move�������������
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
        x = x - speed.x;//����ƫ��  
        y = y - speed.y;

        //��Χ
        float cameraHeight = Camera.main.orthographicSize * 2;
        var cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        var cameraHalfWidth = eyeCamera.orthographic ? cameraSize.x / 2 : 0;
        var cameraHalfHeight = eyeCamera.orthographic ? cameraSize.y / 2 : 0;
        //��֤�����Ƴ���Χ��  

        x = Mathf.Clamp(x, minVec3.x + cameraHalfWidth, maxVec3.x - cameraHalfWidth);
        y = Mathf.Clamp(y, minVec3.y + cameraHalfHeight, maxVec3.y - cameraHalfHeight);
        //if (Bounds)
        //{
        //    float cameraHeight = Camera.main.orthographicSize * 2;
        //    var cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        //    var cameraHalfWidth = eyeCamera.orthographic ? cameraSize.x / 2 : 0;
        //    var cameraHalfHeight = eyeCamera.orthographic ? cameraSize.y / 2 : 0;
        //    //��֤�����Ƴ���Χ��  

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
