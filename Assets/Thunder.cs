using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public GameObject startPos;
    public GameObject thunderPrefab;
    protected GameObject thunder;
    public GameObject TheChar;
    private action charA;
    private bool IsThunder = false;

    // Start is called before the first frame update
    void Start()
    {
        charA = TheChar.GetComponent<action>();

    }

    // Update is called once per frame
    void Update()
    {
        IsThunder = charA.IsThunder();
        
        if(IsThunder)
        {
            ThunderInstantiate();
            IsThunder = false;
        }
    }

    public void ThunderInstantiate() 
    {
        GameObject target = charA.returnTarget();

        thunder = Instantiate(thunderPrefab, target.transform.position, Quaternion.identity);

        if(target.tag=="RedTeam")
        {
            thunder.tag = "BlueTeamBall";
        }

        else if(target.tag=="BlueTeam")
        {
            thunder.tag = "RedTeamBall";
        }

        else
        {
            thunder.tag ="";
        }
    }
}
