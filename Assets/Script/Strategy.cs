using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strategy : MonoBehaviour
{
    protected teamManager team_manager;

    private int count;
    private float[] targetDist;
    private GameObject[] targets;

    private float nearestDist;

    private float exploreTimer;
    private float timeDeliver;
    private bool isOperate;

    private GameObject nearestTarget;

    // Start is called before the first frame update
    void Start()
    {
        team_manager = FindFirstObjectByType<teamManager>();

        count = team_manager.teamNum;
        targetDist = new float[count];
        targets = new GameObject[count];

        EnemyList();

        exploreTimer = 5.0f;
        timeDeliver = 5.0f;
        
        isOperate = true;

        nearestDist = 1000.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(targets);

        if (isOperate)
        {
            TimerOperate();
        }
    }

    public void EnemyList()
    {
        for (int i = 0; i < count; i++)
        {
            if (this.gameObject.CompareTag("RedTeam"))
            {
                targets[i] = team_manager.GetRedTeam(i);
            }

            else if (this.gameObject.CompareTag("BlueTeam"))
            {
                targets[i] = team_manager.GetBlueTeam(i);
            }
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
    public void ExploreTargetViaDistance() 
    {
        for (int i = 0; i < count; i++) 
        {
            GameObject target = targets[i];

            if (target == null)
            {
                nearestTarget = null;
                continue;
            }

            targetDist[i] = Vector3.Distance(target.transform.position , this.transform.position);

            if (targetDist[i] < nearestDist)
            {
                nearestDist = targetDist[i];
                nearestTarget = target;   // find the target
            }
        }
    }

    public GameObject GetNearestTarget() 
    {
        return nearestTarget;
    }

    
}
