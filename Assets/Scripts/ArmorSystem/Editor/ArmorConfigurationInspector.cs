using ArmorSystem.Armors;
using ArmorSystem.Settings;
using UnityEditor;

namespace ArmorSystem.Editor
{
    [CustomEditor(typeof(ArmorConfiguration))]
    public class ArmorConfigurationInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var armorConfiguration = target as ArmorConfiguration;
            
            armorConfiguration.Projectile =
                (Projectile) EditorGUILayout.ObjectField("Armor Projectile", armorConfiguration.Projectile, typeof(Projectile), armorConfiguration.Projectile);

            armorConfiguration.ArmorType =
                (ArmorType) EditorGUILayout.EnumPopup("Armor Type", armorConfiguration.ArmorType);

            if (armorConfiguration.ArmorType != ArmorType.Bullet)
            {
                armorConfiguration.AmmoCapacity =
                    EditorGUILayout.IntField("Ammo Capacity", armorConfiguration.AmmoCapacity);
                
                armorConfiguration.AttackRate =
                    EditorGUILayout.IntField("Attack Rate", armorConfiguration.AttackRate);
            }

            if (armorConfiguration.ArmorType == ArmorType.Laser)
                armorConfiguration.RechargeRateInSecs =
                    EditorGUILayout.FloatField("Recharge rate in secs", armorConfiguration.RechargeRateInSecs);
            
            EditorUtility.SetDirty(armorConfiguration);
        }
    }
}