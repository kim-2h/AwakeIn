using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrgelWholeManager : MonoBehaviour, IPuzzle
{
    public bool IsSolved { get; set; }
    public Canvas canvas;
    public InvenManager invenManager;
    public GameObject Orgel;
    public UnityEngine.UI.Image answer;

    // Start is called before the first frame update
    public void StartPuzzle()
    {
        canvas.enabled = true;

        Debug.Log("handle in? : " + invenManager.ItemMap["OrgelHandle"].InInventory);
        Debug.Log("handle used? : " + invenManager.ItemMap["OrgelHandle"].IsUsed);

        if (invenManager.ItemMap["OrgelHandle"].InInventory ||invenManager.ItemMap["OrgelHandle"].IsUsed)
        {
            invenManager.ItemMap["OrgelHandle"].IsUsed = true;
            Orgel.SetActive(true);
        }
        else
        {
            Orgel.SetActive(false);
        }

        if (!IsSolved)
        {
            StartCoroutine(SolvingAnim());
            Debug.Log("Orgel Puzzle anime Started!!!");
        }

    }

    // Update is called once per frame
    public void ExitPuzzle()
    {
        Orgel.SetActive(false);
        canvas.enabled = false;
    }

    void Start()
    {
        IsSolved = false;
        answer.fillAmount = 0.34f;
    }

    IEnumerator SolvingAnim()
    {
        float elapsedTime = 0f;
        float duration = 2f;

        while (elapsedTime < duration)
        {
            // 시간에 따라 fillAmount 계산 (0 ~ 1)
            float t = elapsedTime / duration;

            // Lerp를 사용하여 fillAmount 보간
            answer.fillAmount = Mathf.Lerp(0.34f, 1, t);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 최종 fillAmount 설정 (보정)
        answer.fillAmount = 1f;
        IsSolved = true;
    }
}


