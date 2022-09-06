using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRHands : MonoBehaviour
{
    private GameObject handInstantiation;
    private Animator animatorHands;

    public GameObject handPrefab;
    public ActionBasedController device;

    public DetectHandMovement hand;

    /// <summary>
    /// When the game is started
    /// </summary>
    void Awake()
    {
        // instantiante hands
        handInstantiation =  Instantiate(handPrefab, transform);

        // get the animator of the hands
        animatorHands = handInstantiation.GetComponent<Animator>();

        // Find the hands in the scene
        if(this.gameObject.tag == "left")
        {
            hand = GameObject.FindGameObjectWithTag("left_hand").GetComponent<DetectHandMovement>();
        }

        if(this.gameObject.tag == "right")
        {
            hand = GameObject.FindGameObjectWithTag("right_hand").GetComponent<DetectHandMovement>();
        }

    }

    /// <summary>
    /// At every frame, detect if the player try to grab something or trigger it
    /// </summary>
    private void Update()
    {
        TriggerAnimation();
        GripAnimation();
    }

    /// <summary>
    /// Start the Trigger animation
    /// </summary>
    public void TriggerAnimation()
    {
        animatorHands.SetFloat("trigger", hand.device.activateActionValue.action.ReadValue<float>());
    }

    /// <summary>
    /// Start the Grip animation
    /// </summary>
    public void GripAnimation()
    {
        animatorHands.SetFloat("grip", hand.device.selectActionValue.action.ReadValue<float>());
    }
}
