using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    
    //bullet
    public GameObject startPos;
    public GameObject bulletPrefab;
    protected GameObject bullet;
    public GameObject TheChar;
    private action charA;
    private character_property charP;
    private bool isFire = false;


    // Start is called before the first frame update
    void Start()
    {
        charA = TheChar.GetComponent<action>();
        charP = TheChar.GetComponent<character_property>();
    }   

    // Update is called once per frame
    void Update()
    {
        isFire = charA.IsFire();
        
        if(isFire)
        {
            BulletInstantiate();
        }
    }

    public void BulletInstantiate() 
    {
        bullet = Instantiate(bulletPrefab, startPos.transform.position, Quaternion.identity);

        BulletMove();

        GameObject target = charA.returnTarget();

        if(target.tag=="RedTeam")
        {
            bullet.tag = "BlueTeamBullet";
        }

        else if(target.tag=="BlueTeam")
        {
            bullet.tag = "RedTeamBullet";
        }
        else
        {
            bullet.tag = "";
        }
    }

    public void BulletMove()
    {
        GameObject target = charA.returnTarget();
        Vector2 dist = target.transform.position - startPos.transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = dist.normalized * 2.0f * charP.dex;
    }
}
