using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    public bool isPaused = false;

    [Header("UI")]
    public GameObject canvasPaused;
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;


    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();


        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private void Awake()
    {
        instance = this;
    }

    //Cambia el timeScale y el isPaused en funcion del parametro que se le pase
    //Cuando _pause es true, el juego pasa a estar pausado y cuando es false se quita la pausa
    public void Pause(bool _pause)
    {
        isPaused = _pause;
        if(_pause == true)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    //Activa/desactiva la pausa si se le da a escape, y muestra u oculta el cursor en funcion de si esta pausado o no
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            Pause(!isPaused);
            canvasPaused.SetActive(true);
        }
        if(isPaused == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!isPaused)
        {
            canvasPaused.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void MenuGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
