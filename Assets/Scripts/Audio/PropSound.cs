using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which play prop sounds !
/// </summary>
public class PropSound : MonoBehaviour
{
    private string KEY_SOUND = "grab_key";
    private string COIN_SOUND = "coin";
    private string KEY_TWIST_SOUND = "key_twist";


    public void PlayKeySound()
    {
        FindObjectOfType<SoundManager>().Play(KEY_SOUND);
    }

    public void PlayCoinSound()
    {
        FindObjectOfType<SoundManager>().Play(COIN_SOUND);
    }

    public void PlayKeyTwist()
    {
        FindObjectOfType<SoundManager>().Play(KEY_TWIST_SOUND);
    }


}
