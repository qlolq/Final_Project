using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static action;

public class action : MonoBehaviour
{
    public GameObject target;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected move_logic moveLogic;
    protected attack_logic attackLogic;

    //animation state
    int animationState;

    //FSM state
    public enum FSMState 
    {
        None,   //0
        Idle,   //1
        Chase,   //2
        Attack, //3
        Dead,   //4
    }
    public FSMState curState;   //public because for looking the state


    // Start is called before the first frame update
    void Start()
    {
        moveLogic = GetComponent<move_logic>();
        attackLogic = GetComponent<attack_logic>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationState = 0;

        curState = FSMState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case FSMState.Idle: UpdateIdleState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack: UpdateAttackState(); break;
        }

        Debug.Log(animationState);
    }

    protected void UpdateIdleState()
    {
        animationState = 0;
        animator.SetInteger("Action", animationState);

        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

        if (moveLogic.CalculateMagnitude() > 0.8f)  //chase to target if do not arrive
        {
            curState = FSMState.Chase;
        }

        else if (moveLogic.CalculateMagnitude() <= 0.8f)    // attack to target if arrive
        {
            curState = FSMState.Attack;
        }
    }

    protected void UpdateChaseState() 
    {
        animationState = 1;
        animator.SetInteger("Action", animationState);

        moveLogic.ChaseAutoPilot();

        if (moveLogic.CalculateMagnitude() <= 0.8f)    // idle for quick stop and then for next action    
        {
            curState = FSMState.Idle;
        }
    }

    protected void UpdateAttackState() 
    {
        animationState = 11;
        animator.SetInteger("Action", animationState);

        attackLogic.AutoPilot();    //attack the target in the attack range
        
        curState = FSMState.Idle;
    }
}
