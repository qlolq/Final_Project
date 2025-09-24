using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_logic : MonoBehaviour
{
    private float speed = 3.0f;
    public GameObject target;

    // Bool to turn AutoPilot On/Off
    bool autoPilot = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 CalculateDistance() 
    {
        Vector3 fD = target.transform.position - this.transform.position; //我方本體與目標距離
        return fD;
    }

    float CalculateMagnitude() { 
        return CalculateDistance().magnitude;
    }

    void AutoPilot()    //自動波function
    {
        //CalculateAngle();
        Vector3 movement = CalculateDistance();
        movement.Normalize();
        this.transform.Translate(movement * speed * Time.deltaTime);
    }

    void Update()
    {
        // Check if the T key has been pressed
        if (Input.GetKey(KeyCode.T))
        {
            autoPilot = !autoPilot;
        }

        if (autoPilot) {
            if (CalculateMagnitude() > 0.8f)
            {
                AutoPilot();
            }
        }


    }
}
