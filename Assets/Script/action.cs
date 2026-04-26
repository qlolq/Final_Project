using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
    internal GameObject target;
    internal character_property charP;
    private teamManager myTM;

    public GameObject bulletStartPos;
    public GameObject bullet;

    // property
    protected float speed;
    protected float attackRange;

    protected float cooldown;

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
    internal bool hasDealDamage = false;
    
    internal bool hasSkillDamage = false;

    internal bool isRangeAttacking = false;
    internal bool isSkillAttacking = false;

    internal bool isDead = false;
    internal Rigidbody2D rb;
    internal Collider2D hitCollider;

    internal bool isDash = false; //swordman skill

    internal static string teamTag;

    internal int currentSkillIndex = -1;

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

        teamManager[] allManagers = FindObjectsOfType<teamManager>();

        foreach(var manager in allManagers)
        {
            if(manager.gameObject.CompareTag(this.tag))
            {
                // Debug.Log(this.gameObject.tag);
                myTM = manager;
                break;
            }
        }

        teamTag= this.tag;

        rb = GetComponent<Rigidbody2D>();
        hitCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {    
        if(animationState<30)
        {
            if(this.transform.position.x>5.8 && isDead == false)
            {
                this.transform.position = new Vector3(5.7f, this.transform.position.y, this.transform.position.z);
            }

            if(this.transform.position.x<-5.8 && isDead == false)
            {
                this.transform.position = new Vector3(-5.7f, this.transform.position.y, this.transform.position.z);
            }
            
            if(this.transform.position.y>2.85 && isDead == false)
            {
                this.transform.position = new Vector3(this.transform.position.x, 2.75f, this.transform.position.z);
            }

            if(this.transform.position.y<-2.85 && isDead == false)
            {
                this.transform.position = new Vector3(this.transform.position.x, -2.75f, this.transform.position.z);
            }
        }

        if(animationState<30 && CalculateMagnitude() > 100.0f)
        {
            animationState = 0;
            animator.SetInteger("Action", animationState);
            curState = FSMState.Idle;        
        }

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

        //Debug.Log(curState);
        // Debug.Log(CalculateMagnitude());
        // Debug.Log(attackCount);
        // Debug.Log("isAttack " + attackCount);
        // Debug.Log("isAttacked " + isAttacked);
        // Debug.Log($"{curState}+{currentAniLength}+{attackCount}");
        // Debug.Log(this.attackRange);
        //Debug.Log(name);
        //Debug.Log(animationState);
        //Debug.Log(this.gameObject);

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

            else if(CalculateMagnitude() <= attackRange && skilltimer >= cooldown)
            {
                if(name.Equals("Swordsman"))
                {
                    isDash = true;
                    StartCoroutine(DashToTarget());
                }

                else if(name.Equals("Magician"))
                {
                    IsSkillFire();
                }

                animationState = 13;
                animator.SetInteger("Action", animationState);
                curState = FSMState.Skill;
                hasSkillDamage = false;
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
        IsAttacking();
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
                hasDealDamage = false;
                //triggerAttackOn = false;
                DistTimer =0f;
                attackCount = 0;
                StartCoroutine(AniLengthDetector(animationState));
            }
        }
    }

    protected void UpdateNdAttackState(GameObject target)
    {
        IsAttacking();
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
                hasDealDamage = false;  

                //triggerAttackOn = false;
                DistTimer = 0f;
                StartCoroutine(AniLengthDetector(animationState));
            }
        }
    }

    protected void UpdateRdAttackState(GameObject target)
    {
        IsAttacking();
        SpriteToFilp();

        DistTimer += Time.deltaTime;

        if (DistTimer >= currentAniLength + 0.3f) 
        {
            if (attackCount > 2)
            {
                animationState = 0;
                animator.SetInteger("Action", animationState);

                curState = FSMState.Idle;
                hasDealDamage = false;
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
            StartCoroutine(AniLengthDetector(animationState));

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
            StartCoroutine(AniLengthDetector(animationState));
        }
    }
    protected void UpdateRebornState()
    {
        SpriteToFilp();

        DistTimer += Time.deltaTime;
        charP._hp = charP.hp;
        charP.HPreset();
        isAttacked = false;
        isAttacking = false;
        charP.cooldown = 0.0f;
        
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
                StartCoroutine(AniLengthDetector(animationState));

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
                StartCoroutine(AniLengthDetector(animationState));
            }

            DistTimer = 0f;
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
/// <summary>
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>
/// <param name="animationState"></param>
/// <returns></returns>
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
        float dashtime = 1.0f;
        float elapsedTime = 0f;

        Vector3 startPos = this.transform.position;
        Vector3 dir = (target.transform.position - transform.position).normalized;
        Vector3 endPos = startPos + dir * 3.0f;

        while (elapsedTime < dashtime)
        {
            rb.MovePosition(Vector3.Lerp(startPos, endPos, elapsedTime / dashtime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(endPos);
        isDash = false;
    }

    void OnTriggerEnter2D(Collider2D hitCollider)
    {
        GameObject hitTarget = hitCollider.gameObject;
        if(isDash)
        {
            if(hitTarget.CompareTag(target.tag))
            {
                target = hitTarget;
                myTM.HealthInitial(this.gameObject, target); 
                Debug.LogError("Dash Attack!");
            }
            isSkillAttacking=false;
        }
        hitTarget=null;
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

    public void IsAttacking()
    {
        if(animationState >= 10 && animationState <= 12)
        {
            if(!hasDealDamage)
            {
                if(attackRange <= 0.5f)
                {
                    isAttacking = true;
                    if(isAttacking)
                    {
                        myTM.HealthInitial(this.gameObject, target);
                        isAttacking = false;
                    }
                }

                else if(attackRange>0.5f)
                {
                    isAttacking = true;
                    if(isAttacking)
                    {
                        IsFire();
                        myTM.HealthInitial(this.gameObject, target);
                        isAttacking = false;
                    }
                }
                
                hasDealDamage = true;

            }
        }
    }

    public void IsSkillAttacking(GameObject hitTarget)
    {
        GameObject target = hitTarget;

        if(isSkillAttacking)
        {
            //Debug.Log(isSkillAttacking);
            if (hitTarget != null)
            {
                teamManager[] allManagers = FindObjectsOfType<teamManager>();

                foreach(var manager in allManagers)
                {
                    if(manager.gameObject.CompareTag(teamTag))
                    {
                        // Debug.Log(this.gameObject.tag);
                        myTM = manager;
                        break;
                    }
                }
                // Debug.Log(myTM);

                if(myTM!=null)
                {
                    //Debug.Log(animationState);
                    myTM.HealthInitial(this.gameObject, hitTarget);
                }
            }
        }
        //isSkillAttacking = false;
    }

    /// <summary>
    /// ////////////////////////bullet and ball/////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>
    public bool IsFire() 
    {
        // Debug.Log("IsFire");
        IsFireTimer += Time.deltaTime;
        if(IsFireTimer >= currentAniLength+0.1f && CalculateMagnitude() < 50.0f && !hasDealDamage)
        {
            IsFireTimer = 0f;
            return true;
        }
        return false;
    }

    public bool IsSkillFire() 
    {   
        //Debug.Log("IsSkillFire");     
        IsFireTimer += Time.deltaTime;
        if(IsFireTimer >= currentAniLength+0.1f && CalculateMagnitude() < 50.0f && !hasSkillDamage)
        {
            hasSkillDamage = true;
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
