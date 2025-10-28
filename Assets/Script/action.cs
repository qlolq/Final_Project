using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static action;

public class action : MonoBehaviour
{
    public GameObject targets;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected move_logic moveLogic;
    protected attack_logic attackLogic;

    //animation state
    int animationState;
    int attackCount;

    //timer
    float DistTimer = 0f;
    float currentAniLength = 0f;

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

        //Debug.Log(curState);
        //Debug.Log(moveLogic.CalculateMagnitude());
        //Debug.Log(attackCount);
    }

    protected void UpdateIdleState()
    {
        if (this.transform.position.x < targets.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

        DistTimer += Time.deltaTime;

        if (DistTimer >= currentAniLength+0.3f) 
        {
            if (moveLogic.CalculateMagnitude() > 0.4f)  //chase to target if do not arrive
            {
                animationState = 1;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Chase;

                DistTimer = 0f;
                StartCoroutine(AniLengthDetector());
            }

            else if (moveLogic.CalculateMagnitude() <= 0.4f)    // attack to target if arrive
            {
                animationState = 10;
                animator.SetInteger("Action", animationState);

                curState = FSMState.stAttack;

                DistTimer = 0f;
                StartCoroutine(AniLengthDetector());
            }
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
        attackCount=1;

        attackLogic.AutoPilot();    //attack the target in the attack range

        DistTimer += Time.deltaTime;

        if (DistTimer>=currentAniLength + 0.3f) 
        {
            if (moveLogic.CalculateMagnitude() > 0.4f)    // idle for quick stop and then for next action    
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Idle;

                DistTimer=0f;
                StartCoroutine(AniLengthDetector());
            }

            else if (moveLogic.CalculateMagnitude() <= 0.4f)
            {
                animationState = 11;
                animator.SetInteger("Action", animationState);

                curState = FSMState.ndAttack;

                DistTimer=0f;
                StartCoroutine(AniLengthDetector());
            }
        }
    }

    protected void UpdateNdAttackState()
    {
        attackCount=2;

        attackLogic.AutoPilot();    //attack the target in the attack range

        DistTimer+=Time.deltaTime;

        if (DistTimer >= currentAniLength + 0.3f) 
        {
            if (moveLogic.CalculateMagnitude() > 0.4f)    // idle for quick stop and then for next action    
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Idle;

                DistTimer=0f;
                StartCoroutine(AniLengthDetector());
            }

            else if (moveLogic.CalculateMagnitude() <= 0.4f)
            {
                animationState = 12;
                animator.SetInteger("Action", animationState);

                curState = FSMState.rdAttack;

                DistTimer = 0f;
                StartCoroutine(AniLengthDetector());
            }
        }
    }

    protected void UpdateRdAttackState()
    {
        attackCount=3;

        attackLogic.AutoPilot();    //attack the target in the attack range

        DistTimer += Time.deltaTime;

        if (DistTimer >= currentAniLength + 0.3f) 
        {
            if (attackCount > 2)
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Idle;

                DistTimer = 0f;
                StartCoroutine(AniLengthDetector());
            }

        }

    }

    IEnumerator AniLengthDetector()
    {
        yield return null;
        currentAniLength = animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
