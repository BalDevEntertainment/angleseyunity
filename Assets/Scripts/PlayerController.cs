using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public ParticleSystem dust;
    private Animator animator;
    private List<IPlayerStatusListener> playerStatusListeners = new List<IPlayerStatusListener>();
    private AudioSource attackSound;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        attackSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            NotifyListenersThatPlayerStatusChanged(PlayerStatus.Walking);
        }
    }

    public void UpdateState(string state = null)
    {
        if (state != null)
        {
            animator.Play(state);
        }
    }

    public void AddPlayerStatusListener(IPlayerStatusListener playerStatusListener)
    {
        this.playerStatusListeners.Add(playerStatusListener);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            UpdateState("PlayerFighting");
            NotifyListenersThatPlayerStatusChanged(PlayerStatus.Fighting);
            attackSound.Play();
            attackSound.loop = true;
        }
    }

    private void NotifyListenersThatPlayerStatusChanged(PlayerStatus status)
    {
        foreach (var listener in playerStatusListeners)
        {
            listener.OnStatusChanged(status);
        }
    }

    void DustPlay()
    {
        dust.Play();
    }

    void DustStop()
    {
        dust.Stop();
    }
}
