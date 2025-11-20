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

    protected bool isAlive;

    internal int damage = 0;

    // Start is called before the first frame update
    protected void Start()
    {
        teamTag = this.gameObject.tag;
        Team = new GameObject[teamNum];

        targetDist = new float[teamNum];
        targets = new GameObject[teamNum];

        exploreTimer = 2.0f;
        timeDeliver = 2.0f;

        isAlive = true;

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
        GiveName(teamTag);
    }

    void Update()
    {
        EnemyList(teamNum);
        TimerOperate();
        //Debug.Log(nearestTarget);
        //Debug.Log(nearestDist);
        //Debug.Log(damage);
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

    /// <summary> ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //Team character Property, character Position is defined 
    /// </summary>
    /// <param name="teamTag"></param>
    /// 

    void DetectTeamTag(string teamTag)
    {
        for (int i=0; i<teamNum;i++) 
        {
            Team[i].tag = teamTag;
        }
    }

    void GiveName(string teamTag) 
    {
        for (int i = 0; i < teamNum; i++) 
        {
            character_property charP = Team[i].GetComponent<character_property>();
            Team[i].name = $"{teamTag}_{charP.name}";
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
        }

        else {
            timeDeliver += Time.deltaTime;
        }

    }

    /// <summary> ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //Team character Property, character damage determine
    /// </summary>
    /// <param name="Damage"></param>
    /// 

    public void HealthInitial(int count,GameObject meThis, GameObject target) 
    {
        character_property thisCharP = meThis.GetComponent<character_property>();
        character_property tarCharP = target.GetComponent<character_property>();
        action thisA = meThis.GetComponent<action>();
        action tarA = target.GetComponent<action>();

        if (thisA.IsAttacking()) 
        {
            thisA.isAttacking = false;

            if (tarA.IsAttacked())
            {
                DamageCalculation(thisCharP, tarCharP,thisA,tarA);
            }
        }
    }

    public void DamageCalculation(character_property meThis, character_property target, action thisA, action tarA) 
    {
        int i = thisA.animationState - 10;

        damage = Mathf.Max(meThis.skillPower[i] * meThis.atk / (target.def + 10), 0);
        target.Damageable(damage);
        meThis.IndicatorDamage(damage);
    }

    /// <summary> ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// //Team character Property, Strategy
    /// </summary>
    /// <param name="Strategy"></param>
    /// 

    //Strategy 1 -- the nearest target  
    public void ExploreTargetViaDistance(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject meThis = Team[i];
            nearestDist = 1000.0f;

            for (int j = 0; j < count; j++) 
            {
                GameObject target = targets[j];

                if (target == null)
                {
                    nearestTarget = null;
                    continue;
                }

                targetDist[j] = Vector3.Distance(target.transform.position, meThis.transform.position);

                if (targetDist[j] < nearestDist)
                {
                    nearestDist = targetDist[j];
                    nearestTarget = target;   // find the target
                }
            }

            action Action = meThis.GetComponent<action>();
            Action.ReceiveNearestTarget(nearestTarget);

            HealthInitial(count, meThis, nearestTarget);
        }
    }
}

