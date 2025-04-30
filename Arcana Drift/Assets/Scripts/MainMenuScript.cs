using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour 
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Settings")] 
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityText;
    
    private void Start()
    {
        sensitivitySlider.value = GameManager.Instance.sensitivity;
        sensitivityText.text = "" + GameManager.Instance.sensitivity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(mainMenuPanel.activeSelf)
                QuitGame();
            else if(settingsPanel.activeSelf)
                BackToMainMenu();
        }   
    }

    public void StartGame()
    {
        // Load the first scene (by build index 1)
        SceneManager.LoadScene("RuinedArchives");
    }

    public void OpenSettings()
    {
        // Hide Main Menu, Show Settings
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        // Quits the application
        Application.Quit();

        // This only shows in the editor, so you know it worked.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    public void BackToMainMenu()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void UpdateSensitivity()
    {
        GameManager.Instance.sensitivity = sensitivitySlider.value;
        sensitivityText.text = "" + (int)sensitivitySlider.value;
    }
}