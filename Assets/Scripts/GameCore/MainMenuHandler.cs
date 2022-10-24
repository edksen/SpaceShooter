using System;
using SpaceShooter.GameCore.UI;

namespace SpaceShooter.GameCore
{
    public class MainMenuHandler
    {
        private MainMenu _mainMenu;

        public MainMenuHandler(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
        }

        public void InitWindow(int score, Action onGameStart, Action onGameExit)
        {
            const string scorePrefix = "High Score:";
            _mainMenu.ScoreText.text = $"{scorePrefix} {score}";
            _mainMenu.OnGameStart += onGameStart;
            _mainMenu.OnGameExit += onGameExit;
        }

        public void ShowMainMenu()
        {
            _mainMenu.gameObject.SetActive(true);
        }

        public void HideMainMenu()
        {
            _mainMenu.gameObject.SetActive(false);
        }

        public void UpdateScore(int score)
        {
            MakeLooseText(score);
        }

        private void MakeLooseText(int score)
        {
            const string message = "You lose, sorry. Want to start from the top?";
            const string scorePrefix = "Your Score:";
            
            _mainMenu.ScoreText.text = $"{message}\n\r{scorePrefix} {score}";
        }
    }
}