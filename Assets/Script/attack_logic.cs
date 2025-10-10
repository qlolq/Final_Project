using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_logic : action
{
    private move_logic moveLogic;

    void Start()
    {
        moveLogic = GetComponent<move_logic>();

        animator = GetComponent<Animator>();
        animator.SetBool("IsAttacking", false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void AutoPilot()
    {
        animator.SetBool("IsAttacking", true);

        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

    }

    void StopPilot()
    {
        animator.SetBool("IsAttacking", false);
    }

    void Update()
    {
        if (moveLogic.CalculateMagnitude() <= 0.8f)
        {
            AutoPilot();
        }

        else if (moveLogic.CalculateMagnitude() > 0.8f)
        {
            StopPilot();
        }
    }
}
