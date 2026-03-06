using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
//using static action;

public class action : MonoBehaviour
{
    protected Animator animator;
    protected AnimatorStateInfo currentAniState;
    protected SpriteRenderer spriteRenderer;
    public SpriteRenderer bulletRenderer;
    protected GameObject target;
    protected character_property charP;

    public GameObject bulletStartPos;

    // property
    protected float speed;
    protected float attackRange;

    //animation state
    internal int animationState;
    int attackCount;

    //timer
    float DistTimer = 0f;
    float currentAniLength = 0f;
    float IsFireTimer = -1.2f;

    //determine
    internal bool isAttacking;
        
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

        // Debug.Log(curState);
        // Debug.Log(CalculateMagnitude());
        // Debug.Log(attackCount);
        // Debug.Log("isAttack " + attackCount);
        // Debug.Log("isAttacked " + isAttacked);
        // Debug.Log($"{curState}+{currentAniLength}+{attackCount}");
        // Debug.Log(this.attackRange);

    }

    protected void UpdateIdleState(GameObject target)
    {
        SpriteToFilp();

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

        SpriteToFilp();

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
        SpriteToFilp();

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
        SpriteToFilp();

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
        SpriteToFilp();

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
        Vector3 deadPos = new Vector3 (0f, 0f, 0f);
        if (this.CompareTag("BlueTeam"))
        {
            deadPos = new Vector3(300, 0, 0f);
        }

        else 
        {
            deadPos = new Vector3(-300.0f, 0, 0f);
        }
        this.transform.position = deadPos;
        curState = FSMState.Idle;
    }

    IEnumerator AniLengthDetector(int animationState)
    {
        ReceiveAnimatorTime();
        
        if (animator.GetInteger("Action") == animationState)
        {
            currentAniLength = currentAniState.length;
            if (animationState > 9 && animationState <= 12)
            {
                attackCount = animationState - 9;
            }

            else 
            {
                attackCount = 0;
            }
        }
        yield return null;
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

    public Vector3 CalculateDistance(GameObject target)
    {
        Vector3 fD = target.transform.position - this.transform.position;
        return fD;
    }

    internal float CalculateMagnitude()
    {
        return CalculateDistance(target).magnitude;
    }

    internal void SpriteToFilp()
    {
        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
            bulletRenderer.flipX = false;
            bulletStartPos.transform.position = new Vector3(this.transform.position.x+0.5f,this.transform.position.y+0.2f, this.transform.position.z);
        }

        else
        {
            spriteRenderer.flipX = false;
            bulletRenderer.flipX = true;
            bulletStartPos.transform.position = new Vector3(this.transform.position.x-0.5f,this.transform.position.y+0.2f, this.transform.position.z);

        }
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="nearestTarget"></param>

    public void ReceiveNearestTarget(GameObject nearestTarget) 
    {
        target = nearestTarget;
    }

    // void OnTriggerEnter2D(Collider2D collider) 
    // {
    //     // Debug.Log("Triggered");
    //     if (target == null) return;
    //     character_property tarCharP = target.GetComponent<character_property>();
    //     action tarAct = target.GetComponent<action>();

    //     if(tarCharP.atkRange <= 0.5f) 
    //     {
    //         if (collider.gameObject == target)
    //         {
    //             this.isAttacked = true;
    //         }
    //     }

    //     if (tarCharP.atkRange > 0.5f)
    //     {
    //                 // Debug.Log("Triggered,Bullet");
    //         if (collider.CompareTag("Bullet"))
    //         {
    //             this.isAttacked = true;
    //             Bullet bullet = collider.GetComponent<Bullet>();
    //             bullet.BulletDistroy();
    //         }
    //     }
    // }

    // void OnTriggerExit2D(Collider2D collider) 
    // {
    //     if (target == null) return;

    //     if (collider.gameObject == target || collider.CompareTag("Bullet")) 
    //     {
    //         isAttacked = false;
    //     }
    // }

    public bool IsAttacking()
    {
        if(animationState >= 10 && animationState <= 14)
        {
            DistTimer += Time.deltaTime;
            if(DistTimer >= currentAniLength+0.15f)
            {
                Debug.Log("Attacking");

                if(attackRange <= 0.5f)
                {
                    return isAttacking = true;
                }

                else if(attackRange>0.5f)
                {
                    IsFire();
                    return isAttacking = true;
                }

                else
                {
                    return isAttacking = false;
                }
            }
            else 
            {
                return isAttacking = false;
            }
        }
        
        else 
        {
            return isAttacking = false;
        }

    }

    public bool IsFire() 
    {
        IsFireTimer += Time.deltaTime;
        if(IsFireTimer >= currentAniLength+0.3f)
        {
            IsFireTimer = 0f;
            return true;
        }
        return false;
    }

    public GameObject returnTarget()
    {
        return target;
    }
}
