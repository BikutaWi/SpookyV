using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class called when the lever is pulled
/// </summary>
public class KitchenPuzzle : MonoBehaviour
{
    private const float LEVER_POS = -0.45f;
    private const int ITEM_IN_PUZZLE = 4;
    private const string ERROR_SOUND = "error";
    private const string SUCCESS_SOUND = "success";

    //list of item
    public List<bool> allItemsInPlace;
    public UnityEvent OpenDoors;

    
    /// <summary>
    /// Check if all items are in the right place
    /// </summary>
    public void CheckAnswer()
    {
        if(allItemsInPlace.Count >= ITEM_IN_PUZZLE)
        {
            FindObjectOfType<SoundManager>().Play(SUCCESS_SOUND);
            OpenDoors.Invoke();
        }
        else
        {
            FindObjectOfType<SoundManager>().Play(ERROR_SOUND);
        }
    }

   
    /// <summary>
    /// Add elements on the list if they are in the right place
    /// </summary>
    public void AddElementToList()
    {
        allItemsInPlace.Add(true);
    }

    /// <summary>
    /// When the player pulled the lever at the LEVER_POS, check if the puzzle is resolved
    /// </summary>
    private void LateUpdate()
    {
        if(transform.rotation.z <= LEVER_POS)
        {
            CheckAnswer();
        }
    }

}
