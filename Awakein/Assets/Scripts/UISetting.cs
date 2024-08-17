using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
//using UnityEditor.Localization.Editor;
public class UISetting : MonoBehaviour
{
    public Canvas SettingCanvas;
    public Camera cam;
    [SerializeField] private Button Selected;
    public Button SoundBT, GraphicBT, LanguageBT, ScreenBT;

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


        SoundBT.onClick.AddListener(() => SelectedButton(SoundBT));
        GraphicBT.onClick.AddListener(() => SelectedButton(GraphicBT));
        LanguageBT.onClick.AddListener(() => SelectedButton(LanguageBT));
        ScreenBT.onClick.AddListener(() => SelectedButton(ScreenBT));

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

        SoundBT.transform.GetChild(1).gameObject.SetActive(true);
        SoundBT.transform.GetChild(2).gameObject.SetActive(true);

    }
    public void InitSetting()
    {
        SettingCanvas.enabled = true;
        Selected = SoundBT;
        Selected.transform.GetChild(1).gameObject.SetActive(true);
        Selected.transform.GetChild(2).gameObject.SetActive(true);

        Master.value = volume[0];
        BGM.value = volume[1];
        SFX.value = volume[2];

        GraphicBT.transform.GetChild(1).gameObject.SetActive(false);
        GraphicBT.transform.GetChild(2).gameObject.SetActive(false);

        LanguageBT.transform.GetChild(1).gameObject.SetActive(false);
        LanguageBT.transform.GetChild(2).gameObject.SetActive(false);
    
        ScreenBT.transform.GetChild(1).gameObject.SetActive(false);
        ScreenBT.transform.GetChild(2).gameObject.SetActive(false);

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
    private void SelectedButton(Button BT)
    {
        Debug.Log(BT.name);
        if (BT != Selected) 
        {
            ClickSound();

            Selected.transform.GetChild(1).gameObject.SetActive(false);
            Selected.transform.GetChild(2).gameObject.SetActive(false);
            BT.transform.GetChild(1).gameObject.SetActive(true);
            BT.transform.GetChild(2).gameObject.SetActive(true);
            Selected = BT;
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
