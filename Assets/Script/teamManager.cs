using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teamManager : GameManager

{
    public CharInstantiate charInstantiate;

    public int teamNum = 1;
    internal GameObject[] RedTeam;
    internal GameObject[] BlueTeam;
    //internal GameObject[] targets;

    public GameObject character;

    // Start is called before the first frame update
    void Start()
    {
        //charInstantiate = GetComponent<CharInstantiate>();

        RedTeam = new GameObject[teamNum];
        BlueTeam = new GameObject[teamNum];

        charInstantiate.BlueTeamSpawnLocation();
        charInstantiate.RedTeamSpawnLocation();

        CharacterInstantiate();
       
        WhoIsEnemy();
        //EnemyList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CharacterInstantiate()
    {
        for (int i = 0; i < teamNum; i++)
        {
            Vector3 bluePos = charInstantiate.GetBluePos(i);
            Vector3 redPos = charInstantiate.GetRedPos(i);

            BlueTeam[i] = Instantiate(character, bluePos, Quaternion.identity);
            RedTeam[i] = Instantiate(character, redPos, Quaternion.identity);
        }
    }

    void WhoIsEnemy()
    {
        for (int i=0; i<teamNum;i++) 
        {
            RedTeam[i].tag = "RedTeam";
            BlueTeam[i].tag = "BlueTeam";
        }
    }

    public GameObject GetBlueTeam(int i) 
    {
        return BlueTeam[i];
    }

    public GameObject GetRedTeam(int i)
    {
        return RedTeam[i];
    }
}
