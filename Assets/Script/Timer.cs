using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public TextMeshProUGUI whoWin;
    public teamManager BlueTeam;
    public teamManager RedTeam;

    public float timeDeliver;
    // Start is called before the first frame update
    void Start()
    {
        int[] blueStrategy = SettingManager.ReturnBlueStrategyIndex();

        int blueChoice = blueStrategy[2]; 
        switch(blueChoice)
        {
            case 0:
                timeDeliver = 60;
                break;
            case 1:
                timeDeliver = 90;
                break;
            case 2:
                timeDeliver = 120;
                break;
        }
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
            if(BlueTeam.teamScore < RedTeam.teamScore)
            {
                whoWin.text = "Red Team Win!";
                whoWin.color =Color.red;
            }

            else if (BlueTeam.teamScore > RedTeam.teamScore)
            {
                whoWin.text = "Blue Team Win!";
                whoWin.color =Color.blue;
            }

            else
            {
                whoWin.text = "Draw!";
            }

            timeDeliver = 0;
            timer.text = "TimeOut";
            return;
        }

        timer.text = Mathf.FloorToInt(timeDeliver).ToString();
    }
}
