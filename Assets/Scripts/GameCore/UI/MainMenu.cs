using System;
using TMPro;
using UnityEngine;

namespace SpaceShooter.GameCore.UI
{
    public class MainMenu : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;
        
        public event Action OnGameStart;
        public event Action OnGameExit;

        public void StartGame()
        {
            OnGameStart?.Invoke();
        }

        public void ExitGame()
        {
            OnGameExit?.Invoke();
        }
    }
}