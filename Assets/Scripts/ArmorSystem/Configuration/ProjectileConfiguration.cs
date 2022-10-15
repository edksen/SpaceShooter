﻿using SpaceShooter.Controllers;
using UnityEngine;

namespace SpaceShooter.ArmorSystem.Settings
{
    [CreateAssetMenu(fileName = "ProjectileConfiguration", menuName = "Projectile Configuration", order = 1)]
    public class ProjectileConfiguration : ScriptableObject
    {
        public float MaxSpeed;
        public float Inertia;
        public int LifeTimeSec;
        public float CooldownRate;
        public EntityBorderState BorderState;
    }
}