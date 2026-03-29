using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.MPE;
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
    private teamManager myTM;

    public GameObject bulletStartPos;
    public GameObject bullet;

    // property
    protected float speed;
    protected float attackRange;

    protected int cooldown;

    protected string name;

    //animation state
    internal int animationState;
    int attackCount;

    //timer
    float DistTimer = 0f;
    float currentAniLength = 0f;
    float IsFireTimer = -1.2f;
    float skilltimer = 7.0f;
    float attackTimer = 0f;

    float deadTimer = 0f;

    //determine
    internal bool isAttacking;

    internal bool isAttacked;

    internal bool isDead = false;
        
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
        Attacked,  //5
        Skill,  //6
        ExtraSkill, //7
        Reborn,  //8

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
        cooldown = charP.cooldown;
        name = charP.name;

        isAttacking = false;
        isAttacked = false;

        Collider2D collider = GetComponent<Collider2D>();

        teamManager[] allManagers = FindObjectsOfType<teamManager>();

        foreach(var manager in allManagers)
        {
            if(manager.gameObject.CompareTag(this.tag))
            {
                myTM = manager;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {    
        if(isDead)
        {
            isDead = false;
            SpriteToFilp();
            DistTimer += Time.deltaTime;

            if (DistTimer >= currentAniLength)
            {
                animationState = 30;
                animator.SetInteger("Action", animationState);
                curState = FSMState.Dead;

                DistTimer = 0f;

                StartCoroutine(AniLengthDetector(animationState));            
            } 
        }

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
            case FSMState.Dead: UpdateDeadState(); break;
            case FSMState.Attacked: UpdateAttackedState(); break;
            case FSMState.Skill:UpdateSkillState(); break;
            case FSMState.ExtraSkill: UpdateExtraSkillState();break;
            case FSMState.Reborn: UpdateRebornState();break;
        }
        
        if (charP._hp <=0 && curState != FSMState.Dead) 
        {
            isDead = true;
            return;
        }

        if(isAttacked && charP._hp >0)
        {
            SpriteToFilp();

            DistTimer += Time.deltaTime;

            if (DistTimer >= currentAniLength+0.3f)
            {

                animationState = 20;
                animator.SetInteger("Action", animationState);
                curState = FSMState.Attacked;
                
                DistTimer = 0f;

                StartCoroutine(AniLengthDetector(animationState));
            }


        }

        Debug.Log(curState);
        // Debug.Log(CalculateMagnitude());
        // Debug.Log(attackCount);
        // Debug.Log("isAttack " + attackCount);
        // Debug.Log("isAttacked " + isAttacked);
        // Debug.Log($"{curState}+{currentAniLength}+{attackCount}");
        // Debug.Log(this.attackRange);
        //Debug.Log(name);

        if(skilltimer < cooldown)
        {
            skilltimer += Time.deltaTime;
        }

        else if(skilltimer >= cooldown)
        {
            skilltimer = cooldown;
        }
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

            else if (CalculateMagnitude() <= attackRange && skilltimer < cooldown)    // attack to target if arrive
            {
                animationState = 10;
                animator.SetInteger("Action", animationState);

                curState = FSMState.stAttack;

                DistTimer = 0f;
                StartCoroutine(AniLengthDetector(animationState));
            }

            if(skilltimer >= cooldown)
            {
                if(name.Equals("Swordsman"))
                {
                    StartCoroutine(DashToTarget());
                }

                animationState = 13;
                animator.SetInteger("Action", animationState);
                curState = FSMState.Skill;

                DistTimer = 0f;
                StartCoroutine(AniLengthDetector(animationState));

                skilltimer = 0f;
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
        deadTimer+= Time.deltaTime;
        
        if(deadTimer >= 10.0f)
        {
            deadTimer = 0f;
            
            animationState = 32;
            animator.SetInteger("Action", animationState);
            curState = FSMState.Reborn;
        }

        else if(deadTimer >= 3.0f)
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

            animationState = 31;
            animator.SetInteger("Action", animationState);
            curState = FSMState.Dead;
        }
    }
    protected void UpdateRebornState()
    {
        SpriteToFilp();

        DistTimer += Time.deltaTime;
        charP._hp = charP.hp;
        charP.HPreset();
        
        if (DistTimer >= currentAniLength + 0.3f)
        {
            if(this.tag=="BlueTeam")
            {
                float BlueminX = -5.3f;
                float BluemaxX = -1.0f;
                float BlueminY = -2.5f;
                float BluemaxY = 3.0f;

                float x = UnityEngine.Random.Range(BlueminX, BluemaxX);
                float y = UnityEngine.Random.Range(BlueminY, BluemaxY);
                this.transform.position = new Vector3(x, y, 0);
                animationState = 0;
                animator.SetInteger("Action", animationState);
                curState = FSMState.Idle;
            }

            else
            {
                float RedminX = 1.0f;
                float RedmaxX = 5.3f;
                float RedminY = -2.5f;
                float RedmaxY = 3.0f;

                float x = UnityEngine.Random.Range(RedminX, RedmaxX);
                float y = UnityEngine.Random.Range(RedminY, RedmaxY);
                this.transform.position = new Vector3(x, y, 0);
                animationState = 0;
                animator.SetInteger("Action", animationState);
                curState = FSMState.Idle;
            }
        }
    }

    protected void UpdateAttackedState()
    {
        SpriteToFilp();

        DistTimer += Time.deltaTime;

        if (DistTimer >= currentAniLength + 0.3f)
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

            isAttacked = false;
            
            if(spriteRenderer.flipX)
            {
                this.transform.position = new Vector3(this.transform.position.x - 0.2f, this.transform.position.y, this.transform.position.z);
            }

            else
            {
                this.transform.position = new Vector3(this.transform.position.x + 0.2f, this.transform.position.y, this.transform.position.z);
            }
        }
    }

    protected void UpdateSkillState()
    {
        SpriteToFilp();

        DistTimer += Time.deltaTime;

        if (DistTimer >= currentAniLength + 0.3f)
        {
            animationState = 0;
            animator.SetInteger("Action", animationState);
            curState = FSMState.Idle;
            DistTimer = 0f;
            StartCoroutine(AniLengthDetector(animationState));
        }
    }

    protected void UpdateExtraSkillState()
    {
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

    IEnumerator DashToTarget()
    {
        float dashtime = 1.2f;
        float elapsedTime = 0f;

        Vector3 startPos = this.transform.position;
        Vector3 endPos = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);

        if (this.transform.position.x <= target.transform.position.x)
        {
            endPos = new Vector3(this.transform.position.x + 3.0f,this.transform.position.y,this.transform.position.z);
        }

        else
        {
            endPos = new Vector3(this.transform.position.x - 3.0f,this.transform.position.y,this.transform.position.z);
        }

        PathDamage(startPos, endPos);

        while (elapsedTime < dashtime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / dashtime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.transform.position = endPos;
    }

    void PathDamage(Vector3 startPos, Vector3 endPos)
    {
        RaycastHit2D[] hitObjects = Physics2D.LinecastAll(startPos, endPos);
        foreach (RaycastHit2D hit in hitObjects)
        {
            GameObject hitEnemy = hit.collider.gameObject;

            if (hitEnemy == gameObject||hitEnemy.CompareTag(tag))
            {
                isAttacking = true;
                myTM.HealthInitial(0,this.gameObject, target);
            }
        }

        isAttacking = false;
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
        if(animationState >= 10 && animationState <= 12)
        {
            DistTimer += Time.deltaTime;
            if(DistTimer >= currentAniLength+0.15f)
            {
                if(attackRange <= 0.5f)
                {
                    return isAttacking = true;
                }

                else if(attackRange>0.5f)
                {
                    IsFire();
                    attackTimer += Time.deltaTime;
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
