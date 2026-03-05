using Godot;
using Demonteks.Player;

namespace Demonteks.Debug
{
    /// <summary>
    /// Debug script to verify the weapon system is properly configured.
    /// Attach to the Player node or run from the debugger.
    /// </summary>
    public partial class VerifyWeaponSystem : Node
    {
        public override void _Ready()
        {
            GD.Print("=== WEAPON SYSTEM VERIFICATION ===");
            VerifyWeaponConfiguration();
            GD.Print("=== END VERIFICATION ===");

            // Auto-remove after verification
            QueueFree();
        }

        private void VerifyWeaponConfiguration()
        {
            var player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
            if (player == null)
            {
                GD.PrintErr("ERROR: Player not found in 'player' group!");
                return;
            }

            var combat = player.GetNodeOrNull<PlayerCombat>("PlayerCombat");
            if (combat == null)
            {
                GD.PrintErr("ERROR: PlayerCombat node not found!");
                return;
            }

            var animatedSprite = player.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
            if (animatedSprite == null)
            {
                GD.PrintErr("ERROR: AnimatedSprite2D not found!");
                return;
            }

            var weaponSprite = player.GetNodeOrNull<Sprite2D>("WeaponSprite");
            if (weaponSprite == null)
            {
                GD.PrintErr("WARNING: WeaponSprite node not found! Visual weapon display won't work.");
            }
            else
            {
                GD.Print("✓ WeaponSprite node found");
            }

            GD.Print($"\n--- Available Animations ({animatedSprite.SpriteFrames.GetAnimationCount()}) ---");
            foreach (var animName in animatedSprite.SpriteFrames.GetAnimationNames())
            {
                var anim = animatedSprite.SpriteFrames.GetAnimation(animName);
                GD.Print($"  - {animName}: {anim.Frames.Count} frames, speed: {anim.Speed}, loop: {anim.Loop}");
            }

            GD.Print($"\n--- Weapon Configuration ---");
            GD.Print($"{"WEAPON",-20} {"DAMAGE",-8} {"ANIMATION",-20} {"SPRITE",-20} {"STATUS",-10}");
            GD.Print(new string('-', 80));

            foreach (WeaponType weaponType in System.Enum.GetValues(typeof(WeaponType)))
            {
                var weapon = GetWeaponInfo(combat, weaponType);
                string animName = weaponType switch
                {
                    WeaponType.Knife => "attack",
                    WeaponType.Machete => "attack_machete",
                    WeaponType.Sword => "attack_sword",
                    WeaponType.DemontekSaber => "attack_saber",
                    WeaponType.Sawtooth => "attack_sawtooth",
                    WeaponType.AlienSword => "attack_alien",
                    _ => "attack"
                };

                bool hasAnimation = animatedSprite.SpriteFrames.HasAnimation(animName);
                bool hasSprite = System.IO.File.Exists(GetSpritePath(weaponType));
                string status = (hasAnimation && hasSprite) ? "✓ READY" : "✗ INCOMPLETE";

                GD.Print($"{weapon.Item1,-20} {weapon.Item2,-8} {animName,-20} {GetSpriteFileName(weaponType),-20} {status,-10}");

                if (!hasAnimation)
                    GD.Print($"  ⚠ Missing animation: {animName}");
                if (!hasSprite)
                    GD.Print($"  ⚠ Missing sprite: {GetSpritePath(weaponType)}");
            }

            GD.Print($"\n--- Current Weapon ---");
            GD.Print($"Equipped: {combat.CurrentWeapon.WeaponName}");
            GD.Print($"Damage: {combat.CurrentWeapon.BaseDamage}");
            GD.Print($"Animation: {combat.CurrentAttackAnim}");
        }

        private (string, int) GetWeaponInfo(PlayerCombat combat, WeaponType type)
        {
            // This is a helper to display weapon info
            string name = type.ToString();
            int damage = 10;

            switch (type)
            {
                case WeaponType.Knife:
                    name = "Knife"; damage = 10; break;
                case WeaponType.Machete:
                    name = "Machete"; damage = 20; break;
                case WeaponType.Sword:
                    name = "Ancient Sword"; damage = 30; break;
                case WeaponType.DemontekSaber:
                    name = "Demontek Saber"; damage = 35; break;
                case WeaponType.Sawtooth:
                    name = "Sawtooth"; damage = 40; break;
                case WeaponType.AlienSword:
                    name = "Alien Sword"; damage = 50; break;
            }

            return (name, damage);
        }

        private string GetSpritePath(WeaponType type)
        {
            return type switch
            {
                WeaponType.Machete => "res://Assets/items/weapons/weapon_machete.png",
                WeaponType.Sword => "res://Assets/items/weapons/weapon_sword.png",
                WeaponType.DemontekSaber => "res://Assets/items/weapons/weapon_saber.png",
                WeaponType.Sawtooth => "res://Assets/items/weapons/weapon_sawtooth.png",
                WeaponType.AlienSword => "res://Assets/items/weapons/weapon_alien.png",
                _ => ""
            };
        }

        private string GetSpriteFileName(WeaponType type)
        {
            string path = GetSpritePath(type);
            return string.IsNullOrEmpty(path) ? "(none)" : System.IO.Path.GetFileName(path);
        }
    }
}
