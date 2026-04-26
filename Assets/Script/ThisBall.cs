using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThisBall : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ThisChar; 
    protected Animator animator;

    internal action charA;
    internal string EnemyTag;
    internal bool isRangeAttacking = false;

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
        if(this.tag=="RedTeamBall")
        {
            EnemyTag = "BlueTeam";
        }

        else if(this.tag=="BlueTeamBall")
        {
            EnemyTag = "RedTeam";
        }

        else
        {
            EnemyTag = "";
        }

    }

    void OnTriggerEnter2D(Collider2D collider) 
    {

        if(collider.CompareTag(EnemyTag))
        {
            GameObject hitTarget = collider.gameObject;

            // Debug.Log("Hit");
            charA.isSkillAttacking = true;
            charA.target = hitTarget;
            //Debug.Log(hitTarget.name);

            charA.IsSkillAttacking(charA.target);
        }

        if(collider.tag==null)
        {
            Debug.Log("no target");
        }
        BulletDestroy();

    }
    public void BulletDestroy() 
    {
        animator.SetBool("Destroy", true);
        Destroy(this.gameObject,1.0f);
    }
    
}
