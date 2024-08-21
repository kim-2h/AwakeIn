using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.EventSystems;
//using UnityEditor.Localization.Editor;
public class UISetting : MonoBehaviour
{
    public Canvas SettingCanvas;
    public Camera cam;
    [SerializeField] private Button Selected;
    private int SelectedIndex = 0;
    public Button SoundBT, GraphicBT, LanguageBT, ScreenBT;

    public GameObject[][] ButtonContents = new GameObject[4][];
    public GameObject[] SoundsChild;
    public GameObject[] GraphicsChild;
    public GameObject[] LanguagesChild;
    public GameObject[] ScreensChild;
    


    void Awake()
    {
        SetRes();
        OnPreCull();
        
        this.gameObject.SetActive(true);

        if (LocalizationSettings.SelectedLocale != LocalizationSettings.AvailableLocales.Locales[0])
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            StartCoroutine(LocaleChange(0));
        }


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

        FullScreen(true);
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SettingCanvas.enabled = false;
        Master.value = 0.6f; BGM.value = 0.6f; SFX.value = 0.6f;
        ScreenToggle.isOn = true;
        ShadowToggle.isOn = true;
        VolumeToggle.isOn = true;

        Selected = SoundBT;
        SelectedIndex = 0;

        //ButtonContents[0][1].gameObject.SetActive(true);
        ButtonContents[0][2].gameObject.SetActive(true);
        ButtonContents[0][0].gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 

    }
    public void InitSetting()
    {
        ButtonContents[0][2].gameObject.SetActive(true);
        SettingCanvas.enabled = true;
        Selected = SoundBT;
        SelectedIndex = 0;
        //ButtonContents[0][1].gameObject.SetActive(true);
        ButtonContents[0][0].gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 
        

        Master.value = volume[0];
        BGM.value = volume[1];
        SFX.value = volume[2];

        ButtonContents[1][1].gameObject.SetActive(false);
        ButtonContents[1][2].gameObject.SetActive(false);

        ButtonContents[2][1].gameObject.SetActive(false);
        ButtonContents[2][2].gameObject.SetActive(false);
    
        ButtonContents[3][1].gameObject.SetActive(false);
        ButtonContents[3][2].gameObject.SetActive(false);

        ScreenToggle.isOn = isFullScreen;
        ShadowToggle.isOn = isShadow;
        VolumeToggle.isOn = isVolume;

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
        Debug.Log("Button name : " + BT.name + " index : " + idx);
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
        Debug.Log("Button name : " + BT.name + " index : " + idx);
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
            Master.value = volume[0];
            BGM.value = volume[1];
            SFX.value = volume[2];
        }
    }
    public Light PointLight;
    public GameObject GVolume;
    public void SetShadow(bool T)
    {
        T = ShadowToggle.isOn;
        PointLight.shadows = T ? LightShadows.Soft : LightShadows.None;
        isShadow = T;
    }
    public void SetGlobalVolume(bool T)
    {
        T = VolumeToggle.isOn;
        GVolume.SetActive(T);
        isVolume = T;
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
    public static float[] volume = new float[3] { 0.6f, 0.6f, 0.6f };
    public static bool isFullScreen = true;
    public static bool isShadow = true;
    public static bool isVolume = true;

    public Slider Master, BGM, SFX;
    public AudioMixer MasterMixer;
    public Toggle ScreenToggle, ShadowToggle, VolumeToggle;
    public AudioSource ButtonFX;

    public void ChangeMaster()
    {
        volume[0] = Master.value;
        MasterMixer.SetFloat("Master", Mathf.Log10(volume[0]) * 30);
    }
    public void ChangeBGM(float Volume)
    {
        volume[1] = BGM.value;
        MasterMixer.SetFloat("BGM", Mathf.Log10(volume[1]) * 40);
    }
    public void ChangeSFX(float Volume)
    {
        volume[2] = SFX.value;
        MasterMixer.SetFloat("SFX", Mathf.Log10(volume[2]) * 30);

    }

    public void FullScreen(bool T) //빌드한 다음에 잘 돌아가는지 확인해야함
    {
        T = ScreenToggle.isOn;
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
    }
    public void LanguageDropdown(int idx)
    {
        if (isChanging) return;
        StartCoroutine(LocaleChange(idx));
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
        ButtonFX.Play();
    }

    public void InitSettingTab()
    {
        Master.value = volume[0];
        BGM.value = volume[1];
        SFX.value = volume[2];
        ScreenToggle.isOn = isFullScreen;
    }

}
