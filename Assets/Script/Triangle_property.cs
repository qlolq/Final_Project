using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle_property : character_property
{
    void Awake()
    {
        name = "Triangle";
        hp = 600;
        atk = 0;
        def = 40;
        satk = 0;
        sdef = 40;
        speed = 3.0f;
        //dex;  
        atkRange = 0.0f;
        //effectRange;

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
