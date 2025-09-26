using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_logic : MonoBehaviour
{
    private float speed = 3.0f;
    private Animator animator;
    public GameObject target;

    // Bool to turn AutoPilot On/Off
    bool autoPilot = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsWalking", false);

    }

    Vector3 CalculateDistance() 
    {
        Vector3 fD = target.transform.position - this.transform.position; 
        return fD;
    }

    float CalculateMagnitude() { 
        return CalculateDistance().magnitude;
    }

    void AutoPilot()
    {
        Vector3 movement = CalculateDistance();
        movement.Normalize();
        this.transform.Translate(movement * speed * Time.deltaTime);
        animator.SetBool("IsWalking", true);

        if (CalculateMagnitude() <= 0.8f)
        {
            StopPilot();
        }
    }

    void StopPilot() {
        animator.SetBool("IsWalking", false);
        if (CalculateMagnitude() > 0.8f)
        {
            AutoPilot();
        }
    }

    void Update()
    {
        // Check if the T key has been pressed
        if (Input.GetKey(KeyCode.T))
        {
            autoPilot = !autoPilot;
            Debug.Log(target.transform.position - this.transform.position);
        }

        if (autoPilot)
        {
            AutoPilot();
        }
    }
}
