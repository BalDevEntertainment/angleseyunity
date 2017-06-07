using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem dust;
    public Slider lifeBar;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && CanBeDamaged(collision))
        {
            StartFight(collision.gameObject.GetComponent<DamageTarget>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CanBeDamaged(collision))
        {
            StopFight(collision);
        }
    }

    private static bool CanBeDamaged(Collider2D collision)
    {
        return collision.gameObject.GetComponent<DamageTarget>() != null;
    }

    private void StopFight(Collider2D collision)
    {
        collision.gameObject.GetComponent<DamageTarget>().OnStopReceivingDamage();
        UpdateState("PlayerRun");
        NotifyListenersThatPlayerStatusChanged(PlayerStatus.Walking);
        attackSound.Stop();
    }

    private void StartFight(IDamageTarget damageTarget)
    {
        UpdateState("PlayerFighting");
        NotifyListenersThatPlayerStatusChanged(PlayerStatus.Fighting);
        attackSound.Play();
        attackSound.loop = true;
        damageTarget.OnStartReceivingDamage(10);
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
