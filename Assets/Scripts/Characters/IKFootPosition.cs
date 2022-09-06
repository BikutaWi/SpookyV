using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class link to the feet of the enemies in order to touch the floor everytime
/// </summary>
public class IKFootPosition : MonoBehaviour
{
    private const string FLOOR_TAG_NAME = "Ground";

    private RaycastHit hit;
    private Ray ray;
    private Animator animator;

    
    [Range (0, 1f)]
    public float distanceToGround;

    public LayerMask layerMask;

    
    /// <summary>
    /// When the game is started, get the animator
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    /// <summary>
    /// Every frame if animator is IK pass enabled
    /// </summary>
    /// <param name="layerIndex">ID of the layer</param>
    private void OnAnimatorIK(int layerIndex)
    {
        if(animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            
            // Touch the ground with the left foot
            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if(Physics.Raycast(ray, out hit, distanceToGround + 1f, layerMask))
            {
                if(hit.transform.tag == FLOOR_TAG_NAME)
                {
                    Vector3 foot = hit.point;
                    foot.y += distanceToGround;
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, foot);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }

            // Touch the ground with the right foot
            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, layerMask))
            {
                if (hit.transform.tag == FLOOR_TAG_NAME)
                {
                    Vector3 foot = hit.point;
                    foot.y += distanceToGround;
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, foot);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }
        }
    }
}
