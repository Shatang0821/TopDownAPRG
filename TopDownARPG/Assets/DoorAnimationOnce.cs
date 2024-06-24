using System.Collections;
using UnityEngine;

public class DoornimationOnce : MonoBehaviour
{
    private Animator anim;
    private int animTriggerHash;
    private bool hasPlayed;

    private void Start()
    {
        anim = GetComponent<Animator>();
        animTriggerHash = Animator.StringToHash("Door Animation");
        anim.SetTrigger(animTriggerHash);

        hasPlayed = false;
    }

    private void Update()
    {
        if (!hasPlayed && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            anim.enabled = false;
            hasPlayed = true;
        }
    }
}