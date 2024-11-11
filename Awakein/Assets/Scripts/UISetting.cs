using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.EventSystems;
using UnityEditor.Rendering;
using UnityEngine.SocialPlatforms;
using TMPro;
using UnityEditor.AddressableAssets.Build;
using System.ComponentModel;
using UnityEngine.SceneManagement;
using UnityEditor;


//using UnityEditor.Localization.Editor;
public class UISetting : MonoBehaviour
{
    public Canvas SettingCanvas;
    public Camera cam;
    [SerializeField] private Button Selected;
    private int SelectedIndex = 0;
    public Button SoundBT, GraphicBT, LanguageBT, ScreenBT;
    public TMP_Dropdown LanguageDD;

    public GameObject[][] ButtonContents = new GameObject[4][];
    public GameObject[] SoundsChild;
    public GameObject[] GraphicsChild;
    public GameObject[] LanguagesChild;
    public GameObject[] ScreensChild;
    
    private bool IsFirstPlay = true;
    public InvenManager InvenManager;

    void Awake()
    {   
        Screen.SetResolution(1920, 1080, true);
        SetRes();
        OnPreCull();
        
        // GameObject[] SETTT = GameObject.FindGameObjectsWithTag("Setting");
        // if (SETTT.Length >= 2) 
        // {
        //     Destroy(this.gameObject);
        //     // for (int i = 1; i < SETTT.Length; i++)
        //     // {
        //     //     if (SETTT[i] != this.gameObject)
        //     //         Destroy(SETTT[i]);
        //     // }
        // }
        // else DontDestroyOnLoad(this.gameObject);

        this.gameObject.SetActive(true);
        this.GetComponent<Canvas>().enabled = false;

        // if (LocalizationSettings.SelectedLocale != LocalizationSettings.AvailableLocales.Locales[0])
        // {
        //     LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        //     StartCoroutine(LocaleChange(0));
        // }


        SoundBT.onClick.AddListener(() => SelectedButton(SoundBT, 0));
        GraphicBT.onClick.AddListener(() => SelectedButton(GraphicBT, 1));
        LanguageBT.onClick.AddListener(() => SelectedButton(LanguageBT, 2));
        ScreenBT.onClick.AddListener(() => SelectedButton(ScreenBT, 3));

        Button[] Buttons; 
        Buttons = new Button[4] { SoundBT, GraphicBT, LanguageBT, ScreenBT};

    for (int i = 0; i < Buttons.Length; i++)
    {
        int index = i; // 새로운 지역 변수에 i 값을 할당
        EventTrigger trigger = Buttons[index].gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => MouseEnter(Buttons[index].gameObject, index));
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => MouseExit(Buttons[index].gameObject, index));
        trigger.triggers.Add(entryExit);
    }
    

        ButtonContents[0] = SoundsChild;
        ButtonContents[1] = GraphicsChild;
        ButtonContents[2] = LanguagesChild;
        ButtonContents[3] = ScreensChild;

        ButtonFX = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>().SFXs[14];
    }
    void Start()
    {
        

        Debug.Log("Setting Started !!!!!!!!");

        SettingCanvas.enabled = true;
        if (!PlayerPrefs.HasKey("FirstPlay")) PlayerPrefs.SetInt("FirstPlay", 1);
        //IsFirstPlay = PlayerPrefs.GetInt("FirstPlay") == 1 ? true : false;

        if (PlayerPrefs.GetInt("FirstPlay") == 1)
        {
            //if (!PlayerPrefs.HasKey("Master")) 
            PlayerPrefs.SetFloat("Master", 0.6f);
            //if (!PlayerPrefs.HasKey("BGM")) 
            PlayerPrefs.SetFloat("BGM", 0.6f);
            //if (!PlayerPrefs.HasKey("SFX")) 
            PlayerPrefs.SetFloat("SFX", 0.6f);

            //if (!PlayerPrefs.HasKey("FullScreen")) 
            PlayerPrefs.SetInt("FullScreen", 1);
            //if (!PlayerPrefs.HasKey("ShadowOn")) 
            PlayerPrefs.SetInt("ShadowOn", 1);
            //if (!PlayerPrefs.HasKey("VolumeOn")) 
            PlayerPrefs.SetInt("VolumeOn", 1);
            //if (!PlayerPrefs.HasKey("Language")) 
            PlayerPrefs.SetInt("Language", 0);


            PlayerPrefs.SetInt("FirstPlay", 0);
        }



        // Master.value = PlayerPrefs.GetFloat("Master"); BGM.value = PlayerPrefs.GetFloat("BGM"); 
        // SFX.value = PlayerPrefs.GetFloat("SFX");


        // isFullScreen = ScreenToggle.isOn;
        // isShadow = ShadowToggle.isOn;
        // isVolume = VolumeToggle.isOn;
        // Screen.fullScreen = isFullScreen;

        
        // volume[0] = PlayerPrefs.GetFloat("Master");
        // volume[1] = PlayerPrefs.GetFloat("BGM");
        // volume[2] = PlayerPrefs.GetFloat("SFX");

        cam = Camera.main;
        // var GVS = GameObject.FindGameObjectsWithTag("GlobalVolume");

        // foreach (var GV in GVS)
        // {
        //     Debug.Log("Global Volume list : " + GV.name);
        //     if (GV.activeSelf == false) continue;
            
        //     GVolume = GV;
        //     Debug.Log("This Scene's Volume : " + GVolume.name);
        // }

        // if (GVolume != null) GVolume.SetActive(VolumeToggle.isOn);

        InitSetting();
        //SettingCanvas.enabled = false;
        ExitSetting();
        // Selected = SoundBT;
        // SelectedIndex = 0;

        // //ButtonContents[0][1].gameObject.SetActive(true);
        // ButtonContents[0][2].gameObject.SetActive(true);
        // ButtonContents[0][0].gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 

    }
    public void InitSetting()
    {
        if (!PlayerPrefs.HasKey("FirstPlay") || PlayerPrefs.GetInt("FirstPlay") == 1)
        {
            //if (!PlayerPrefs.HasKey("Master")) 
            PlayerPrefs.SetFloat("Master", 0.6f);
            //if (!PlayerPrefs.HasKey("BGM")) 
            PlayerPrefs.SetFloat("BGM", 0.6f);
            //if (!PlayerPrefs.HasKey("SFX")) 
            PlayerPrefs.SetFloat("SFX", 0.6f);

            //if (!PlayerPrefs.HasKey("FullScreen")) 
            PlayerPrefs.SetInt("FullScreen", 1);
            //if (!PlayerPrefs.HasKey("ShadowOn")) 
            PlayerPrefs.SetInt("ShadowOn", 1);
            //if (!PlayerPrefs.HasKey("VolumeOn")) 
            PlayerPrefs.SetInt("VolumeOn", 1);
            //if (!PlayerPrefs.HasKey("Language")) 
            PlayerPrefs.SetInt("Language", 0);


            PlayerPrefs.SetInt("FirstPlay", 0);
        }

        // GameObject[] PPointlight = GameObject.FindGameObjectsWithTag("PointLight");
        // if (PPointlight.Length > 0) PointLight = PPointlight[0].GetComponent<Light>();
        // GameObject[] GGlovalvolume = GameObject.FindGameObjectsWithTag("GlobalVolume");
        // if (GGlovalvolume.Length > 0) GVolume = GGlovalvolume[0];


        ButtonContents[0][2].gameObject.SetActive(true);
        SettingCanvas.enabled = true;
        Selected = SoundBT;
        SelectedIndex = 0;
        //ButtonContents[0][1].gameObject.SetActive(true);
        ButtonContents[0][0].gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 
        
        if (LocalizationSettings.SelectedLocale != LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("Language")])
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("Language")];
            StartCoroutine(LocaleChange(PlayerPrefs.GetInt("Language")));
        }

        LanguageDD.value = PlayerPrefs.GetInt("Language");

        Master.value = PlayerPrefs.GetFloat("Master");
        ChangeMaster();
        BGM.value = PlayerPrefs.GetFloat("BGM");
        ChangeBGM();
        SFX.value = PlayerPrefs.GetFloat("SFX");
        ChangeSFX();
        ScreenToggle.isOn = PlayerPrefs.GetInt("FullScreen") == 1 ? true : false;
        FullScreen(ScreenToggle.isOn);

        
        ShadowToggle.isOn = PlayerPrefs.GetInt("ShadowOn") == 1 ? true : false;
        SetShadow(ShadowToggle.isOn);
        VolumeToggle.isOn = PlayerPrefs.GetInt("VolumeOn") == 1 ? true : false;
        SetGlobalVolume(VolumeToggle.isOn);
  


        ButtonContents[1][1].gameObject.SetActive(false);
        ButtonContents[1][2].gameObject.SetActive(false);

        ButtonContents[2][1].gameObject.SetActive(false);
        ButtonContents[2][2].gameObject.SetActive(false);
    
        ButtonContents[3][1].gameObject.SetActive(false);
        ButtonContents[3][2].gameObject.SetActive(false);


    }
    private void SetRes()
    {
        float fixedAspectRatio = 16.0f / 9.0f;
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;
        if(currentAspectRatio == fixedAspectRatio)
        {
            cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            return;
        }
        else if(currentAspectRatio > fixedAspectRatio)
        {
            float w = fixedAspectRatio / currentAspectRatio;
            float x = (1 - w) / 2;
            cam.rect = new Rect(x, 0.0f, w, 1.0f);
        }
        else if(currentAspectRatio < fixedAspectRatio)
        {
            float h = currentAspectRatio / fixedAspectRatio;
            float y = (1 - h) / 2;
            cam.rect = new Rect(0.0f, y, 1.0f, h);
        }

    }
    void OnPreCull() => GL.Clear(true, true, Color.black);

    public void MouseEnter(GameObject BT, int idx)
    {
        //Debug.Log("Button name : " + BT.name + " index : " + idx);
        ButtonContents[idx][1].gameObject.SetActive(true);
        ButtonContents[idx][0].gameObject.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);  
    }

    public void MouseExit(GameObject BT, int idx)
    {
        ButtonContents[idx][1].gameObject.SetActive(false);
        ButtonContents[idx][0].gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);   
    }

    private void SelectedButton(Button BT, int idx)
    {
        //Debug.Log("Button name : " + BT.name + " index : " + idx);
        if (BT != Selected) 
        {
            ClickSound();

            ButtonContents[SelectedIndex][1].gameObject.SetActive(false);
            ButtonContents[SelectedIndex][2].gameObject.SetActive(false);
            ButtonContents[SelectedIndex][0].gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);   
            //BT.transform.GetChild(1).gameObject.SetActive(true);
            ButtonContents[idx][2].gameObject.SetActive(true);
            ButtonContents[idx][0].gameObject.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
            Selected = BT;
            SelectedIndex = idx;
        }
        if (BT == SoundBT)
        {
            Master.value = PlayerPrefs.GetFloat("Master");
            BGM.value = PlayerPrefs.GetFloat("BGM");
            SFX.value = PlayerPrefs.GetFloat("SFX");
        }
    }
    public Light PointLight;
    public GameObject GVolume;
    public void SetShadow(bool T)
    {
        T = ShadowToggle.isOn;
        isShadow = T;

        GameObject[] PPointlight = GameObject.FindGameObjectsWithTag("PointLight");
        if (PPointlight.Length > 0) PointLight = PPointlight[0].GetComponent<Light>();

        if (PointLight == null) return;

        PointLight.shadows = T ? LightShadows.Soft : LightShadows.None;
        PlayerPrefs.SetInt("ShadowOn", T ? 1 : 0);
    }
    public void SetGlobalVolume(bool T)
    {
        T = VolumeToggle.isOn;

        GameObject[] GGlovalvolume = GameObject.FindGameObjectsWithTag("GlobalVolume");
        if (GGlovalvolume.Length > 0) GVolume = GGlovalvolume[0];

        if (GVolume != null) GVolume.SetActive(T);
        isVolume = T;
        PlayerPrefs.SetInt("VolumeOn", T ? 1 : 0);
    }

    public void InSettingMode()
    {
        SettingCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void ExitSetting()
    {
        ClickSound();
        SettingCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1;
    }
    //public static float[] volume = new float[3] { 0.6f, 0.6f, 0.6f };
    public static bool isFullScreen = true;
    public static bool isShadow = true;
    public static bool isVolume = true;

    public Slider Master, BGM, SFX;
    public AudioMixer MasterMixer;
    public Toggle ScreenToggle, ShadowToggle, VolumeToggle;
    public AudioSource ButtonFX;

    public void ChangeMaster()
    {
        //volume[0] = Master.value;
        PlayerPrefs.SetFloat("Master", Master.value);
        MasterMixer.SetFloat("Master", Mathf.Log10(PlayerPrefs.GetFloat("Master")) * 30);
        //PlayerPrefs.SetFloat("Master", volume[0]);
    }
    public void ChangeBGM()
    {
        //volume[1] = BGM.value;
        PlayerPrefs.SetFloat("BGM", BGM.value);
        MasterMixer.SetFloat("BGM", Mathf.Log10(PlayerPrefs.GetFloat("BGM")) * 40);
        //PlayerPrefs.SetFloat("BGM", volume[1]);
    }
    public void ChangeSFX()
    {
        //volume[2] = SFX.value;
        PlayerPrefs.SetFloat("SFX", SFX.value);
        MasterMixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFX")) * 30);
        //PlayerPrefs.SetFloat("SFX", volume[2]);

    }

    public void FullScreen(bool T) //빌드한 다음에 잘 돌아가는지 확인해야함
    {
        
        T = ScreenToggle.isOn;
        //isFullScreen = T;
        Screen.fullScreen = T;
        Debug.Log("fullscreen : " + T);
        if (!T)
        {
            Screen.SetResolution(1920, 1080, false);
        }
        else
        {
            Screen.SetResolution(Screen.width, (Screen.width*9)/16, true);
        }
        PlayerPrefs.SetInt("FullScreen", T ? 1 : 0);
    }
    public void LanguageDropdown(int idx)
    {
        if (isChanging) return;
        StartCoroutine(LocaleChange(idx));
        LanguageDD.value = idx;
        PlayerPrefs.SetInt("Language", idx);
    }
    IEnumerator LocaleChange(int index)
    {
        isChanging = true;

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        isChanging = false;
    }
    bool isChanging = false;


    private void ClickSound()
    {
        if (ButtonFX != null)
        ButtonFX.Play();
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        InitSetting();
    }
    public void ToMenu()
    {
        StartCoroutine(LoadScene(0));
    }

    public void ToRoom1()
    {
        StartCoroutine(LoadScene(1));
    }

    public void ToRoom2()
    {
        InvenManager.SaveInven();
        StartCoroutine(LoadScene(2));
    }

    public void SaveInven()
    {
        InvenManager.SaveInven();
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
                nextScene = "TitleScene";
                break;
            case 1:
                nextScene = "2hBuildTest2";
                break;
            case 2:
                nextScene = "2hRoom2Backup";
                break;
        }

        // if (Idx != 0)
        // {
        //     InvenManager.SaveInven();
        // }

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
        InitSetting();
        if (Idx == 2)
        {
            InvenManager.LoadInven();
            Debug.Log("Inven Load called!");
        }
        if (timer < minimumLoadingTime)
        {
            yield return new WaitForSeconds(minimumLoadingTime - timer);
        }

        
        Destroy(Loading);


    }

}
