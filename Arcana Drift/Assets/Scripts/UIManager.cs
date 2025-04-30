using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player")] 
    public GameObject player;
    
    [Header("Panels")]
    public GameObject gameplayUI;
    public GameObject pauseMenu;
    public GameObject controlsMenu;
    
    [Header("Settings")] 
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityText;
    
    [Header("GamePlayUI")] 
    public Image handleImage;
    
    private void Start()
    {
        handleImage.color = new Color(1f, 1f, 1f, 1f);
        sensitivitySlider.value = GameManager.Instance.sensitivity;
        sensitivityText.text = "" + GameManager.Instance.sensitivity;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameplayUI.activeSelf)
                PauseGame();
            else if(pauseMenu.activeSelf)
                UnPauseGame();
            else if(controlsMenu.activeSelf)
                BackToPauseMenu();
        }   
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        player.SetActive(false);
        gameplayUI.SetActive(false);
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        player.SetActive(true);
        pauseMenu.SetActive(false);
        gameplayUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Controls()
    {
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }
    
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void BackToPauseMenu()
    {
        controlsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
    
    public void UpdateSensitivity()
    {
        GameManager.Instance.sensitivity = sensitivitySlider.value;
        sensitivityText.text = "" + (int)sensitivitySlider.value;
    }
}
