using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockManagerCharacter : MonoBehaviour
{
    public TextMeshProUGUI[] numberDisplays;
    private int[] currentCharacters;
    public int[] targetCharacters;
    public BookShelfDrawerManager BDManager;
    public ToyBoxManager TBManager;
    public ElectricBoxManager EBManager;
    void Start()
    {
        currentCharacters = new int[numberDisplays.Length];
        for (int i = 0; i < numberDisplays.Length; i++)
        {
            currentCharacters[i] = 65; // 대문자 'A'의 ASCII 코드는 65입니다.
            UpdateNumberDisplay(i);
        }
    }

    public void InitLock()
    {
        currentCharacters = new int[numberDisplays.Length];
        for (int i = 0; i < numberDisplays.Length; i++)
        {
            currentCharacters[i] = 65; // 대문자 'A'의 ASCII 코드는 65입니다.
            UpdateNumberDisplay(i);
        }
    }

    public void IncreaseNumber(int panelIndex)
    {
        currentCharacters[panelIndex] = (currentCharacters[panelIndex] + 1 - 65) % 26 + 65;
        UpdateNumberDisplay(panelIndex);
    }

    public void DecreaseNumber(int panelIndex)
    {
        currentCharacters[panelIndex] = (currentCharacters[panelIndex] - 1 - 65 + 26) % 26 + 65;
        UpdateNumberDisplay(panelIndex);
    }

    void UpdateNumberDisplay(int panelIndex)
    {
        numberDisplays[panelIndex].text = ((char)currentCharacters[panelIndex]).ToString();
    }

    public void CheckCombination()
    {
        bool isComplete = true;

        for (int i = 0; i < currentCharacters.Length; i++)
        {
            if (currentCharacters[i] != targetCharacters[i])
            {
                isComplete = false;
                break;
            }
        }

        if (isComplete)
        {
            Debug.Log("맞았습니다!");
            SoundManager.Instance.PlaySFX(1);

            if (currentCharacters[0] == 84)
            {
                if (BDManager != null) 
                BDManager.LockSolved();
            }
            else if (currentCharacters[0] == 78)
            {
                if (TBManager != null) 
                TBManager.LockSolved();
            }
            else if (currentCharacters[0] == 82)
            {
                if (EBManager != null) 
                EBManager.LockSolved();
            }
        }
        else
        {
            Debug.Log("틀렸습니다");
        }
    }
}
