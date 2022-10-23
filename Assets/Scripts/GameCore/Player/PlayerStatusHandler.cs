using System;
using TMPro;
using UnityEngine;

namespace SpaceShooter.GameCore
{
    public class PlayerStatusHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _sore;
        [SerializeField] private TextMeshProUGUI _coordinates;
        [SerializeField] private TextMeshProUGUI _rotation;
        [SerializeField] private TextMeshProUGUI _currentSpeed;
        [SerializeField] private TextMeshProUGUI _laserCapacity;
        [SerializeField] private TextMeshProUGUI _laserCooldown;

        public void UpdateLaserInfo(int laserCapacity, float laserCooldown)
        {
            _laserCooldown.text = Math.Round(laserCooldown, 2).ToString();
            _laserCapacity.text = laserCapacity.ToString();
        }

        public void UpdateMovementInfo(Vector2 coordinates, float angle, float currentSpeed)
        {
            _coordinates.text = coordinates.ToString();
            _rotation.text = Mathf.RoundToInt(360f - angle).ToString();
            _currentSpeed.text = Math.Round(currentSpeed, 2).ToString();
        }

        public void UpdateScore(int score)
        {
            _sore.text = score.ToString();
        }
    }
}