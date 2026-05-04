using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_property : character_property
{
    void Awake() 
    {
        name = "Sniper";
        hp = 402;
        mp = 120;
        atk = 66;
        def = 18;
        satk = 0;
        sdef = 12;
        speed = 1.8f;
        dex = 1.8f;  
        atkRange = 4.0f;
        //effectRange;
        cooldown = 15;
        skillPower = new int[] { 16, 19, 24, 55, 30 };
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
