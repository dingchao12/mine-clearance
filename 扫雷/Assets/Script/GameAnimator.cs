using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAnimator : MonoBehaviour {

    public AudioClip audioClip;

    private Animator animator;
    private AudioSource audioSource;
	void Start ()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAnimator()
    {
        animator.Play("Animation",0);
    }

    public void PlayBgAnimator()
    {
        animator.Play("AnimationBG", 0);
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
