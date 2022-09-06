using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which show the text when the treasure is robbed
/// </summary>
public class GoldCoinText : MonoBehaviour
{
    private const float WAIT_TIME = 10f;

    /// <summary>
    /// When the gameobject is active
    /// </summary>
    private void OnEnable()
    {
        StartCoroutine(WaitBeforeDisable());
    }

    /// <summary>
    /// Coroutine which wait before do something
    /// </summary>
    /// <returns>Return the time to wait to pass the Coroutine</returns>

    IEnumerator WaitBeforeDisable()
    {
        yield return new WaitForSeconds(WAIT_TIME);
        this.gameObject.SetActive(false);
    }
}
