using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;   //used for transferred the scene


public class SettingManager : MonoBehaviour
{
    public Button[] strategyBlue = new Button[3];
    public Button[] mpAmountBlue = new Button[3];
    public Button[] strategyRed = new Button[3];
    public Button[] mpAmountRed = new Button[3];
    public Button[] timeSelect = new Button[6];

    public Button confirm;

    static internal int [] strategyRedIndex = new int[3];
    static internal int[] strategyBlueIndex = new int[3];
    private Color normalColor = Color.white;
    private Color selectedColor = Color.yellow;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < strategyBlue.Length; i++)
        {
            int buttonIndex = i;
            strategyBlue[i].onClick.AddListener(() => OnStrategyBlueButtonClicked(buttonIndex));
            strategyRed[i].onClick.AddListener(() => OnStrategyRedButtonClicked(buttonIndex));
            mpAmountBlue[i].onClick.AddListener(() => OnMpAmountBlueButtonClicked(buttonIndex));
            mpAmountRed[i].onClick.AddListener(() => OnMpAmountRedButtonClicked(buttonIndex));
        }

        for (int i = 0; i < timeSelect.Length; i++)
        {
            int buttonIndex = i;
            timeSelect[i].onClick.AddListener(() => OnTimeSelectButtonClicked(buttonIndex));
        }

        confirm.onClick.AddListener(() => OnConfirmButtonClicked());
    }
    void Update()
    {

    }

    void OnStrategyBlueButtonClicked(int index)
    {
        strategyBlueIndex[0] = index;

        for (int i = 0; i < strategyBlue.Length; i++)
        {
            ColorBlock colors = strategyBlue[i].colors;
            if (i == index)
            {
                colors.normalColor = selectedColor;
                colors.highlightedColor = selectedColor;
                colors.pressedColor = selectedColor;
                colors.selectedColor = selectedColor;
            }
            else
            {
                colors.normalColor = normalColor;
                colors.highlightedColor = normalColor;
                colors.pressedColor = normalColor;
                colors.selectedColor = normalColor;
            }
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.1f;
            strategyBlue[i].colors = colors;  
        
        }
    }

    void OnStrategyRedButtonClicked(int index)
    {
        strategyRedIndex[0] = index;

        for (int i = 0; i < strategyRed.Length; i++)
        {
            ColorBlock colors = strategyRed[i].colors;
            if (i == index)
            {
                colors.normalColor = selectedColor;
                colors.highlightedColor = selectedColor;
                colors.pressedColor = selectedColor;
                colors.selectedColor = selectedColor;
            }
            else
            {
                colors.normalColor = normalColor;
                colors.highlightedColor = normalColor;
                colors.pressedColor = normalColor;
                colors.selectedColor = normalColor;
            }
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.1f;
            strategyRed[i].colors = colors;  
        
        }
    }

    void OnMpAmountBlueButtonClicked(int index)
    {
        strategyBlueIndex[1] = index;

        for (int i = 0; i < mpAmountBlue.Length; i++)
        {
            ColorBlock colors = mpAmountBlue[i].colors;
            if (i == index)
            {
                colors.normalColor = selectedColor;
                colors.highlightedColor = selectedColor;
                colors.pressedColor = selectedColor;
                colors.selectedColor = selectedColor;
            }
            else
            {
                colors.normalColor = normalColor;
                colors.highlightedColor = normalColor;
                colors.pressedColor = normalColor;
                colors.selectedColor = normalColor;
            }
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.1f;
            mpAmountBlue[i].colors = colors;  
        
        }
    }

    void OnMpAmountRedButtonClicked(int index)
    {
        strategyRedIndex[1] = index;

        for (int i = 0; i < mpAmountRed.Length; i++)
        {
            ColorBlock colors = mpAmountRed[i].colors;
            if (i == index)
            {
                colors.normalColor = selectedColor;
                colors.highlightedColor = selectedColor;
                colors.pressedColor = selectedColor;
                colors.selectedColor = selectedColor;
            }
            else
            {
                colors.normalColor = normalColor;
                colors.highlightedColor = normalColor;
                colors.pressedColor = normalColor;
                colors.selectedColor = normalColor;
            }
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.1f;
            mpAmountRed[i].colors = colors;  
        
        }
    }

    void OnTimeSelectButtonClicked(int index)
    {
        strategyBlueIndex[2] = index;

        for (int i = 0; i < timeSelect.Length; i++)
        {
            ColorBlock colors = timeSelect[i].colors;
            if (i == index || i == (index+3)%6)
            {
                colors.normalColor = selectedColor;
                colors.highlightedColor = selectedColor;
                colors.pressedColor = selectedColor;
                colors.selectedColor = selectedColor;
            }
            else
            {
                colors.normalColor = normalColor;
                colors.highlightedColor = normalColor;
                colors.pressedColor = normalColor;
                colors.selectedColor = normalColor;
            }
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.1f;
            timeSelect[i].colors = colors;  
        
        }
    }

    void OnConfirmButtonClicked()
    {
        SceneManager.LoadScene("MAINScene");
        ReturnBlueStrategyIndex();
        ReturnRedStrategyIndex();
    }

    static public int[] ReturnBlueStrategyIndex()
    {
        return strategyBlueIndex;
    }

    static public int[] ReturnRedStrategyIndex()
    {
        return strategyRedIndex;
    }
}
