using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Image healthPointBar;
    public Image healthPointBarEffect;

    public float maxHP;
    private float currentHP;
    private float effectHP;

    //coroutine on/off
    private bool coroutineOperating;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = 100.0f;
        currentHP = maxHP;
        effectHP = maxHP;
        coroutineOperating = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentHP -= 10.0f * Time.deltaTime;
        
        healthPointBar.fillAmount = currentHP / maxHP;

        if (healthPointBarEffect.fillAmount > healthPointBar.fillAmount && coroutineOperating == false)
        {
            StartCoroutine(HpBarEffectMovement());
        }
    }
    IEnumerator HpBarEffectMovement() 
    {
        coroutineOperating = true;

        float delay = 1.0f;
        yield return new WaitForSeconds(delay);

        while (currentHP*1.1 < effectHP) 
        {
            effectHP -= 30.0f * Time.deltaTime;
            healthPointBarEffect.fillAmount = effectHP / maxHP;
        }

        coroutineOperating = false;
    }
  
}
