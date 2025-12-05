using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician_property : character_property
{
    void Awake()
    {
        name = "Magician";
        hp = 422;
        atk = 45;
        def = 19;
        satk = 25;
        sdef = 20;
        speed = 3.0f;
        //dex;  
        atkRange = 2.5f;
        //effectRange;
        skillPower = new int[] { 12, 16, 18, 60, 125 };
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
