using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiziBall : MonoBehaviour
{
    public GameObject startPos;
    public GameObject liziballPrefab;
    protected GameObject liziball;
    public GameObject TheChar;
    private action charA;
    private bool isSkillFire = false;

    // Start is called before the first frame update
    void Start()
    {
        charA = TheChar.GetComponent<action>();

    }

    // Update is called once per frame
    void Update()
    {
        isSkillFire = charA.IsSkillFire();
        
        if(isSkillFire)
        {
            LiziBallInstantiate();
            isSkillFire = false;
        }
    }

    public void LiziBallInstantiate() 
    {
        GameObject target = charA.returnTarget();

        liziball = Instantiate(liziballPrefab, target.transform.position, Quaternion.identity);

        if(target.tag=="RedTeam")
        {
            liziball.tag = "BlueTeamBall";
        }

        else if(target.tag=="BlueTeam")
        {
            liziball.tag = "RedTeamBall";
        }

        else
        {
            liziball.tag ="";
        }
    }
}
