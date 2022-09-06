using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class use for the tutorial scene
/// </summary>
public class Tutorial : MonoBehaviour
{
    private const string SCENE_MENU_NAME = "MainMenu";
    private const string UI_BUTTON_SOUND = "ui_button";

    private int index = 0;

    public Image currentTutoImage;
    public List<Sprite> tutorialImagesList;

    /// <summary>
    /// Play the sound of the button when user click
    /// </summary>
    private void PlayButtonSound()
    {
        FindObjectOfType<SoundManager>().Play(UI_BUTTON_SOUND);
    }

    /// <summary>
    /// If the player press Next button
    /// </summary>
    public void NextImage()
    {
        PlayButtonSound();
       if(index == tutorialImagesList.Count -1)
        {
            index = 0;
            currentTutoImage.sprite = tutorialImagesList[index];
        }
       else
        {
            index++;
            currentTutoImage.sprite = tutorialImagesList[index];
        }
    }

    /// <summary>
    /// If the player press Previous button
    /// </summary>
    public void PreviousImage()
    {
        PlayButtonSound();
        if (index == 0)
        {
            index = tutorialImagesList.Count;
            currentTutoImage.sprite = tutorialImagesList[index];
        }
        else
        {
            index--;
            currentTutoImage.sprite = tutorialImagesList[index];
        }
    }

    /// <summary>
    /// If the player press Main Menu button
    /// </summary>
    public void BackToMenu()
    {
        PlayButtonSound();
        SceneManager.LoadScene(SCENE_MENU_NAME);
    }
}
