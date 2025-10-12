using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_logic : action
{
    private float speed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    Vector3 CalculateDistance() 
    {
        Vector3 fD = target.transform.position - this.transform.position; 
        return fD;
    }

    internal float CalculateMagnitude() { 
        return CalculateDistance().magnitude;
    }

    public void ChaseAutoPilot()
    {
        Vector3 movement = CalculateDistance();
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
    }

    //public void ChaseStopPilot() {
    //    animator.SetInt("Action", 0);
    //}

    void Update()
    {
        
    }
}
