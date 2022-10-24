using UnityEngine;

namespace SpaceShooter.GameCore
{
    public class Score
    {
        private const string PREFS_PREFIX = "Player_Score";

        private int _currentScore;
        private int _maxScore;

        public int CurrentScore => _currentScore;

        public Score()
        {
            _currentScore = 0;
            GetHighScore();
        }

        public void IncreaseScore()
        {
            ++_currentScore;
        }

        public void ResetScore()
        {
            SaveScore();
            _currentScore = 0;
        }

        public int GetHighScore()
        {
            if(_maxScore == 0)
                GetHighScoreFromPrefs();

            return _maxScore;
        }
        
        public void SaveScore()
        {
            if(_currentScore > GetHighScore())
                SaveScoreToPrefs(_currentScore);
        }

        private void GetHighScoreFromPrefs()
        {
            _maxScore = PlayerPrefs.GetInt(PREFS_PREFIX);
        }

        private void SaveScoreToPrefs(int score)
        {
            _maxScore = score;
            PlayerPrefs.SetInt(PREFS_PREFIX, score);
        }
    }
}