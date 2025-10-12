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
    int attackCount;

    //FSM state
    public enum FSMState 
    {
        None,   //0
        Idle,   //1
        Chase,   //2
        stAttack, //3
        ndAttack, //3
        rdAttack, //3
        Dead,   //4
    }
    protected FSMState curState;   //public because for looking the state


    // Start is called before the first frame update
    void Start()
    {
        moveLogic = GetComponent<move_logic>();
        attackLogic = GetComponent<attack_logic>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationState = 0;
        curState = FSMState.Idle;

        attackCount = 0;
        Invoke("updateIdleState", 2.0f);

    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case FSMState.Idle: UpdateIdleState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.stAttack: UpdateStAttackState(); break;
            case FSMState.ndAttack: UpdateNdAttackState(); break;
            case FSMState.rdAttack: UpdateRdAttackState(); break;

        }

        Debug.Log(curState);
        Debug.Log(moveLogic.CalculateMagnitude());
    }

    protected void UpdateIdleState()
    {
        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

        if (moveLogic.CalculateMagnitude() > 0.4f)  //chase to target if do not arrive
        {
            animationState = 1;
            animator.SetInteger("Action", animationState);

            curState = FSMState.Chase;
        }

        else if (moveLogic.CalculateMagnitude() <= 0.4f)    // attack to target if arrive
        {
            animationState = 10;
            animator.SetInteger("Action", animationState);

            curState = FSMState.stAttack;
        }
    }

    protected void UpdateChaseState() 
    {
        moveLogic.ChaseAutoPilot();

        if (moveLogic.CalculateMagnitude() <= 0.4f)    // idle for quick stop and then for next action    
        {
            animationState = 0;
            animator.SetInteger("Action", animationState);

            curState = FSMState.Idle;
        }
    }

    protected void UpdateStAttackState() 
    {
        attackLogic.AutoPilot();    //attack the target in the attack range

        if (moveLogic.CalculateMagnitude() > 0.4f)    // idle for quick stop and then for next action    
        {
            animationState = 0;
            animator.SetInteger("Action", animationState);

            curState = FSMState.Idle;
        }

        else if (moveLogic.CalculateMagnitude() <= 0.4f) 
        {
            animationState = 11;
            animator.SetInteger("Action", animationState);

            curState = FSMState.ndAttack;
        }
    }

    protected void UpdateNdAttackState()
    {
        attackLogic.AutoPilot();    //attack the target in the attack range

        if (moveLogic.CalculateMagnitude() > 0.4f)    // idle for quick stop and then for next action    
        {
            animationState = 0;
            animator.SetInteger("Action", animationState);

            curState = FSMState.Idle;
        }

        else if (moveLogic.CalculateMagnitude() <= 0.4f)
        {
            animationState = 12;
            animator.SetInteger("Action", animationState);

            curState = FSMState.rdAttack;
        }
    }

    protected void UpdateRdAttackState()
    {
        attackCount++;

        attackLogic.AutoPilot();    //attack the target in the attack range

        if (attackCount > 2) 
        {
            animationState = 0;
            animator.SetInteger("Action", animationState);

            curState = FSMState.Idle;
        }
    }
}
