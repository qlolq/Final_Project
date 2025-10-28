using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class teamManager : MonoBehaviour {
    public int teamNum;
    public GameObject[] RedTeam;
    public GameObject[] BlueTeam;
    protected GameObject[] targets;

    // Start is called before the first frame update
    void Start()
    {
        teamNum = 3;

        RedTeam = new GameObject[teamNum];
        BlueTeam = new GameObject[teamNum];
        targets = new GameObject[teamNum];

        WhoIsEnemy();
        EnemyList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void WhoIsEnemy()
    {
        for (int i=0; i<teamNum;i++) 
        {
            RedTeam[i].tag = "RedTeam";
            BlueTeam[i].tag = "BlueTeam";
        }
    }

    void EnemyList() 
    {
        for (int i = 0; i < teamNum; i++)
        {
            if (gameObject.CompareTag("RedTeam"))
            {
                targets[i] = BlueTeam[i];
            }

            else if (gameObject.CompareTag("BlueTeam"))
            {
                targets[i] = RedTeam[i];
            }
        }
    }
}
