using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_property : MonoBehaviour
{
    char name; 
    int hp;
    int atk;
    int def;
    int satk; //special attack,通常指定為魔法攻擊    
    int sdef; //special defence,通常指定為魔法防禦
    int speed;
    int dex;  //dex是指攻擊速度（攻擊的頻率）
    int atkRange; //攻擊範圍（手長短）只for普通攻擊
    int effectRange; //傷害判定範圍（AOE？單體攻擊？）只for普通攻擊
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
