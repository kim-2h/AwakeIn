using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseManager : MonoBehaviour
{
    public Canvas PauseCanvas, SettingCanvas;
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        this.gameObject.SetActive(true);
        PauseCanvas.enabled = false;
    }

    public void PauseGame()
    {
        PauseCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void ExitPause()
    {
        PauseCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1;
    }

    public void NoteClicied()
    {
        PauseCanvas.enabled = false;

    }

    public void SettingClicked()
    {

        GameObject[] CCAnvas = GameObject.FindGameObjectsWithTag("Setting");
        SettingCanvas = CCAnvas[0].GetComponent<Canvas>(); 
        Debug.Log("Setting object num: " + CCAnvas.Length);

        SettingCanvas.GetComponent<UISetting>().InitSetting();
        PauseCanvas.enabled = false;
        SettingCanvas.enabled = true;
    }

    public void ExitClicked()
    {
        Debug.Log("Exit");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

    }
    


}
