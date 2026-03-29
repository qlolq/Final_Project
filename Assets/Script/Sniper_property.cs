using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_property : character_property
{
    void Awake() 
    {
        name = "Sniper";
        hp = 402;
        atk = 66;
        def = 15;
        satk = 0;
        sdef = 12;
        speed = 2.5f;
        //dex;  
        atkRange = 2.5f;
        //effectRange;
        cooldown = 20;
        skillPower = new int[] { 16, 19, 24, 55, 125 };
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
