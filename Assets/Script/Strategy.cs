using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strategy : teamManager
{
    public teamManager team_manager;

    protected int count;
    protected float[] targetDist;
    protected float nearestDist;

    private float exploreTimer;
    private float timeDeliver;
    private bool isOperate;

    internal GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        team_manager = GetComponent<teamManager>();

        count = team_manager.teamNum;

        exploreTimer = 5.0f;
        timeDeliver = 5.0f;
        
        isOperate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOperate)
        {
            TimerOperate();
        }


    }

    void TimerOperate() 
    {
        isOperate = false;
        timeDeliver += Time.deltaTime;

        if (timeDeliver >= exploreTimer)
        {
            ExploreTargetViaDistance();
            timeDeliver = 0.0f;
            isOperate = true;
        }
    }

    //Strategy 1 -- the nearest target  
    void ExploreTargetViaDistance() 
    {
        for (int i = 0; i < count; i++) 
        {
            targetDist[i] = Vector3.Distance(team_manager.targets[i].transform.position , this.transform.position);

            if (i == 0) 
            {
                nearestDist = targetDist[i];
            }

            if (i > 0 && i < count) 
            {
                if (targetDist[i] < nearestDist) 
                {
                    nearestDist = targetDist[i];
                    target = team_manager.targets[i];   // find the target
                }
            }
        }
    }
}
