using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman_property : character_property
{
   
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        name = "Swordsman";
        hp = 456;
        atk = 55;
        def = 20;
        satk = 0;
        sdef = 15;
        speed = 3.0f;
        //dex;  
        atkRange = 0.8f;
        //effectRange;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
