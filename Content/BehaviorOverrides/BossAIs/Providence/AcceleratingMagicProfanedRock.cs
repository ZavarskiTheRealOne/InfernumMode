﻿using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Particles.Metaballs;
using InfernumMode.Common.Graphics;
using InfernumMode.Common.Graphics.Metaballs;
using InfernumMode.Content.Projectiles.Wayfinder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernumMode.Content.BehaviorOverrides.BossAIs.Providence
{
    public class AcceleratingMagicProfanedRock : ModProjectile
    {
        public int CurrentVarient = 1;

        public int MagicGlowTimer = 30;

        public ref float Timer => ref Projectile.ai[0];

        public NPC Owner => Main.npc[(int)Projectile.ai[1]];

        public override string Texture => "CalamityMod/Projectiles/Typeless/ArtifactOfResilienceShard" + CurrentVarient;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Profaned Rock");
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
        }

        public override void SetDefaults()
        {
            // These get changed later, but are this be default.
            Projectile.width = 42;
            Projectile.height = 36;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.Opacity = 1;
            Projectile.timeLeft = 240;
        }

        public override void AI()
        {
            // Initialize the rock variant.
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    CurrentVarient = Main.rand.Next(1, 7);
                    switch (CurrentVarient)
                    {
                        case 2:
                            Projectile.width = 30;
                            Projectile.height = 38;
                            break;
                        case 3:
                            Projectile.width = 34;
                            Projectile.height = 38;
                            break;
                        case 4:
                            Projectile.width = 36;
                            Projectile.height = 46;
                            break;
                        case 5:
                            Projectile.width = 28;
                            Projectile.height = 36;
                            break;
                        case 6:
                            Projectile.width = 22;
                            Projectile.height = 20;
                            break;
                    }
                    Projectile.netUpdate = true;
                }
            }

            // Accelerate.
            if (Projectile.velocity.Length() < 32f)
                Projectile.velocity *= 1.037f;

            // Create rock particles.
            Particle rockParticle = new SandyDustParticle(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width / 3f, Projectile.height / 3f), Vector2.Zero, Color.SandyBrown, Main.rand.NextFloat(0.45f, 0.75f), 30);
            GeneralParticleHandler.SpawnParticle(rockParticle);

            // Emit lava particles.
            if (Main.rand.NextBool() && Main.netMode != NetmodeID.Server)
            {
                Vector2 lavaSpawnPosition = Projectile.Center + Projectile.velocity * 0.5f;
                ModContent.Request<Texture2D>(Texture).Value.CreateMetaballsFromTexture(ref FusableParticleManager.GetParticleSetByType<ProfanedLavaParticleSet>().Particles, lavaSpawnPosition, 0f, Projectile.scale * 0.8f, 12f, 190);
            }

            // Spin.
            Projectile.rotation -= 0.1f;
            Timer++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() * 0.5f;

            Color backglowColor = Color.Lerp(WayfinderSymbol.Colors[0], Color.Pink, 0.5f);
            backglowColor.A = 0;

            // Draw the bloom line telegraph.
            if (Timer <= MagicGlowTimer)
            {
                float opacity = CalamityUtils.Convert01To010(Timer / MagicGlowTimer);
                BloomLineDrawInfo lineInfo = new()
                {
                    LineRotation = -Projectile.velocity.ToRotation(),
                    WidthFactor = 0.003f + MathF.Pow(opacity, 5f) * (MathF.Sin(Main.GlobalTimeWrappedHourly * 3f) * 0.001f + 0.001f),
                    BloomIntensity = MathHelper.Lerp(0.06f, 0.16f, opacity),
                    Scale = Vector2.One * 1950f,
                    MainColor = Color.Pink,
                    DarkerColor = Color.Orange,
                    Opacity = opacity,
                    BloomOpacity = 0.4f,
                    LightStrength = 5f
                };
                Utilities.DrawBloomLineTelegraph(drawPosition, lineInfo);
            }

            float backglowCount = 12;
            for (int i = 0; i < backglowCount; i++)
            {
                Vector2 backglowOffset = (MathHelper.TwoPi * i / backglowCount).ToRotationVector2() * 4f;
                Main.EntitySpriteDraw(texture, drawPosition + backglowOffset, null, backglowColor * Projectile.Opacity, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, drawPosition, null, Projectile.GetAlpha(lightColor) * Projectile.Opacity, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            if (Timer <= MagicGlowTimer)
            {
                backglowColor = Color.HotPink * (1 - Timer / MagicGlowTimer);
                for (int i = 0; i < 3; i++)
                    Main.EntitySpriteDraw(texture, drawPosition, null, backglowColor * Projectile.Opacity, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}