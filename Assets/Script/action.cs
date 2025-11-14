using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static action;

public class action : MonoBehaviour
{
    protected GameObject nearestTarget;
    protected teamManager team_Manager;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    // property
    protected float speed = 2.5f;


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
        team_Manager = GetComponent<teamManager>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationState = 0;
        curState = FSMState.Idle;

        attackCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        nearestTarget = team_Manager.nearestTarget;

        switch (curState)
        {
            case FSMState.Idle: UpdateIdleState(nearestTarget); break;
            case FSMState.Chase: UpdateChaseState(nearestTarget); break;
            case FSMState.stAttack: UpdateStAttackState(nearestTarget); break;
            case FSMState.ndAttack: UpdateNdAttackState(nearestTarget); break;
            case FSMState.rdAttack: UpdateRdAttackState(nearestTarget); break;

        }

        //Debug.Log(curState);
        //Debug.Log(CalculateMagnitude());
        //Debug.Log(attackCount);
    }

    protected void UpdateIdleState(GameObject target)
    {
        if (this.transform.position.x < target.transform.position.x)
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
            if (CalculateMagnitude() > 0.4f)  //chase to target if do not arrive
            {
                animationState = 1;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Chase;

                DistTimer = 0f;
                StartCoroutine(AniLengthDetector());
            }

            else if (CalculateMagnitude() <= 0.4f)    // attack to target if arrive
            {
                animationState = 10;
                animator.SetInteger("Action", animationState);

                curState = FSMState.stAttack;

                DistTimer = 0f;
                StartCoroutine(AniLengthDetector());
            }
        }
    }

    protected void UpdateChaseState(GameObject target) 
    {
        Vector3 movement = CalculateDistance(nearestTarget);
        movement.Normalize();
        this.transform.Translate(movement * speed * Time.deltaTime);

        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

        if (CalculateMagnitude() <= 0.4f)    // idle for quick stop and then for next action    
        {
            animationState = 0;
            animator.SetInteger("Action", animationState);

            curState = FSMState.Idle;
        }
    }

    protected void UpdateStAttackState(GameObject target) 
    {
        attackCount=1;

        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

        DistTimer += Time.deltaTime;

        if (DistTimer>=currentAniLength + 0.3f) 
        {
            if (CalculateMagnitude() > 0.4f)    // idle for quick stop and then for next action    
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Idle;

                DistTimer=0f;
                StartCoroutine(AniLengthDetector());
            }

            else if (CalculateMagnitude() <= 0.4f)
            {
                animationState = 11;
                animator.SetInteger("Action", animationState);

                curState = FSMState.ndAttack;

                DistTimer=0f;
                StartCoroutine(AniLengthDetector());
            }
        }
    }

    protected void UpdateNdAttackState(GameObject target)
    {
        attackCount=2;

        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

        DistTimer +=Time.deltaTime;

        if (DistTimer >= currentAniLength + 0.3f) 
        {
            if (CalculateMagnitude() > 0.4f)    // idle for quick stop and then for next action    
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Idle;

                DistTimer=0f;
                StartCoroutine(AniLengthDetector());
            }

            else if (CalculateMagnitude() <= 0.4f)
            {
                animationState = 12;
                animator.SetInteger("Action", animationState);

                curState = FSMState.rdAttack;

                DistTimer = 0f;
                StartCoroutine(AniLengthDetector());
            }
        }
    }

    protected void UpdateRdAttackState(GameObject target)
    {
        attackCount=3;

        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

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

    //////////////////////////////////////////////////////////////////////////////////
    
    Vector3 CalculateDistance(GameObject target)
    {
        Vector3 fD = target.transform.position - this.transform.position;
        return fD;
    }

    internal float CalculateMagnitude()
    {
        return CalculateDistance(nearestTarget).magnitude;
    }
}
