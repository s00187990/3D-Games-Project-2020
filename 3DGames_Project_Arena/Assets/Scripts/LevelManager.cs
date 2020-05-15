using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Started,
    Playing,
    Paused,
    GameOver
}

public delegate void EnemyDied(GameObject gameObject);
public class LevelManager : MonoBehaviour
{
    public static GameState currentGameState;

    public AudioClip[] AudioClips;
    public Transform playerSpawnPosition;
    public bool MusicOn;
    public GameObject MainBoss;
    public GameObject startedPanel;
    public GameObject pausedPanel;
    public GameObject PlayerHUD;
    public GameObject YouDiedScreen;

    GameObject Player;
    int currentTrackIndex;
    AudioSource Source;
    bool paused = false;
    public static event EnemyDied OnEnemyDied;
    private void OnEnable()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Source = GetComponent<AudioSource>();
        currentGameState = GameState.Started;
        // for now start puased
        YouDiedScreen.SetActive(false);
        PlayerHUD.SetActive(false);
        pausedPanel.SetActive(false);
        startedPanel.SetActive(true);
        Pause();
        Player.transform.position = playerSpawnPosition.position;

        Player.GetComponent<Health>().OnDeath += LevelManager_OnPlayerDeath;
        MainBoss.GetComponent<Health>().OnDeath += LevelManager_OnBossDeath;

    }

    private void LevelManager_OnPlayerDeath()
    {
        currentGameState = GameState.GameOver;
        YouDiedScreen.SetActive(true);
    }

    private void LevelManager_OnBossDeath()
    {
        // show you win screen 
        currentGameState = GameState.GameOver;
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("YouWin");
        //SceneManager.SetActiveScene("YouWin");
    }

    private void Update()
    {
        if (MusicOn)
        {
            if (!Source.isPlaying || Source.clip == null)
                UpdateSourceAndPlay();
        }

        switch (currentGameState)
        {
            case GameState.Started:
                // show help UI
                if (Input.anyKey)
                {
                    Pause();
                    currentGameState = GameState.Playing;
                    startedPanel.SetActive(false);
                    PlayerHUD.SetActive(true);
                }
                break;
            case GameState.Playing:
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    Pause();
                    pausedPanel.SetActive(paused);
                    PlayerHUD.SetActive(!paused);
                    Cursor.visible = paused;
                    Cursor.lockState = !paused ? CursorLockMode.Locked : CursorLockMode.None;
                }
                break;
            case GameState.GameOver:
                // play something or do nothing and show retry or exit ui 
                PlayerHUD.SetActive(false);
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                MusicOn = false;
                Source.Stop();
                break;
        }

    }

    private void Pause()
    {
        // stop time
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        // show or hide ui

    }

    private void UpdateSourceAndPlay()
    {
        Source.clip = AudioClips[currentTrackIndex];
        Source.Play();
        currentTrackIndex++;
        if (currentTrackIndex >= AudioClips.Length) currentTrackIndex = 0;
    }

    public static void IDied(GameObject gameObject)
    {
        OnEnemyDied?.Invoke(gameObject);
    }


    //private void OnDisable()
    //{
    //    if (Player.gameObject != null || MainBoss.gameObject != null)
    //    {
    //        Player.GetComponent<Health>().OnDeath -= LevelManager_OnPlayerDeath;
    //        MainBoss.GetComponent<Health>().OnDeath -= LevelManager_OnBossDeath;
    //    }
    //}
}
