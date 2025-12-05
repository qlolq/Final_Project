using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public GameObject[] blueIndicator;
    public GameObject[] redIndicator;
    public teamManager BlueTeam;
    public teamManager RedTeam;
    public TextMeshProUGUI[] BlueIndicatorDamage;
    public TextMeshProUGUI[] BlueIndicatorBurden;
    public TextMeshProUGUI[] RedIndicatorDamage;
    public TextMeshProUGUI[] RedIndicatorBurden;
    public TextMeshProUGUI[] BlueIndicatorName;
    public TextMeshProUGUI[] RedIndicatorName;


    // Start is called before the first frame update
    void Start()
    {
        BlueIndicatorHide();
        RedIndicatorHide();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBlueIndicatorDetail(BlueTeam.teamNum);
        UpdateRedIndicatorDetail(BlueTeam.teamNum);
    }

    void BlueIndicatorHide() 
    {
        for (int i = 3; i > BlueTeam.teamNum-1; i--)
        {
            blueIndicator[i].SetActive(false);
        }
    }

    void RedIndicatorHide()
    {
        for (int i = 3; i > RedTeam.teamNum-1; i--)
        {
            redIndicator[i].SetActive(false);
        }
    }

    void UpdateBlueIndicatorDetail(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (count < 0) 
            {
                return;
            }
            character_property charP = BlueTeam.Team[i].GetComponent<character_property>();
            BlueIndicatorDamage[i].text = charP.indDamage.ToString();
            BlueIndicatorBurden[i].text = charP.indBurden.ToString();
            BlueIndicatorName[i].text = charP.name;

        }
    }

    void UpdateRedIndicatorDetail(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (count < 0)
            {
                return;
            }
            character_property charP = RedTeam.Team[i].GetComponent<character_property>();
            RedIndicatorDamage[i].text = charP.indDamage.ToString();
            RedIndicatorBurden[i].text = charP.indBurden.ToString();
            RedIndicatorName[i].text = charP.name;
        }
    }
}
