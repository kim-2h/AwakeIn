using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class TitleSceneManager : MonoBehaviour
{   
    public SoundManager SoundManager;
    public GameObject OptionCanvas;
    public string nextScene;
    //[SerializeField] 
    //Image progressBar;

    void Awake()
    {
        
    }
    void Start()
    {
        SoundManager.Scenes = SoundManager.EScenes.Title;
        OptionCanvas.SetActive(true);
        OptionCanvas.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;
        OptionCanvas.GetComponent<UISetting>().InitSetting();
    }

    public void StartButton()
    {
        nextScene = "2hBuildTest2";
        StartCoroutine(LoadScene(1));
    }

    public void OptionButton()
    {
        GameObject[] CCAnvas = GameObject.FindGameObjectsWithTag("Setting");
        OptionCanvas = CCAnvas[0];
        OptionCanvas.SetActive(true);
        OptionCanvas.GetComponent<Canvas>().enabled = true; 
    }

    public void ExitButton()
    {
        Debug.Log("Exit");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    IEnumerator LoadScene(int Idx)
    {
        yield return null;
        string nextScene = "";
        GameObject LoadingCanvas = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/LoadingCanvas.prefab");
        GameObject Loading = Instantiate(LoadingCanvas);
        Loading.SetActive(true);
        Loading.GetComponent<Canvas>().enabled = true;

        switch (Idx) 
        {
            case 0:
                nextScene = "Title Test";
                break;
            case 1:
                nextScene = "2hBuildTest2";
                break;
            case 2:
                nextScene = "2hRoom2Test";
                break;
        }


        UnityEngine.AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float minimumLoadingTime = 3.0f;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
        
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {

            }
            else 
            {
                op.allowSceneActivation = true;
            }
        }
        
        if (timer < minimumLoadingTime)
        {
            yield return new WaitForSeconds(minimumLoadingTime - timer);
        }

        
        Destroy(Loading);


    }
}



