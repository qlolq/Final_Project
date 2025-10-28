using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public float timeDeliver;
    // Start is called before the first frame update
    void Start()
    {
        timeDeliver = 120;
    }

    // Update is called once per frame
    void Update()
    {
        GameDeliverTimer();
    }

    void GameDeliverTimer() 
    {
        timeDeliver -= Time.deltaTime;

        if (timeDeliver <= 0)
        {
            timeDeliver = 0;
            timer.text = "TimeOut";
            return;
        }

        timer.text = Mathf.FloorToInt(timeDeliver).ToString();
    }
}
