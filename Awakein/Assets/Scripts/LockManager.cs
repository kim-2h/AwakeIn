using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockManager : MonoBehaviour
{
public TextMeshProUGUI[] numberDisplays; // 각 자물쇠 패널의 숫자를 표시할 Text 배열
private int[] currentNumbers; // 각 자물쇠 패널의 현재 숫자를 저장할 배열
public int[] targetNumbers; // 맞춰야 할 숫자 조합
void Start()
{
    currentNumbers = new int[numberDisplays.Length];
    // 자물쇠 패널 초기화
    for (int i = 0; i < numberDisplays.Length; i++)
    {
        currentNumbers[i] = 0;
        UpdateNumberDisplay(i);
    }
}

// 숫자를 올리거나 내리는 함수
    public void IncreaseNumber(int panelIndex)
    {
        currentNumbers[panelIndex] = (currentNumbers[panelIndex] + 1) % 10;
        UpdateNumberDisplay(panelIndex);
    }

    public void DecreaseNumber(int panelIndex)
    {
        currentNumbers[panelIndex] = (currentNumbers[panelIndex] + 9) % 10;
        UpdateNumberDisplay(panelIndex);
    }

// 패널에 숫자 업데이트
void UpdateNumberDisplay(int panelIndex)
{
    numberDisplays[panelIndex].text = currentNumbers[panelIndex].ToString();
}

// 확인 버튼 클릭 시 호출될 함수
public void CheckCombination()
{
    bool isComplete = true;

    // 택한 숫자와 목표 숫자를 비교
    for (int i = 0; i < currentNumbers.Length; i++)
    {
        if (currentNumbers[i] != targetNumbers[i])
        {
            isComplete = false;
            break;
        }
    }

    // 결과 출력
    if (isComplete)
    {
        Debug.Log("맞췄습니다! 이 숫자는 조효원의 생일입니다!");
    }
    else
    {
        Debug.Log("다시 해라 멍청아.");
    }
}
}