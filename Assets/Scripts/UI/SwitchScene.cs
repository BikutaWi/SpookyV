using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for scene change
/// </summary>
public class SwitchScene : MonoBehaviour
{
    private const string UI_BUTTON_SOUND = "ui_button";

    private const float TIME_BEFORE_SWITCH = 10f;

    private bool switchHasBeenActivated = false;

    public string sceneName;
    public bool autoSwitch = false;
   
    /// <summary>
    /// Methods call every frame which change scene if autoSwitch is true
    /// </summary>
    private void Update()
    {
        if(autoSwitch && !switchHasBeenActivated)
        {
            switchHasBeenActivated = true;
            StartCoroutine("WaitBeforeSwitchScene");
        }
    }

    /// <summary>
    /// Play the sound of the button when player click
    /// </summary>
    private void PlayButtonSound()
    {
        FindObjectOfType<SoundManager>().Play(UI_BUTTON_SOUND);
    }

    /// <summary>
    /// Load a scene
    /// </summary>
    public void SceneLoader()
    {
        PlayButtonSound();
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Reload a scene (GameOver)
    /// </summary>
    public void ReloadScene()
    {
        PlayButtonSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// If the player want to quit the game
    /// </summary>
    public void QuitGame()
    {
        PlayButtonSound();
        Application.Quit();
    }

    /// <summary>
    /// Coroutine which wait before witch scene
    /// </summary>
    /// <returns>Wait for the specified time</returns>
    IEnumerator WaitBeforeSwitchScene()
    {
        yield return new WaitForSeconds(TIME_BEFORE_SWITCH);
        SceneManager.LoadScene(sceneName);
    }
}
