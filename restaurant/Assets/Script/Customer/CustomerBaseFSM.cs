using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBaseFSM : MonoBehaviour
{
    public IState currenState;
    public Dictionary<State_Enum, IState> states = new Dictionary<State_Enum, IState>();
    private Move move;

    public bool NavBool = true;//Move
    public int[] EndPointPosC = new int[2];
    public Animator ani;


    private void Awake()
    {
        ani = GetComponent<Animator>();
        move = new Move(this, gameObject);
        states.Add(State_Enum.idle, new Idle(this));
        states.Add(State_Enum.move, move);
        states.Add(State_Enum.waitforwaiter, new WaitForWaiter(this));
        states.Add(State_Enum.waitforfood, new WaitForFood(this));
        states.Add(State_Enum.eat, new Eat(this));
        TransitionState(State_Enum.idle);
    }

    public virtual void TransitionState(State_Enum type)
    {
        if (currenState != null)
        {
            currenState.OnExit();
        }
        currenState = states[type];
        currenState.OnEnter();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currenState.OnUpdate();
        
        if(NavBool)
        {
            move.SetNowPoint(EndPointPosC);
            NavBool = false;
            TransitionState(State_Enum.move);
        }
    }
}
