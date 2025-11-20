using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static action;

public class action : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected GameObject target;

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
        character_property charP = GetComponent<character_property>();
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

        switch (curState)
        {
            case FSMState.Idle: UpdateIdleState(target); break;
            case FSMState.Chase: UpdateChaseState(target); break;
            case FSMState.stAttack: UpdateStAttackState(target); break;
            case FSMState.ndAttack: UpdateNdAttackState(target); break;
            case FSMState.rdAttack: UpdateRdAttackState(target); break;

        }

        //Debug.Log(curState);
        //Debug.Log(CalculateMagnitude());
        //Debug.Log(attackCount);
        //Debug.Log("isAttack " + attackCount);
        //Debug.Log("isAttacked " + isAttacked);
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
                StartCoroutine(AniLengthDetector());
            }

            else if (CalculateMagnitude() <= attackRange)    // attack to target if arrive
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
        }
    }

    protected void UpdateStAttackState(GameObject target) 
    {
        attackCount = 1;
        
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
            if (CalculateMagnitude() > attackRange)    // idle for quick stop and then for next action    
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Idle;

                DistTimer=0f;
                StartCoroutine(AniLengthDetector());
            }

            else if (CalculateMagnitude() <= attackRange)
            {
                animationState = 11;
                animator.SetInteger("Action", animationState);

                curState = FSMState.ndAttack;

                DistTimer=0f;
                attackCount = 0;
                StartCoroutine(AniLengthDetector());
            }
        }
    }

    protected void UpdateNdAttackState(GameObject target)
    {
        attackCount =2;

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
            if (CalculateMagnitude() > attackRange)    // idle for quick stop and then for next action    
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Idle;

                DistTimer=0f;
                attackCount = 0;
                StartCoroutine(AniLengthDetector());
            }

            else if (CalculateMagnitude() <= attackRange)
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
        attackCount =3;

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
                attackCount = 0;
                StartCoroutine(AniLengthDetector());
            }

        }

    }

    IEnumerator AniLengthDetector()
    {
        yield return null;
        currentAniLength = animator.GetCurrentAnimatorStateInfo(0).length;
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

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    character_property tarCharP = target.GetComponent<character_property>();

    //    if (target == null) return;

    //    if (tarCharP.atkRange <= 0.5)
    //    {
    //        if (collision.gameObject == target)
    //        {
    //            isAttacked = true;
    //        }
    //    }

    //    else if (tarCharP.atkRange > 0.5) 
    //    {
    //        if (collision.gameObject.tag == "Bullet")
    //        {
    //            isAttacked = true;
    //        }
    //    }
    //}

    //void OnCollisionExit2D(Collision2D collision) 
    //{
    //    if (collision.gameObject == target) 
    //    {
    //        isAttacked = false;
    //    }
    //}

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if (target == null) return;
        character_property tarCharP = target.GetComponent<character_property>();

        if (tarCharP.atkRange <= 0.5)
        {
            if (collider.gameObject == target)
            {
                isAttacked = true;
            }
        }

        else if (tarCharP.atkRange > 0.5)
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
