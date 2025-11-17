using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_property : MonoBehaviour
{
    internal string name;
    internal int hp;
    internal int _hp;
    internal int atk;
    internal int def;
    internal int satk; //special attack,通常指定為魔法攻擊    
    internal int sdef; //special defence,通常指定為魔法防禦
    internal float speed;
    internal float dex;  //dex是指攻擊速度（攻擊的頻率）
    internal float atkRange; //攻擊範圍（手長短）只for普通攻擊
    internal float effectRange; //傷害判定範圍（AOE？單體攻擊？）只for普通攻擊
                                //skilltime

    internal int indDamage;
    internal int indBurden;
    internal int indHeal;

    protected GameObject hp_indicator;

    // Start is called before the first frame update
    protected void Start()
    {
        indDamage = 0;
        indBurden = 0;
        indHeal = 0;

        _hp = hp;
        hpInstantiate();
    }

    // Update is called once per frame
    protected void Update()
    {

    }

    protected void hpInstantiate() 
    {
        Transform hpTran = this.transform.Find("hp_indicator");
        hp_indicator = hpTran.gameObject;

        float hpYPosition = this.transform.position.y - 0.75f;
        Vector3 hpPos = new Vector3(this.transform.position.x, hpYPosition, 50.0f);
        hp_indicator.transform.position = hpPos;
    }

    public int Damageable(int damage) 
    {
        _hp -= damage;
        return _hp;
    }

    public int IndicatorDamage(int damage) 
    {
        indDamage += damage;
        return indDamage;
    }
}

