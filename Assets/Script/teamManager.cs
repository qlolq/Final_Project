using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teamManager : MonoBehaviour

{
    public teamManager Enemy;
    public int teamNum = 1;
    internal GameObject[] Team;
    private Vector3 [] RandPos;
    private string teamTag;

    [SerializeField] private GameObject character;

    protected float[] targetDist;
    protected GameObject[] targets;
    internal float nearestDist;
    internal GameObject nearestTarget;

    protected float exploreTimer;
    protected float timeDeliver;
    protected bool isOperate;

    // Start is called before the first frame update
    protected void Start()
    {
        teamTag = this.gameObject.tag;
        Team = new GameObject[teamNum];

        targetDist = new float[teamNum];
        targets = new GameObject[teamNum];
        nearestDist = 1000.0f;

        exploreTimer = 5.0f;
        timeDeliver = 5.0f;
        isOperate = true;

        if (this.gameObject.CompareTag("BlueTeam"))
        {
            BlueTeamSpawnLocation(teamNum);
        }

        else if (this.gameObject.CompareTag("RedTeam")) 
        {
            RedTeamSpawnLocation(teamNum);
        }

        CharacterInstantiate(teamTag);
        DetectTeamTag(teamTag);
        EnemyList(teamNum);
    }

    void Update()
    {
        if (isOperate)
        {
            isOperate = false;
            TimerOperate();
        }
    }

    public Vector3[] BlueTeamSpawnLocation(int count)
    {
        RandPos = new Vector3[count];

        float BlueminX = -5.3f;
        float BluemaxX = -1.0f;
        float BlueminY = -3.0f;
        float BluemaxY = 3.0f;

        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(BlueminX, BluemaxX);
            float y = Random.Range(BlueminY, BluemaxY);
            RandPos[i] = new Vector3(x, y, 0);
        }

        return RandPos;
    }

    public Vector3[] RedTeamSpawnLocation(int count)
    {
        RandPos = new Vector3[count];

        float RedminX = 1.0f;
        float RedmaxX = 5.3f;
        float RedminY = -3.0f;
        float RedmaxY = 3.0f;

        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(RedminX, RedmaxX);
            float y = Random.Range(RedminY, RedmaxY);
            RandPos[i] = new Vector3(x, y, 0);
        }

        return RandPos;
    }

    void CharacterInstantiate(string teamTag)
    {
        for (int i = 0; i < teamNum; i++)
        {
            Vector3 teamPos = RandPos[i];
            Team[i] = Instantiate(character, teamPos, Quaternion.identity);
        }
    }

/// <summary>
/// //Team character Property, character Position is defined 
/// </summary>
/// <param name="teamTag"></param>
    void DetectTeamTag(string teamTag)
    {
        for (int i=0; i<teamNum;i++) 
        {
            Team[i].tag = teamTag;
        }
    }

    void EnemyList(int count)
    {
        for (int i = 0; i < count; i++)
        {
            targets[i] = Enemy.Team[i];
        }
    }

    void TimerOperate()
    {
        if (timeDeliver >= exploreTimer)
        {
            ExploreTargetViaDistance(teamNum);
            timeDeliver = 0.0f;
            isOperate = true;
        }

        else 
        {
            timeDeliver += Time.deltaTime;
        }

        Debug.Log(timeDeliver);
    }

    //Strategy 1 -- the nearest target  
    public void ExploreTargetViaDistance(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject target = targets[i];

            if (target == null)
            {
                nearestTarget = null;
                continue;
            }

            targetDist[i] = Vector3.Distance(target.transform.position, this.transform.position);

            if (targetDist[i] < nearestDist)
            {
                nearestDist = targetDist[i];
                nearestTarget = target;   // find the target
            }
        }
    }
}

