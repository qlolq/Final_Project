using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman_property : character_property
{
    void Awake() 
    {
        name = "Swordsman";
        hp = 456;
        atk = 55;
        def = 20;
        satk = 0;
        sdef = 15;
        speed = 2.5f;
        //dex;  
        atkRange = 0.4f;
        //effectRange;
        skillPower = new int[] { 15, 18, 22, 60, 125 };
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
