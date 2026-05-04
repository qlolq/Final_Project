using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThisSword : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ThisChar;
    protected Animator animator;

    internal action charA;
    internal string EnemyTag;
    internal bool isRangeAttacking = false;

    internal bool hasCollider = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        charA = ThisChar.GetComponent<action>();

        EnemyTagDefined();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnemyTagDefined()
    {
        if(this.tag=="RedTeamBullet")
        {
            EnemyTag = "BlueTeam";
        }

        else
        {
            EnemyTag = "RedTeam";
        }

    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if(hasCollider) 
        {
            return;
        }

        if(collider.CompareTag(EnemyTag))
        {
            hasCollider = true;
            GameObject hitTarget = collider.gameObject;
            charA.isExtraSkillAttacking = true;
            charA.target = hitTarget;
            BulletDestroy();
        }
    }
    public void BulletDestroy() 
    {
        animator.SetBool("Destroy", true);
        Destroy(this.gameObject,0.4f);
    }
}