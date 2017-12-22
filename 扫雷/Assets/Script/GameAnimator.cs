using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAnimator : MonoBehaviour {

    private Animator animator;

	void Start ()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimator()
    {
        animator.Play("Animation",0);
    }

    public void PlayBgAnimator()
    {
        animator.Play("AnimationBG", 0);
    }
}
