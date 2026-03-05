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
    public action thisA;
    private bool isFire = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }   

    // Update is called once per frame
    void Update()
    {

        isFire = thisA.IsFire();
        
        if(isFire)
        {
            BulletInstantiate();
        }


    }

    public void BulletInstantiate() 
    {
        bullet = Instantiate(bulletPrefab, startPos.transform.position, Quaternion.identity);
        BulletMove();

        GameObject target = thisA.returnTarget();
        if(target.tag=="RedTeam")
        {
            bullet.tag = "BlueTeam";
        }

        else
        {
            bullet.tag = "RedTeam";
        }
    }

    public void BulletMove()
    {
        GameObject target = thisA.returnTarget();
        Vector2 dist = target.transform.position - startPos.transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = dist.normalized * 2.0f;
    }
}
