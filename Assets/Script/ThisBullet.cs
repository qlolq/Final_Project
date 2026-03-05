using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThisBullet : MonoBehaviour
{
    // Start is called before the first frame update
    internal String EnemyTag;
    void Start()
    {
        EnemyTagDefined();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void EnemyTagDefined()
    {
        if(this.tag=="RedTeam")
        {
            EnemyTag = "BlueTeam";
        }

        else
        {
            EnemyTag = "RedTeam";
        }

    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if(collider.gameObject.tag == EnemyTag)
        {
            IsAttacking();
            BulletDestroy();
        }
    }
    public void BulletDestroy() 
    {
        Debug.Log("Bullet Destroyed");
        Destroy(this.gameObject,0.5f);
    }

    public bool IsAttacking()
    {
        return true;
    }
}
