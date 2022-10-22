using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
        [SerializeField] private Button startButton;
        [SerializeField] private Button exitButton;

        private void Start()
        {
                startButton.onClick.AddListener(StartGame);
                exitButton.onClick.AddListener(ExitGame);
        }

        private void StartGame()
        {
                SceneManager.LoadScene(1);
        }

        private void ExitGame()
        {
              Application.Quit();  
        }

        private void OnDestroy()
        {
                startButton.onClick.RemoveListener(StartGame);
                exitButton.onClick.RemoveListener(ExitGame);    
        }
}