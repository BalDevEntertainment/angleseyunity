using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public ParticleSystem dust;
    private Animator animator;
    private List<IPlayerStatusListener> playerStatusListeners = new List<IPlayerStatusListener>();

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
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
            NotifyListenersThatPlayerStatusChanged(PlayerStatus.Idle);
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
