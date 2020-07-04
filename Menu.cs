using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEditor.ShaderGraph.Internal;
/// <summary>
/// 
/// </summary>
public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioMixer audioMixer;

    public void PlayGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {

        Application.Quit();
    }

    public void UIEnable()
    {
        GameObject.Find("Canvas/menu/UI").SetActive(true);
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("MainVol",value);
    }
}
