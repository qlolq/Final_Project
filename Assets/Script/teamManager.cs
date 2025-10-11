using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class teamManager : MonoBehaviour {
    public int teamNum =1;
    ArrayList getSelect = new ArrayList();
    public GameObject target;
    GameObject[] RedTeam;
    GameObject[] BlueTeam;


    // Start is called before the first frame update
    void Start()
    {
        int selectCount = teamNum * 2 - 1;

        int teamSize = teamNum - 1;
        RedTeam = new GameObject [teamSize];
        BlueTeam = new GameObject [teamSize];

        WhichTeam();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void WhichTeam()
    {
        int j = 0;
        int k = 0;
        string isRedTeam = "RedTeam";
        string isBlueTeam = "BlueTeam";

        if (teamNum>0) {
            foreach (object selectCount in getSelect)
            {
                getSelect.Add(target);

                string RedOrBlue = target.tag;

                if (RedOrBlue == isRedTeam)
                {
                    if (j < RedTeam.Length)
                    {
                        RedTeam[j] = target;
                        j++;
                    }

                    else
                    {
                        Debug.Log("Red Team is full!");
                    }

                }

                else if (RedOrBlue == isBlueTeam)
                {
                    if (k < BlueTeam.Length)
                    {
                        BlueTeam[k] = target;
                        k++;
                    }

                    else
                    {
                        Debug.Log("Blue Team is full!");
                    }
                }

                else
                {
                    Debug.Log("The Character has not tag!!!");
                    return;
                }
            }
        }

        

    }
}
