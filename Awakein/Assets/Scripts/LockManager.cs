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
public void IncreaseNumber1()
{
    currentNumbers[0] = (currentNumbers[0] + 1) % 10; // 첫 번째 숫자창의 숫자를 증가시킴
    UpdateNumberDisplay(0);
}

public void DecreaseNumber1()
{
    currentNumbers[0] = (currentNumbers[0] + 9) % 10; // 첫 번째 숫자창의 숫자를 감소시킴
    UpdateNumberDisplay(0);
}

// 상버튼2와 하버튼2에 대한 함수
public void IncreaseNumber2()
{
    currentNumbers[1] = (currentNumbers[1] + 1) % 10; // 두 번째 숫자창의 숫자를 증가시킴
    UpdateNumberDisplay(1);
}

public void DecreaseNumber2()
{
    currentNumbers[1] = (currentNumbers[1] + 9) % 10; // 두 번째 숫자창의 숫자를 감소시킴
    UpdateNumberDisplay(1);
}

// 상버튼3와 하버튼3에 대한 함수
public void IncreaseNumber3()
{
    currentNumbers[2] = (currentNumbers[2] + 1) % 10; // 세 번째 숫자창의 숫자를 증가시킴
    UpdateNumberDisplay(2);
}

public void DecreaseNumber3()
{
    currentNumbers[2] = (currentNumbers[2] + 9) % 10; // 세 번째 숫자창의 숫자를 감소시킴
    UpdateNumberDisplay(2);
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