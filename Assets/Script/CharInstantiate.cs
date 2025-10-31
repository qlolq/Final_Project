using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInstantiate : MonoBehaviour
{
    public teamManager team_manager;

    private float [] BlueRandPosX;
    private float [] BlueRandPosY;
    private float[] RedRandPosX;
    private float[] RedRandPosY;
    private Vector3 [] BlueRandPos;
    private Vector3 [] RedRandPos;

    private float RedminX;
    private float RedmaxX;
    private float RedminY;
    private float RedmaxY;
    private float BlueminX;
    private float BluemaxX;
    private float BlueminY;
    private float BluemaxY;

    private int count;

    // Start is called before the first frame update
    void Start()
    {
        //team_manager = GetComponent<teamManager>();

        count = team_manager.teamNum;

        BlueRandPos = new Vector3 [count];
        RedRandPos = new Vector3 [count];

        RedminX = 1.0f;
        RedmaxX = 5.3f;
        RedminY = -3.0f;
        RedmaxY = 3.0f;
        BluemaxX = -1.0f;
        BlueminX = -5.3f;
        BlueminY = -3.0f;
        BluemaxY = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BlueTeamSpawnLocation() 
    {
        for (int i=0; i<count;i++) 
        {
            float x = Random.Range(BlueminX, BluemaxX);
            float y = Random.Range(BlueminY, BluemaxY);
            BlueRandPos[i] = new Vector3 (x, y, 0);
        }
    }

    public void RedTeamSpawnLocation()
    {
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(RedminX, RedmaxX);
            float y = Random.Range(RedminY, RedmaxY);
            RedRandPos[i] = new Vector3(x, y, 0);
        }
    }

    public Vector3 GetBluePos(int i)
    {
        return BlueRandPos[i];
    }

    public Vector3 GetRedPos(int i) 
    {
        return RedRandPos[i];
    }
}
