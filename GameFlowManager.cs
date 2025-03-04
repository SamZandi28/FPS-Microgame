﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.FPS.Game
{
    public class GameFlowManager : MonoBehaviour
    {
        [Header("Parameters")]
        [Tooltip("Duration of the fade-to-black at the end of the game")]
        public float EndSceneLoadDelay = 3f;

        [Tooltip("The canvas group of the fade-to-black screen")]
        public CanvasGroup EndGameFadeCanvasGroup;

        [Header("Win")]
        [Tooltip("This string has to be the name of the scene you want to load when winning")]
        public string WinSceneName = "WinScene";

        [Tooltip("Duration of delay before the fade-to-black, if winning")]
        public float DelayBeforeFadeToBlack = 4f;

        [Tooltip("Win game message")]
        public string WinGameMessage;
        [Tooltip("Duration of delay before the win message")]
        public float DelayBeforeWinMessage = 2f;

        [Tooltip("Sound played on win")] public AudioClip VictorySound;

        [Header("Lose")]
        [Tooltip("This string has to be the name of the scene you want to load when losing")]
        public string LoseSceneName = "LoseScene";


        public bool GameIsEnding { get; private set; }

        float m_TimeLoadEndGameScene;
        string m_SceneToLoad;

        void Awake()
        {
            EventManager.AddListener<AllObjectivesCompletedEvent>(OnAllObjectivesCompleted);
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
        }

        void Start()
        {
            AudioUtility.SetMasterVolume(1);
        }

        void Update()
        {
            if (GameIsEnding)
            {
                float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / EndSceneLoadDelay;
                EndGameFadeCanvasGroup.alpha = timeRatio;

                AudioUtility.SetMasterVolume(1 - timeRatio);

                // See if it's time to load the end scene (after the delay)
                if (Time.time >= m_TimeLoadEndGameScene)
                {
                    SceneManager.LoadScene(m_SceneToLoad);
                    GameIsEnding = false;
                }
            }
        }

        void OnAllObjectivesCompleted(AllObjectivesCompletedEvent evt)
        {
            LoadNextLevel();
        }

        void OnPlayerDeath(PlayerDeathEvent evt) => EndGame(false);

        void LoadNextLevel()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // Check if the next scene index is within the bounds of your build settings
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                // If no more scenes, go to WinScene or an end screen
                SceneManager.LoadScene(WinSceneName);
            }
        }


        void EndGame(bool win)
        {
            if (!win)
            {
                GameIsEnding = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                EndGameFadeCanvasGroup.gameObject.SetActive(true);
                m_SceneToLoad = LoseSceneName;
                m_TimeLoadEndGameScene = Time.time + EndSceneLoadDelay;
            }
            else if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                // Only end game if this is the last level
                GameIsEnding = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                EndGameFadeCanvasGroup.gameObject.SetActive(true);
                m_SceneToLoad = WinSceneName;
                m_TimeLoadEndGameScene = Time.time + EndSceneLoadDelay + DelayBeforeFadeToBlack;
            }
        }


        void OnDestroy()
        {
            EventManager.RemoveListener<AllObjectivesCompletedEvent>(OnAllObjectivesCompleted);
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
        }
    }
}
