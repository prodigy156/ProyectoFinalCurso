using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    enum State
    {
        Play,
        Options,
        Menu,
        Editor
    }
    State nextState;
    State state;
    
    [Header("Options")]
    public AudioMixer audioMixer;


    public GameObject canvasOption;

    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    [Header("Animator")]
    public Animator animator;
    public Animator animatorPlayer;
    float timer;
    float time = 5.5f;

    void Start()
    {
        Time.timeScale = 1f;
        timer = time;
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();


        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        state = State.Menu;
        nextState = State.Menu;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Options)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                canvasOption.SetActive(true);
            }
        }

        if (state == State.Play)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        if (state == State.Editor)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            }
        }


        if (state == State.Menu)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                animatorPlayer.gameObject.SetActive(true);
            }
        }

        if (nextState != state)
        {
            if(nextState == State.Play)
            {
                timer = time;
                animator.SetBool("BoxOpen", true);
                animatorPlayer.gameObject.SetActive(false);
            }
            if(nextState == State.Options)
            {
                timer = time;
                animator.SetBool("BoxOpen", true);
                animatorPlayer.gameObject.SetActive(false);
            }
            if (nextState == State.Menu)
            {
                canvasOption.SetActive(false);
                timer = time;
            }
            if (nextState == State.Editor)
            {
                timer = time;
                animator.SetBool("BoxOpen", true);
                animatorPlayer.gameObject.SetActive(false);
            }
            state = nextState;
        }


    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void setVolume (float _volume)
    {
        audioMixer.SetFloat("Volume", _volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void CloseCanvas()
    {
        animator.SetBool("BoxOpen", false);
        nextState = State.Menu;
    }

    public void PlayGame()
    {
        nextState = State.Play;
    }

    public void OptionGame()
    {
        nextState = State.Options;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void EditorLevel()
    {
        nextState = State.Editor;
    }
}
