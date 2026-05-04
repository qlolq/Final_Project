using System;
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
    internal float mp;
    internal float _mp = 0f;
    internal float cooldown;

    internal int []skillPower;

    internal int indDamage;
    internal int indBurden;
    internal int indHeal;

    internal int killCount;
    internal int assistCount;
    internal int deadCount;
    internal bool isDead = false;

    protected GameObject hp_indicator;
    protected GameObject hp_Full;
    protected GameObject hp_Effect;
    protected GameObject mp_Full;
    private bool coroutineOperating;

    internal bool isReady = false;
    internal List<GameObject> DamageList = new List<GameObject>();

    protected void Awake() 
    {
        indDamage = 0;
        indBurden = 0;
        indHeal = 0;
        killCount = 0;
        assistCount = 0;
        deadCount = 0;

        _hp = hp;
        coroutineOperating = false;
        hpInstantiate();
    }

    // Start is called before the first frame update
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
    }

    protected void hpInstantiate() 
    {
        Transform hpTran = this.transform.Find("hp_indicator");
        hp_indicator = hpTran.gameObject;

        Transform hpTranF = hp_indicator.transform.Find("hp_Full");
        hp_Full = hpTranF.gameObject;

        Transform hpTranE = hp_indicator.transform.Find("hp_Effect");
        hp_Effect = hpTranE.gameObject;

        float hpYPosition = this.transform.position.y - 0.75f;
        Vector3 hpPos = new Vector3(this.transform.position.x, hpYPosition, 50.0f);
        hp_indicator.transform.position = hpPos;

        Transform mpTran = hp_indicator.transform.Find("mp_Effect");
        Transform mp_empty = mpTran.transform.Find("mpBarFIll_0");
        mp_Full = mp_empty.gameObject;

        float mpYPosition = this.transform.position.y - 1.09f;
        float mpXPosition = this.transform.position.x - 0.6f;
        Vector3 mpPos = new Vector3(mpXPosition, mpYPosition, 50.0f);
        mp_Full.transform.position = mpPos;
    }
    public int Damageable(int damage) 
    {
        if(_hp<=0)
        {
            return 0;
        }

        if (_hp - damage <= 0)
        {
            _hp = 0;
            isDead = true;
        }

        else 
        {
            _hp -= damage;
        }

        HealthBarEffect(damage);
        //DamageFeedback();
        return _hp;
    }

    public int IndicatorDamage(int damage) 
    {
        indDamage += damage;
        return indDamage;
    }

    public int IndicatorBurden(int damage)
    {
        indBurden += damage;
        return indBurden;
    }

    public int IndicatorKill()
    {
        return killCount;
    }

    public int IndicatorAssist()
    {
        return assistCount;
    }

    public int IndicatorDead()
    {
        return deadCount;
    }

    void HealthBarEffect(int damage) 
    {
        float currentHpRatio = (float)_hp / hp;
        //Debug.Log($"{_hp} + {currentHpRatio:F2}");

        hp_Full.transform.localScale = new Vector3(currentHpRatio, 1f, 1f);

        _mp += damage/5;
        //Debug.Log(_mp);        
        float currentMpRatio = (float)_mp / mp;

        mp_Full.transform.localScale = new Vector3(currentMpRatio, 1f, 1f);

        if(_mp>=mp)
        {
            _mp = mp;
            isReady = true;
        }

        if (hp_Effect.transform.localScale.x > hp_Full.transform.localScale.x && coroutineOperating == false)
        {
            StartCoroutine(HpBarEffectMovement(hp_Full.transform.localScale, hp_Effect.transform.localScale));
        }


    }

    IEnumerator HpBarEffectMovement(Vector3 targetScale, Vector3 startScale)
    {
        coroutineOperating = true;

        float delay = 0.5f;
        yield return new WaitForSeconds(delay);

        hp_Effect.transform.localScale = Vector3.Lerp(startScale, targetScale, 0.01f * Time.deltaTime);
        coroutineOperating = false;
        yield return null;

        hp_Effect.transform.localScale = targetScale;
    }

    public void HPreset()
    {
        float fullHPRatio = (float)hp / hp;
        hp_Full.transform.localScale = new Vector3(fullHPRatio, 1f, 1f);
        hp_Effect.transform.localScale = new Vector3(fullHPRatio, 1f, 1f);
    }
}

