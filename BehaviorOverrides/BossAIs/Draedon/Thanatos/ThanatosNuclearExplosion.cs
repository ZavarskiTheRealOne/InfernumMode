using CalamityMod;
using InfernumMode.BehaviorOverrides.BossAIs.Prime;
using InfernumMode.ILEditingStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernumMode.BehaviorOverrides.BossAIs.Draedon.Thanatos
{
    public class ThanatosNuclearExplosion : ModProjectile
    {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Explosion");

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 14;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 180;
            projectile.extraUpdates = 1;
            projectile.scale = 0.15f;
            projectile.hide = true;
            projectile.Calamity().canBreakPlayerDefense = true;
        }

        public override void AI()
        {
            projectile.scale += 0.049f;
            projectile.Opacity = Utils.InverseLerp(300f, 265f, projectile.timeLeft, true) * Utils.InverseLerp(0f, 50f, projectile.timeLeft, true);
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (projectile.velocity.Length() < 18f)
                projectile.velocity *= 1.02f;

            if (Main.netMode != NetmodeID.MultiplayerClient && projectile.Opacity > 0.74f)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 spawnPosition = projectile.Center + Main.rand.NextVector2Circular(35f, 35f);
                    Vector2 smokeVelocity = -Vector2.UnitY.RotatedByRandom(2.16f) * Main.rand.NextFloat(6f, 29f);
                    Projectile.NewProjectile(spawnPosition, smokeVelocity, ModContent.ProjectileType<NukeSmoke>(), 0, 0f);
                }
            }

            Lighting.AddLight(projectile.Center, Color.Red.ToVector3());
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            DrawBlackEffectHook.DrawCacheAdditiveLighting.Add(index);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Color explosionColor = Color.Red * projectile.Opacity;
            Vector2 drawPosition = projectile.Center - Main.screenPosition;

            for (int i = 0; i < 2; i++)
                spriteBatch.Draw(texture, drawPosition, null, explosionColor, 0f, texture.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Utilities.CircularCollision(projectile.Center, targetHitbox, projectile.scale * 135f);
        }

        public override bool CanDamage() => projectile.Opacity > 0.45f;
    }
}