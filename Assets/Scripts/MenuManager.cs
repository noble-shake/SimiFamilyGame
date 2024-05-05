using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject MenuUI;
    [SerializeField] GameObject IntroduceUI;
    [SerializeField] GameObject Images;


    public void onStartBtn() {
        SceneManager.LoadSceneAsync("GameScene");
    }


    public void onIntroduceBtn()
    {
        MenuUI.SetActive(false);
        IntroduceUI.SetActive(true);
        Images.SetActive(false);
    }

    public void OnIntroduceBackBtn() {
        MenuUI.SetActive(true);
        IntroduceUI.SetActive(false);
        Images.SetActive(true);
    }

    public void onExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        // SetResolution();
        MenuUI.SetActive(true);
        IntroduceUI.SetActive(false);
        
    }

    private void SetResolution()
    {
        int setWidth = 1280;
        int setHeight= 760;

        Screen.SetResolution(setWidth, setHeight, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }





}
