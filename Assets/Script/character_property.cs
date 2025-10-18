using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_property : MonoBehaviour
{
    protected string name;
    protected int hp;
    protected int atk;
    protected int def;
    protected int satk; //special attack,通常指定為魔法攻擊    
    protected int sdef; //special defence,通常指定為魔法防禦
    protected float speed;
    protected float dex;  //dex是指攻擊速度（攻擊的頻率）
    protected float atkRange; //攻擊範圍（手長短）只for普通攻擊
    protected float effectRange; //傷害判定範圍（AOE？單體攻擊？）只for普通攻擊
    //skilltime
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
