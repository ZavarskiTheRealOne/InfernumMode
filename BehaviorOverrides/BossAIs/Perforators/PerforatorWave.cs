using InfernumMode.BaseEntities;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;

namespace InfernumMode.BehaviorOverrides.BossAIs.Perforators
{
    public class PerforatorWave : BaseWaveExplosionProjectile
    {
        public override int Lifetime => 80;
        public override float MaxRadius => 1500f;
        public override float RadiusExpandRateInterpolant => 0.15f;
        public override float DetermineScreenShakePower(float lifetimeCompletionRatio, float distanceFromPlayer)
        {
            float baseShakePower = MathHelper.Lerp(1f, 5f, (float)Math.Sin(MathHelper.Pi * lifetimeCompletionRatio));
            return baseShakePower * Utils.GetLerpValue(2200f, 1050f, distanceFromPlayer, true);
        }

        public override Color DetermineExplosionColor(float lifetimeCompletionRatio)
        {
            return Color.Lerp(Color.Crimson, Color.Gray, MathHelper.Clamp(lifetimeCompletionRatio * 1.5f, 0f, 1f));
        }

        public override void SendExtraAI(BinaryWriter writer) => writer.Write((int)Projectile.localAI[1]);

        public override void ReceiveExtraAI(BinaryReader reader) => Projectile.localAI[1] = reader.ReadInt32();
    }
}