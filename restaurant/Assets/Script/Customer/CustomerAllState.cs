using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : IState
{
    private CustomerBaseFSM manager;
    public Eat(CustomerBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
        manager.ani.SetBool("IsEat", true);
        //Debug.Log("Eat");
    }

    public void OnExit()
    {
        manager.ani.SetBool("IsEat", false);
    }

    public void OnUpdate()
    {

    }
}


public class WaitForWaiter : IState
{
    private CustomerBaseFSM manager;
    public WaitForWaiter(CustomerBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
        manager.ani.SetBool("IsWait", true);
        //Debug.Log("Waiter");
        //manager.TransitionState(State_Enum.waitforfood);
    }

    public void OnExit()
    {
        manager.ani.SetBool("IsWait", false);
    }

    public void OnUpdate()
    {
        if(manager.ani.GetCurrentAnimatorStateInfo(0).IsName("WaitForWaiter"))
        {
            manager.TransitionState(State_Enum.waitforfood);
        }
    }
}

public class WaitForFood : IState
{
    private CustomerBaseFSM manager;
    public WaitForFood(CustomerBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
        manager.ani.SetBool("IsWaitForFood", true);
        //Debug.Log("Food");
        //manager.TransitionState(State_Enum.eat);
    }

    public void OnExit()
    {
        manager.ani.SetBool("IsWaitForFood", false);
    }

    public void OnUpdate()
    {
        if (manager.ani.GetCurrentAnimatorStateInfo(0).IsName("WaitForFood"))
        {
            manager.TransitionState(State_Enum.eat);
        }
    }
}

public class Move : IState
{
    public float speed = 1.5f;
    public GameObject table = null;

    private float accuracy = 0.04f;
    private Animator ani;
    private GameObject customer;
    private CustomerBaseFSM manager;
    private int[] EndPointPosC = new int[2];
    private Vector3 EndPoint = Vector3.zero;
    private NavSystem nav;
    private int id = 0;
    private Vector3[] dir_move = { Vector3.zero, new Vector3(-2, -1, 0).normalized, new Vector3(2, 1, 0).normalized, new Vector3(2, -1, 0).normalized, new Vector3(-2, 1, 0).normalized };//下上右左
    
    public Move(CustomerBaseFSM manager,GameObject customer)
    {
        this.manager = manager;
        this.customer = customer;
        ani = customer.GetComponent<Animator>();
    }
    
    public void OnEnter()
    {
        int table_id = -1;
        if (EndPointPosC[0] == 0 && EndPointPosC[1] == 0)
        {
            //bool pd = true;
            for (int i = 0; i < MapMessage._instance.BuildData.Count; i++)
            {
                if (MapMessage._instance.BuildData[i].serial == 0) table_id++;
                if (MapMessage._instance.BuildData[i].isidle)
                {
                    table = GameObject.Find("Map").transform.GetChild(table_id).GetChild(MapMessage._instance.BuildData[i].serial).gameObject;
                    //Debug.Log(table.name);
                    EndPoint = new Vector3((float)MapMessage._instance.BuildData[i].x, (float)MapMessage._instance.BuildData[i].y, 0);
                    MapMessage._instance.BuildData[i].isidle = false;
                    //pd = false;
                    break;
                }
            }
            //if(pd)//无座，等待
            //{

            //}
        }
        else
        {
            EndPoint = TransformCoordinate._instance.CoordinateToTransform(EndPointPosC[0], EndPointPosC[1]);
        }
        nav = new NavSystem(EndPoint,customer.transform.position);
        nav.NavStart();
    }

    public void OnExit()
    {
        ani.SetFloat("MoveX", 0.0f);
        ani.SetFloat("MoveY", 0.0f);
        ani.SetBool("IsMove", false);
    }

    public void OnUpdate()
    {
        if (id < nav.path.Count - 1)
        {
            Person p_next = nav.path[id + 1];
            int obj_dir_int = p_next.Getdir();
            ToMove(obj_dir_int);
            Vector3 next_p = TransformCoordinate._instance.CoordinateToTransform(p_next.Getx(), p_next.Gety());
            if (Vector3.Distance(customer.transform.position, next_p) < accuracy)
            {
                id++;
            }
        }
        else
        {
            if (id == nav.path.Count - 1)
            {
                SetMoveZero();
                if (table != null)
                {
                    if(table.GetComponent<SpriteMask>())
                    {
                        table.GetComponent<SpriteMask>().enabled = true;
                    }
                    manager.TransitionState(State_Enum.waitforwaiter);
                }
            }
        }
    }

    public void SetNowPoint(int[] EndPointPosC)
    {
        id = 0;
        this.EndPointPosC = EndPointPosC;
    }

    void ToMove(int obj_dir_int)
    {
        Vector3 obj_move = dir_move[obj_dir_int];
        customer.transform.Translate(obj_move * speed * Time.fixedDeltaTime, Space.World);
        if (obj_dir_int == 1) SetMove(0.0f, -1.0f);//below
        else if (obj_dir_int == 2) SetMove(0.0f, 1.0f);//on
        else if (obj_dir_int == 3) SetMove(1.0f, 0.0f);//right
        else if (obj_dir_int == 4) SetMove(-1.0f, 0.0f);//left
    }

    //行走动画
    void SetMove(float move_x, float move_y)
    {
        ani.SetFloat("MoveX", move_x);
        ani.SetFloat("MoveY", move_y);
        ani.SetBool("IsMove", true);
    }

    void SetMoveZero()
    {
        ani.SetFloat("MoveX", 0.0f);
        ani.SetFloat("MoveY", 0.0f);
        ani.SetBool("IsMove", false);
    }
}

public class Idle : IState
{
    private CustomerBaseFSM manager;
    //构造函数获取刚刚创建脚本的属性方法
    public Idle(CustomerBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
        //进入Idle状态脚本写在这里
    }

    public void OnExit()
    {
        //退出Idle状态脚本写在这里
    }

    public void OnUpdate()
    {
        //Idle动态的脚本写在这里
    }
}

public enum State_Enum
{
    idle,
    move,
    waitforwaiter,
    waitforfood,
    eat,
    leave
}


public interface IState
{
    //进入状态时调用一次
    void OnEnter();

    //处于状态时，连续调用
    void OnUpdate();

    //退出状态时调用一次
    void OnExit();

}
