using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_logic : action
{
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AutoPilot()
    {
        if (this.transform.position.x < targets.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

    }

    void Update()
    {
 
    }
}
