using System;
using System.Threading;
using System.Threading.Tasks;
using ArmorSystem.Contracts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ArmorSystem.Armors
{
    public class LaserArmor : Armor
    {
        protected override event Action<int, float> _onArmorStatusUpdated;
        public override event Action OnProjectileHit;

        protected override int AmmoLeft => _currentShots;
        protected override float AmmoCooldown => _cooldownLeft;

        private readonly int _maxShots;
        private int _currentShots;
        private float _cooldownLeft;
        private readonly float _cooldownRate;
        private bool _laserOn;
        private bool _recharging;
        private CancellationTokenSource _tokenSource;

        public LaserArmor(Projectile projectile, Transform armoryPosition, int maxShots, float cooldownRate) : base(
            projectile, armoryPosition, ArmorType.Laser)
        {
            _maxShots = maxShots;
            _currentShots = _maxShots;
            _cooldownRate = cooldownRate;
            _laserOn = false;
            _recharging = false;
            _tokenSource = new CancellationTokenSource();
        }
        
        public override void OnArmorDestroy()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        internal override void MakeShot()
        {
            if((_currentShots == 0 || _laserOn) && _tokenSource.IsCancellationRequested)
                return;

            --_currentShots;
            _laserOn = true;
            var projectile = CreateProjectile();

            LaserOn(projectile);
            
            if (!_recharging)
                Recharging();
        }

        protected override Projectile CreateProjectile()
        {
            Projectile projectile = Object.Instantiate(_projectile, _armorTransform);
            projectile.OnDestroyCaughtEntity += gameObject =>
            {
                ObjectObserver.DestroyEntity(gameObject);
                if(gameObject != projectile.gameObject)
                    OnProjectileHit?.Invoke();
            };

            return projectile;
        }

        private async Task LaserOn(Projectile projectile)
        {
            await Task.Delay((int) (projectile.LifeTime * 1000), _tokenSource.Token);
            Object.Destroy(projectile.gameObject);
            _laserOn = false;
        }

        private async Task Recharging()
        {
            _recharging = true;
            while (_currentShots != _maxShots && !_tokenSource.IsCancellationRequested)
            {
                _cooldownLeft = _cooldownRate;
                await RechargeShot();
                ++_currentShots;
                _onArmorStatusUpdated?.Invoke(_currentShots, 0f);
            }

            _recharging = false;
        }

        private async Task RechargeShot()
        {
            while (_cooldownLeft > 0 && !_tokenSource.IsCancellationRequested)
            {
                float time = Time.deltaTime;
                await Task.Delay((int)(time * 1000), _tokenSource.Token);
                _cooldownLeft -= time;
                _onArmorStatusUpdated?.Invoke(_currentShots, _cooldownLeft);
            }
        }
    }
}