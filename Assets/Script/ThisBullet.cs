using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThisBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ThisChar;
    protected Animator animator;

    private action charA;
    internal string EnemyTag;
    bool isAttacking = false;
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
        if(this.tag=="RedTeam")
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
        if(collider.CompareTag(EnemyTag))
        {
            BulletDestroy();
        }
    }
    public void BulletDestroy() 
    {
        animator.SetBool("Destroy", true);
        Destroy(this.gameObject,0.4f);
        isAttacking = true;
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }
    
}
