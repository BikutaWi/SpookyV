using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Class which check if the fresco is resolved
/// </summary>
public class FrescoResolve : MonoBehaviour
{
    private const float DELAY_BEFORE_RESOLVE = 3f;
    private const string SUCCESS_SOUND = "success";
    private const string ERROR_SOUND = "error";

    private bool hasBeenResolved = false;

    private int inRightPlace = 0;

    public UnityEvent PuzzleResolved;
    
    /// <summary>
    /// Check if all pieces are at the good orientation
    /// </summary>
    public void CheckFrescoSolution()
    {
        StartCoroutine(WaitBeforeAnswer());

        if(!hasBeenResolved)
        {
            foreach (Transform child in transform)
            {
                if (child.localRotation.eulerAngles.x != 0 || child.localRotation.eulerAngles.x % 360 != 0)
                {
                    FindObjectOfType<SoundManager>().Play(ERROR_SOUND);
                    return;
                }
                else
                {
                    inRightPlace++;
                }
            }

            if(inRightPlace == transform.childCount)
            {
                FindObjectOfType<SoundManager>().Play(SUCCESS_SOUND);
                PuzzleResolved.Invoke();
                hasBeenResolved = true;
            }
        }
    }


    /// <summary>
    /// Coroutine which wait before fresco verification (avoid bogs)
    /// </summary>
    /// <returns>Return the time before passing the coroutine</returns>
    IEnumerator WaitBeforeAnswer()
    {
        yield return new WaitForSeconds(DELAY_BEFORE_RESOLVE);
    }
}
