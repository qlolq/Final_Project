using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwordBall : MonoBehaviour
{
    
    //bullet
    public GameObject startPos;
    public GameObject bulletPrefab;
    protected GameObject sword;
    protected GameObject sword2;
        
    public GameObject TheChar;
    private action charA;
    private character_property charP;
    private bool isSword = false;


    // Start is called before the first frame update
    void Start()
    {
        charA = TheChar.GetComponent<action>();
        charP = TheChar.GetComponent<character_property>();
    }   

    // Update is called once per frame
    void Update()
    {
        isSword = charA.IsSword();
        
        if(isSword)
        {
            BulletInstantiate();
        }
    }

    public void BulletInstantiate() 
    {
        sword = Instantiate(bulletPrefab, startPos.transform.position, Quaternion.identity);
        sword2 = Instantiate(bulletPrefab, startPos.transform.position, Quaternion.identity);


        SwordMove();

        GameObject target = charA.returnTarget();

        if(target.tag=="RedTeam")
        {
            sword.tag = "BlueTeamBullet";
            sword2.tag = "BlueTeamBullet";
        }

        else if(target.tag=="BlueTeam")
        {
            sword.tag = "RedTeamBullet";
            sword2.tag = "RedTeamBullet";
        }
        else
        {
            sword.tag = "";
            sword2.tag = "";
        }
    }

    public void SwordMove()
    {
        GameObject target = charA.returnTarget();
        Vector2 dist = target.transform.position - startPos.transform.position;
        sword.GetComponent<Rigidbody2D>().velocity = dist.normalized * 3.0f * charP.dex;
        sword2.GetComponent<Rigidbody2D>().velocity = -dist.normalized * 3.0f * charP.dex;
    }
}
