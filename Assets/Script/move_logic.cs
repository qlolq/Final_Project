using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_logic : action
{
    private float speed = 3.0f;

    // Bool to turn AutoPilot On/Off
    bool autoPilot = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsWalking", false);
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

    void AutoPilot()
    {
        Vector3 movement = CalculateDistance();
        movement.Normalize();
        this.transform.Translate(movement * speed * Time.deltaTime);
        animator.SetBool("IsWalking", true);

        if (this.transform.position.x < target.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void StopPilot() {
        animator.SetBool("IsWalking", false);
    }

    void Update()
    {
        // Check if the T key has been pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            autoPilot = !autoPilot;
        }

        if (autoPilot == true)
        {

            if (CalculateMagnitude() > 0.8f)
            {
                AutoPilot();
            }

            else if (CalculateMagnitude() <= 0.8f)
            {
                StopPilot();
            }
        }

        else if (autoPilot == false) {
            StopPilot();
        }
    }
}
