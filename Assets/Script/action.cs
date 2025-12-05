using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static action;

public class action : MonoBehaviour
{
    protected Animator animator;
    protected AnimatorStateInfo currentAniState;
    protected SpriteRenderer spriteRenderer;
    protected GameObject target;
    protected character_property charP;

    // property
    protected float speed;
    protected float attackRange;

    //animation state
    internal int animationState;
    int attackCount;

    //timer
    float DistTimer = 0f;
    float currentAniLength = 0f;

    //determine
    internal bool isAttacking;
    internal bool isAttacked;
    //private bool triggerAttackOn = false; 

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
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationState = 0;
        curState = FSMState.Idle;

        attackCount = 0;
        charP = GetComponent<character_property>();
        speed = charP.speed;
        attackRange = charP.atkRange;

        isAttacking = false;
        isAttacked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) 
        {
            if (curState != FSMState.Idle) 
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);
                curState = FSMState.Idle;
            }
            return;
        }

        if (charP._hp <=0 && curState != FSMState.Dead) 
        {
            curState = FSMState.Dead;
        }

        switch (curState)
        {
            case FSMState.Idle: UpdateIdleState(target); break;
            case FSMState.Chase: UpdateChaseState(target); break;
            case FSMState.stAttack: UpdateStAttackState(target); break;
            case FSMState.ndAttack: UpdateNdAttackState(target); break;
            case FSMState.rdAttack: UpdateRdAttackState(target); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }

        //Debug.Log(curState);
        //Debug.Log(CalculateMagnitude());
        //Debug.Log(attackCount);
        //Debug.Log("isAttack " + attackCount);
        //Debug.Log("isAttacked " + isAttacked);
        //Debug.Log($"{curState}+{currentAniLength}+{attackCount}");
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
            if (CalculateMagnitude() > attackRange)  //chase to target if do not arrive
            {
                animationState = 1;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Chase;

                DistTimer = 0f;
                StartCoroutine(AniLengthDetector(animationState));
            }

            else if (CalculateMagnitude() <= attackRange)    // attack to target if arrive
            {
                animationState = 10;
                animator.SetInteger("Action", animationState);

                curState = FSMState.stAttack;

                DistTimer = 0f;
                StartCoroutine(AniLengthDetector(animationState));
            }
        }
    }

    protected void UpdateChaseState(GameObject target) 
    {
        Vector3 movement = CalculateDistance(target);
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

        if (CalculateMagnitude() <= attackRange)    // idle for quick stop and then for next action    
        {
            animationState = 0;
            animator.SetInteger("Action", animationState);

            curState = FSMState.Idle;
            StartCoroutine(AniLengthDetector(animationState));
        }
    }

    protected void UpdateStAttackState(GameObject target) 
    {

        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

        //ReceiveAnimatorTime();
        //if (currentAniState.normalizedTime >= 0.7f && !triggerAttackOn) 
        //{
        //    attackCount = 1;
        //    triggerAttackOn = true;
        //}

        DistTimer += Time.deltaTime;

        if (DistTimer>=currentAniLength + 0.3f) 
        {
            if (CalculateMagnitude() > attackRange)    // idle for quick stop and then for next action    
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Idle;

                DistTimer=0f;
                StartCoroutine(AniLengthDetector(animationState));
            }

            else if (CalculateMagnitude() <= attackRange)
            {
                animationState = 11;
                animator.SetInteger("Action", animationState);

                curState = FSMState.ndAttack;

                //triggerAttackOn = false;
                DistTimer =0f;
                attackCount = 0;
                StartCoroutine(AniLengthDetector(animationState));
            }
        }
    }

    protected void UpdateNdAttackState(GameObject target)
    {

        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

        //ReceiveAnimatorTime();
        //if (currentAniState.normalizedTime >= 0.7f && !triggerAttackOn)
        //{
        //    triggerAttackOn = true;
        //    attackCount = 2;
        //}

        DistTimer +=Time.deltaTime;

        if (DistTimer >= currentAniLength + 0.3f) 
        {
            if (CalculateMagnitude() > attackRange)    // idle for quick stop and then for next action    
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Idle;

                DistTimer=0f;
                attackCount = 0;
                StartCoroutine(AniLengthDetector(animationState));
            }

            else if (CalculateMagnitude() <= attackRange)
            {
                animationState = 12;
                animator.SetInteger("Action", animationState);

                curState = FSMState.rdAttack;

                //triggerAttackOn = false;
                DistTimer = 0f;
                StartCoroutine(AniLengthDetector(animationState));
            }
        }
    }

    protected void UpdateRdAttackState(GameObject target)
    {

        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

        //ReceiveAnimatorTime();
        //if (currentAniState.normalizedTime >= 0.7f && !triggerAttackOn)
        //{
        //    triggerAttackOn = true;
        //    attackCount = 3;
        //}

        DistTimer += Time.deltaTime;

        if (DistTimer >= currentAniLength + 0.3f) 
        {
            if (attackCount > 2)
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Idle;

               //triggerAttackOn = false;
                DistTimer = 0f;
                //attackCount = 0;
                StartCoroutine(AniLengthDetector(animationState));
            }

        }

    }

    protected void UpdateDeadState() 
    {
        Vector3 deadPos = new Vector3(300.0f, 0, 0f); 
        this.transform.position = deadPos;
        curState = FSMState.Idle;
    }

    IEnumerator AniLengthDetector(int animationState)
    {
        yield return null;
        ReceiveAnimatorTime();

        while (true) 
        {
            if (animator.GetInteger("Action") == animationState)
            {
                currentAniLength = currentAniState.length;
                if (animationState > 9 && animationState <= 12)
                {
                    if (currentAniLength >= currentAniLength*0.6f)
                    {
                        attackCount = animationState - 9;
                    }
                }

                else 
                {
                    attackCount = 0;
                }
            }
            yield return null;
        }
    }

    void ReceiveAnimatorTime() 
    {
        currentAniState = animator.GetCurrentAnimatorStateInfo(0);
    }

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="Calculate"></param>
    /// <returns></returns>

    Vector3 CalculateDistance(GameObject target)
    {
        Vector3 fD = target.transform.position - this.transform.position;
        return fD;
    }

    internal float CalculateMagnitude()
    {
        return CalculateDistance(target).magnitude;
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="nearestTarget"></param>

    public void ReceiveNearestTarget(GameObject nearestTarget) 
    {
        target = nearestTarget;
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if (target == null) return;
        character_property tarCharP = target.GetComponent<character_property>();

        if (tarCharP.atkRange <= 0.5f)
        {
            if (collider.gameObject == target)
            {
                isAttacked = true;
            }
        }

        else if (tarCharP.atkRange > 0.5f)
        {
            if (collider.CompareTag("Bullet"))
            {
                isAttacked = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider) 
    {
        if (target == null) return;

        if (collider.gameObject == target || collider.CompareTag("Bullet")) 
        {
            isAttacked = false;
        }
    }

    public bool IsAttacking()
    {
        if (attackCount > 0)
        {
            return isAttacking = true;
        }

        else if (attackCount <= 0)
        {
            return isAttacking = false;
        }

        else 
        {
            return isAttacking = false;
        }
    }

    public bool IsAttacked() 
    {
        return isAttacked;
    }
}
