using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman_property : character_property
{
    void Awake() 
    {
        name = "Swordsman";
        hp = 456;
        mp = 200;
        atk = 76;
        def = 20;
        satk = 0;
        sdef = 15;
        speed = 2.5f;
        dex = 1.0f;  
        atkRange = 0.4f;
        //effectRange;
        cooldown = 15;
        skillPower = new int[] { 15, 18, 22, 35, 40 };
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
