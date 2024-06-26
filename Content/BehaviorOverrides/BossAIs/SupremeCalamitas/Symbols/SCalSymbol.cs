﻿using CalamityMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace InfernumMode.Content.BehaviorOverrides.BossAIs.SupremeCalamitas.Symbols
{
    public class SCalSymbol : ModProjectile
    {
        public ref float Time => ref Projectile.ai[0];

        public const int Lifetime = 300;

        public static readonly string[] AlchemicalNames =
        [
            "Brimstone",
            "Elements",
            "Fire",
            "Jupiter",
            "Lead",
            "PhilosophersStone",
            "Platinum",
            "Salt"
        ];

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 256;
            Projectile.height = 256;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Lifetime;
            Projectile.Opacity = 0f;
        }

        // Projectile spawning code is done in SCal's AI.
        public override void AI()
        {
            // Die if SCal is gone.
            if (CalamityGlobalNPC.SCal == -1 || !Main.npc[CalamityGlobalNPC.SCal].active)
            {
                Projectile.Kill();
                return;
            }

            // Fade in and out.
            Projectile.Opacity = Utils.GetLerpValue(0f, 108f, Time, true) * Utils.GetLerpValue(0f, 60f, Projectile.timeLeft, true);
            Projectile.scale = Projectile.Opacity * 0.5f + 0.001f;
            Projectile.velocity.Y = Sin(Time / 42f + Projectile.identity) * 2.1f;

            Time++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>($"InfernumMode/Content/BehaviorOverrides/BossAIs/SupremeCalamitas/Symbols/{AlchemicalNames[Projectile.identity % 8]}").Value;
            Rectangle frame = tex.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
            Vector2 drawPosition;
            Vector2 origin = frame.Size() * 0.5f;
            Color glowColor = Color.Lerp(Color.Pink, Color.Red, Cos(Main.GlobalTimeWrappedHourly * 5f) * 0.5f + 0.5f);

            Main.spriteBatch.SetBlendState(BlendState.Additive);
            // Draw an ominous glowing backimage of the symbol after a bit of time.
            float outwardFade = Main.GlobalTimeWrappedHourly * 0.4f % 1f;
            for (int i = 0; i < 8; i++)
            {
                float opacity = (1f - outwardFade) * Utils.GetLerpValue(0f, 0.15f, outwardFade, true) * 0.6f;
                drawPosition = Projectile.Center + (TwoPi * i / 8f).ToRotationVector2() * outwardFade * Projectile.scale * 32f - Main.screenPosition;
                Main.spriteBatch.Draw(tex, drawPosition, frame, Projectile.GetAlpha(glowColor) * opacity, Projectile.rotation, origin, Projectile.scale, 0, 0);
            }

            drawPosition = Projectile.Center - Main.screenPosition;

            for (int i = 0; i < 3; i++)
                Main.spriteBatch.Draw(tex, drawPosition, frame, Projectile.GetAlpha(Color.White), Projectile.rotation, origin, Projectile.scale, 0, 0);
            Main.spriteBatch.ResetBlendState();
            return false;
        }
    }
}
