using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Video;

public class TitleSceneManager : MonoBehaviour
{   
    public SoundManager SoundManager;
    public GameObject OptionCanvas;
    public Camera cam;
    public string nextScene;
    //[SerializeField] 
    //Image progressBar;

    void Awake()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Clicked();
        }

    
    }

    public void Clicked()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10f);



        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == "StartButton")
            {
                Debug.Log("StartButton");
                SoundManager.PlaySFX(0);
                StartButton();
            }
            else if (hit.transform.name == "ExitButton")
            {
                Debug.Log("ExitButton");
                SoundManager.PlaySFX(0);
                ExitButton();
            }
            else if (hit.transform.name == "Window")
            {
                Debug.Log("Window");
                SoundManager.PlaySFX(1);
            }
            else if (hit.transform.name == "Grass1" || hit.collider.gameObject.name == "Grass2")
            {
                Debug.Log("Grass");
                SoundManager.PlaySFX(2);
                
            }
        }
    }
    void Start()
    {
        SoundManager.Scenes = SoundManager.EScenes.Title;
        //OptionCanvas.SetActive(true);
        //OptionCanvas.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;
        //OptionCanvas.GetComponent<UISetting>().InitSetting();
    }

    public void StartButton()
    {
        nextScene = "2hBuildTest2";
        StartCoroutine(LoadFirstScene());
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

    IEnumerator LoadFirstScene()
    {
        SkipDone = false;

        yield return null;
        string nextScene = "2hBuildTest2";

        SoundManager.StopBGM();

        #if UNITY_EDITOR
            string path = "Assets/Prefabs/TutorialCanvas.prefab";
            GameObject LoadingCanvas = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        #else
            string path = "CustomSprites/TutorialCanvas";
            GameObject LoadingCanvas = Resources.Load(path) as GameObject;
        #endif



        GameObject Loading = Instantiate(LoadingCanvas);
        Loading.SetActive(true);
        Loading.GetComponent<Canvas>().enabled = true;

        TextMeshProUGUI LoadingText = GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        LoadingText.text = "Loading...";
        //Loading.transform.GetChild(0).gameObject.SetActive(false);

        VideoPlayer LoadingVideo;
        Button SkipButton;
        LoadingVideo = GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>();

        SkipButton = GameObject.Find("SkipButton").GetComponent<Button>();
        GameObject Screen = LoadingCanvas.transform.GetChild(1).gameObject;
        SkipButton.onClick.AddListener(() => SkipPressed(Screen, LoadingVideo));

        LoadingVideo.time = 0.0f;
        LoadingVideo.Play();


        UnityEngine.AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float minimumLoadingTime = 2f;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
        
            timer += Time.unscaledDeltaTime;
            if (op.progress < 0.9f)
            {
                if (SkipDone)
                {
                    LoadingVideo.Stop();
                    LoadingVideo.time = LoadingVideo.clip.length - 0.1f;

                    //Loading.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            else if (SkipDone)
            {
                LoadingVideo.Stop();
                op.allowSceneActivation = true;
                //Loading.transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (LoadingVideo.time >= LoadingVideo.clip.length - 0.1f)
            {
                op.allowSceneActivation = true;
            }
            // else
            // {
            //     op.allowSceneActivation = true;
            // }
        }
        
        if (timer < minimumLoadingTime)
        {
            yield return new WaitForSeconds(minimumLoadingTime - timer);
        }

        
        Destroy(Loading);

    }

    IEnumerator LoadScene(int Idx)
    {
        yield return null;
        string nextScene = "";

        #if UNITY_EDITOR
            string path = "Assets/Prefabs/LoadingCanvas.prefab";
            GameObject LoadingCanvas = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        #else
            string path = "CustomSprites/LoadingCanvas";
            GameObject LoadingCanvas = Resources.Load(path) as GameObject;
        #endif




        GameObject Loading = Instantiate(LoadingCanvas);
        Loading.SetActive(true);
        Loading.GetComponent<Canvas>().enabled = true;

        TextMeshProUGUI LoadingText = GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        LoadingText.text = "Loading...";

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
        float minimumLoadingTime = 2.5f;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
        
            timer += Time.unscaledDeltaTime;
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

    public bool SkipDone = false;
    public void SkipPressed(GameObject Go, VideoPlayer VP)
    {
        Go.SetActive(false);
        VP.Stop();
        VP.time = VP.clip.length - 0.1f;
    
        SkipDone = true;
    }
}



