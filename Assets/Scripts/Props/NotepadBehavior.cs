using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class linked to the notepads
/// </summary>
public class NotepadBehavior : MonoBehaviour
{
    private const float WRITING_SPEED = 0.15f;
    private const float DELAY_BEFORE_DESTROY = 5f;
    private const string WRITING_SOUND = "writing";
    private const string FIRE_SOUND = "fire_sound";

    private bool hasBeenSelectedOnce = false;

    public Text notepadText;
    public string text;
    public GameObject fireEffect;

    public GameObject nextNotepad;

   

    /// <summary>
    /// Called when pick up the clipboard for the first time
    /// </summary>
    /// 
    public void TextAnimation()
    {
        if(!hasBeenSelectedOnce)
        {
            StartCoroutine("StartTextAnimation");
            
        }

        if(nextNotepad != null)
        {
            nextNotepad.SetActive(true);
        }
    }

    /// <summary>
    /// Coroutine which will animate the text on the notepad and call the fire
    /// </summary>
    /// <returns>Return the time before write another character</returns>
    IEnumerator StartTextAnimation()
    {
        hasBeenSelectedOnce = true;
        FindObjectOfType<SoundManager>().Play(WRITING_SOUND);
        for (int i = 0; i <= text.Length; i++)
        {
           notepadText.text = text.Substring(0, i);
           yield return new WaitForSeconds(WRITING_SPEED);
        }
        FindObjectOfType<SoundManager>().Stop(WRITING_SOUND);

        StartCoroutine("NotepadFire");
        
    }

    /// <summary>
    /// Put the notepad on fire and disable it after x seconds
    /// </summary>
    /// <returns>Return the time before destroying the notepad</returns>
    IEnumerator NotepadFire()
    {
        FindObjectOfType<SoundManager>().Play(FIRE_SOUND);
        fireEffect.SetActive(true);
        yield return new WaitForSeconds(DELAY_BEFORE_DESTROY);
        Destroy(this.gameObject);
        FindObjectOfType<SoundManager>().Stop(FIRE_SOUND);
    }


}
