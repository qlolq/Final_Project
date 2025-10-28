using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_property : GameManager
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
    public Canvas gameCanvas;
    protected Camera mainCamera;
    public GameObject hpBarInstance;
    protected GameObject HP_show;
    protected Vector3 offset = new Vector3(0, -0.7f, 0);

    protected GameObject hpBar;
    protected GameObject hpBarEffect;

    // Start is called before the first frame update
    protected void Start()
    {
        mainCamera = Camera.main;
        HP_show = Instantiate(hpBarInstance, gameCanvas.transform);

    }

    // Update is called once per frame
    protected void Update()
    {
        UpdateHPbarPos();
    }

    void UpdateHPbarPos()
    {
        if (mainCamera == null) return;

        Vector3 targetWorldPos = transform.position + offset;
        Vector2 screenPos = mainCamera.WorldToScreenPoint(targetWorldPos);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gameCanvas.GetComponent<RectTransform>(),
            screenPos,
            gameCanvas.worldCamera,
            out Vector2 uiPos))
        {
            HP_show.GetComponent<RectTransform>().localPosition = uiPos;
            //Debug.Log("尝试实例化血条：" + (HP_show != null ? "成功" : "失败"));
        }

    }
}
