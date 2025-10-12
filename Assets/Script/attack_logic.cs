using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_logic : action
{
    //private move_logic moveLogic;

    void Start()
    {
        //moveLogic = GetComponent<move_logic>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AutoPilot()
    {
        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

    }

    //public void StopPilot()
    //{
    //    animator.SetBool("IsAttacking", false);
    //}

    void Update()
    {
 
    }
}
