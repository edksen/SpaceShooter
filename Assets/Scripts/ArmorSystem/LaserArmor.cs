using System.Threading.Tasks;
using SpaceShooter.ArmorSystem.Contracts;
using UnityEngine;

namespace SpaceShooter.ArmorSystem
{
    public class LaserArmor : Armor
    {
        public override int AmmoLeft => _currentShots;
        
        private readonly int _maxShots;
        private int _currentShots;
        private readonly float _cooldownRate;
        private bool _laserOn;
        private bool _recharging;

        public LaserArmor(Projectile projectile, Transform armoryPosition, int maxShots, float cooldownRate) : base(
            projectile, armoryPosition, ArmorType.Static)
        {
            _maxShots = maxShots;
            _currentShots = _maxShots;
            _cooldownRate = cooldownRate;
            _laserOn = false;
            _recharging = false;
        }

        internal override void MakeShot()
        {
            if(_currentShots == 0 || _laserOn)
                return;

            --_currentShots;
            _laserOn = true;
            var projectile = Object.Instantiate(_projectile, _armorTransform);
            projectile.OnDestroyEntity += () => _laserOn = false;

            if (!_recharging)
                Recharging();
        }

        private async Task Recharging()
        {
            _recharging = true;
            while (_currentShots != _maxShots)
            {
                await Task.Delay((int)(_cooldownRate * 1000));
                ++_currentShots;
            }

            _recharging = false;
        }
    }
}