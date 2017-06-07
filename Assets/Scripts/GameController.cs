using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour, IPlayerStatusListener
{
    public float defaultParallaxSpeed = 0.05f;
    public RawImage background;
    public RawImage platform;
    public enum GameState { Idle, Playing };
    public GameState gameState = GameState.Idle;
    public GameObject uiIdle;
    public Camera mainCamera;

    public PlayerController player;
    public GameObject enemyGenerator;

    private float parallaxSpeed;
    private Dictionary<PlayerStatus, Func<PlayerStatusStrategy>> playerStatusStrategyDictionary = new Dictionary<PlayerStatus, Func<PlayerStatusStrategy>>();

    // Use this for initialization
    void Start()
    {
        parallaxSpeed = defaultParallaxSpeed;
        InitializePlayerStatusStrategyDictionary();
        player.AddPlayerStatusListener(this);
    }

    // Update is called once per frame
    void Update()
    {

        if (ShouldStartGame())
        {
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            player.SendMessage("UpdateState", "PlayerRun");
            enemyGenerator.SendMessage("StartGenerator");
            player.SendMessage("DustPlay");
        }
        else if (gameState == GameState.Playing)
        {
            Parallax();
        }
    }

    private bool ShouldStartGame()
    {
        return gameState == GameState.Idle && (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0));
    }

    private void Parallax()
    {
        float finalSpeed = parallaxSpeed * Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 6, 0f, 1f, 1f);
    }

    private void InitializePlayerStatusStrategyDictionary()
    {
        playerStatusStrategyDictionary.Add(PlayerStatus.Idle, () => new IdleStatusStrategy());
        playerStatusStrategyDictionary.Add(PlayerStatus.Walking, () => new WalkingStatusStrategy());
        playerStatusStrategyDictionary.Add(PlayerStatus.Fighting, () => new FightingStatusStrategy());
    }

    public void OnStatusChanged(PlayerStatus playerStatus)
    {
        Func<PlayerStatusStrategy> strategy;
        playerStatusStrategyDictionary.TryGetValue(playerStatus, out strategy);
        strategy().Execute(this);
    }

    public interface PlayerStatusStrategy
    {
        void Execute(GameController gameController);
    }

    private class IdleStatusStrategy : PlayerStatusStrategy
    {
        public void Execute(GameController gameController)
        {
            gameController.parallaxSpeed = 0;
            gameController.mainCamera.GetComponent<CameraShake>().enabled = false;
        }
    }

    private class WalkingStatusStrategy : PlayerStatusStrategy
    {
        public void Execute(GameController gameController)
        {
            gameController.parallaxSpeed = gameController.defaultParallaxSpeed;
            gameController.mainCamera.GetComponent<CameraShake>().enabled = false;
        }
    }

    private class FightingStatusStrategy : PlayerStatusStrategy
    {
        public void Execute(GameController gameController)
        {
            gameController.parallaxSpeed = 0;
            gameController.mainCamera.GetComponent<CameraShake>().enabled = true;
        }
    }
}
