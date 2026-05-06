using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class teamManager : MonoBehaviour

{
    public teamManager Enemy;
    public int teamNum = 3;
    internal GameObject[] Team;
    private Vector3 [] RandPos;
    private string teamTag;

    [SerializeField] private GameObject character_1;
    [SerializeField] private GameObject character_2;
    [SerializeField] private GameObject character_3;

    protected float[] targetDist;
    protected GameObject[] targets;
    internal float nearestDist;
    internal GameObject nearestTarget;
    internal int lowestHP;
    internal GameObject lowestHPTarget;
    internal GameObject lowest_HPTarget;

    protected float exploreTimer;
    protected float timeDeliver;
    protected bool isAlive;
    internal int damage = 0;
    internal int count = 0;
    internal int teamScore = 0;


    // Start is called before the first frame update
    protected void Start()
    {
        teamTag = this.gameObject.tag;
        Team = new GameObject[teamNum];

        targetDist = new float[teamNum];
        targets = new GameObject[teamNum];

        exploreTimer = 0.3f;
        timeDeliver = 0.5f;

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
        MPStratgy(teamNum);
        //Debug.Log(nearestTarget);
        //Debug.Log(nearestDist);
        //Debug.Log(damage);
    }

    public Vector3[] BlueTeamSpawnLocation(int count)
    {
        RandPos = new Vector3[count];

        float BlueminX = -5.3f;
        float BluemaxX = -1.0f;
        float BlueminY = -2.5f;
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
        float RedminY = -2.5f;
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

            if (i == 0)
            {
                Team[i] = Instantiate(character_1, teamPos, Quaternion.identity);
            }
            else if (i == 1)
            {
                Team[i] = Instantiate(character_2, teamPos, Quaternion.identity);
            }

            else if (i == 2)
            {
                Team[i] = Instantiate(character_3, teamPos, Quaternion.identity);
            }
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

    public GameObject[] EnemyList(int count)
    {
        for (int i = 0; i < count; i++)
        {
            targets[i] = Enemy.Team[i];
        }

        return targets;
    }

    void TimerOperate()
    {
        int[] blueStrategy = SettingManager.ReturnBlueStrategyIndex();
        int[] redStrategy = SettingManager.ReturnRedStrategyIndex();

        int blueChoice = blueStrategy[0]; 
        int redChoice = redStrategy[0];

        if (timeDeliver >= exploreTimer)
        {
            timeDeliver = 0.0f;

            switch (blueChoice)
            {
                case 0: ExploreTargetViaLowerHP(teamNum);
                break;
                case 1: ExploreTargetViaDistance(teamNum);
                break;
                case 2: ExploreTargetViaLowerHP(teamNum);
                break;
            }

            switch (redChoice)
            {
                case 0: ExploreTargetViaLowerHP(teamNum);
                break;
                case 1: ExploreTargetViaDistance(teamNum);
                break;
                case 2: ExploreTargetViaLowerHP(teamNum);
                break;
            }
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

    public void HealthInitial(GameObject meThis, GameObject target) 
    {
        character_property thisCharP = meThis.GetComponent<character_property>();
        character_property tarCharP = target.GetComponent<character_property>();
        action thisA = meThis.GetComponent<action>();
        action tarA = target.GetComponent<action>();

        //Debug.Log("Attacking");
        //Debug.Log(thisA.isRangeAttacking);
        //Debug.Log(thisA.IsAttacking());

        DamageCalculation(thisCharP, tarCharP, thisA, tarA); 
    }

    public void DamageCalculation(character_property meThis, character_property target, action thisA, action tarA) 
    {
        int i = 0;

        if (meThis.skillPower.Length<0)
        {
            i = thisA.currentSkillIndex;
        }
        else
        {
            i = thisA.animationState - 10;
        }    

        // Debug.Log(meThis.skillPower[i]);
        if(i>0 && i < meThis.skillPower.Length)
        {
            damage = Mathf.Max(meThis.skillPower[i] * meThis.atk / (target.def + 10), 0);            
        }

        else
        {
            damage =  Mathf.Max(30 * meThis.atk / (target.def + 10), 0);            
        }

        //Debug.Log(damage);

        target.Damageable(damage);
        meThis.IndicatorDamage(damage);
        target.IndicatorBurden(damage);

        target.DamageList.Add(meThis.gameObject);


        // Debug.Log(target.isDead);

        for(int k=0;k<target.DamageList.Count;k++)
        {
            if(target.isDead)
            {
                target.isDead = false;
                meThis.killCount++;
                target.deadCount++;
                teamScore++;

                HashSet<GameObject> assistSet = new HashSet<GameObject>(target.DamageList);
                assistSet.Remove(meThis.gameObject);  
                foreach (GameObject assistant in assistSet)
                {
                character_property assistantProp = assistant.GetComponent<character_property>();
                if (assistantProp != null)
                        assistantProp.assistCount++;
                }
                break;
            }
        }

        

        // tarA.isAttacked = true;
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
            nearestDist = 300.0f;

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
        }
    }

    public void ExploreTargetViaLowerHP(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject meThis = Team[i];
            nearestDist = 300.0f;

            for(int j =0;j<count;j++)
            {
                GameObject target = targets[j];
                character_property tarCharP = target.GetComponent<character_property>();
                lowestHP = 1000;
                targetDist[j] = Vector3.Distance(target.transform.position, meThis.transform.position);


                if(tarCharP.hp < lowestHP && targetDist[j] < 100.0f)
                {
                    lowestHP = tarCharP.hp;
                    lowestHPTarget = target;   // find the target
                }     

            }

            action Action = meThis.GetComponent<action>();
            Action.ReceiveLowestHPTarget(lowestHPTarget);
        }
    }

    public void ExploreTargetViaLower_HP(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject meThis = Team[i];
            nearestDist = 300.0f;

            for(int j =0;j<count;j++)
            {
                GameObject target = targets[j];
                character_property tarCharP = target.GetComponent<character_property>();
                lowestHP = 1000;
                targetDist[j] = Vector3.Distance(target.transform.position, meThis.transform.position);
                if(tarCharP._hp < lowestHP && targetDist[j] < 100.0f)
                {
                    lowestHP = tarCharP._hp;
                    lowestHPTarget = target;   // find the target
                }     
            }

            action Action = meThis.GetComponent<action>();
            Action.ReceiveLowest_HPTarget(lowest_HPTarget);
        }
    }

    public void MPStratgy(int count)
    {
        for(int i =0;i<count;i++)
        {
            GameObject meThis = Team[i];
            GameObject target = Enemy.Team[i];
            character_property charP = meThis.GetComponent<character_property>();
            character_property charTar = target.GetComponent<character_property>();

            int[] blueStrategy = SettingManager.ReturnBlueStrategyIndex();
            int blueChoice = blueStrategy[1];
            int[] redStrategy = SettingManager.ReturnBlueStrategyIndex();
            int redChoice = redStrategy[1];

            if(Team[i].tag=="BlueTeam")
            {
                switch(blueChoice)
                {
                    case 0:
                        charP.mp /= 2;
                        break;
                    case 1:
                        charP.mp /= 1;
                        break;
                    case 2:
                        charP.mp /= 0.5f;
                        break;
                }

                switch(redChoice)
                {
                    case 0:
                        charTar.mp /= 2;
                        break;
                    case 1:
                        charTar.mp /= 1;
                        break;
                    case 2:
                        charTar.mp /= 0.5f;
                        break;
                }            
            }

            else
            {
                switch(redChoice)
                {
                    case 0:
                        charP.mp /= 2;
                        break;
                    case 1:
                        charP.mp /= 1;
                        break;
                    case 2:
                        charP.mp /= 0.5f;
                        break;
                }

                switch(blueChoice)
                {
                    case 0:
                        charTar.mp /= 2;
                        break;
                    case 1:
                        charTar.mp /= 1;
                        break;
                    case 2:
                        charTar.mp /= 0.5f;
                        break;
                }   
            }
        }
    }
}

