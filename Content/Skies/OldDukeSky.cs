﻿using CalamityMod.NPCs.OldDuke;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace InfernumMode.Content.Skies
{
    public class OldDukeSkyScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            int oldDukeID = ModContent.NPCType<OldDuke>();
            int oldDuke = NPC.FindFirstNPC(oldDukeID);
            NPC oldDukeNPC = oldDuke >= 0 ? Main.npc[oldDuke] : null;
            bool enabled = oldDukeNPC != null && oldDukeNPC.Infernum().ExtraAI[6] >= 2f;
            return enabled;
        }

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("InfernumMode:OldDuke", isActive);
        }
    }

    public class OldDukeSky : CustomSky
    {
        private bool isActive;
        private float intensity;
        private int OldDukeIndex = -1;

        public override void Update(GameTime gameTime)
        {
            if (isActive && intensity < 1f)
            {
                intensity += 0.01f;
            }
            else if (!isActive && intensity > 0f)
            {
                intensity -= 0.01f;
            }
        }

        private float GetIntensity()
        {
            if (UpdatePIndex())
            {
                float x = 0f;
                if (OldDukeIndex != -1)
                {
                    x = Vector2.Distance(Main.player[Main.myPlayer].Center, Main.npc[OldDukeIndex].Center);
                }
                return (1f - Utils.SmoothStep(3000f, 6000f, x)) * 0.65f;
            }
            return 0.7f;
        }

        public override Color OnTileColor(Color inColor)
        {
            float intensity = GetIntensity();
            return new Color(Vector4.Lerp(new Vector4(0.5f, 0.8f, 0.5f, 1f), inColor.ToVector4(), 1f - intensity));
        }

        private bool UpdatePIndex()
        {
            int oldDukeType = ModContent.NPCType<OldDuke>();
            if (OldDukeIndex >= 0 && Main.npc[OldDukeIndex].active && Main.npc[OldDukeIndex].type == oldDukeType)
                return false;

            OldDukeIndex = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == oldDukeType && Main.npc[i].Infernum().ExtraAI[6] >= 2f)
                {
                    OldDukeIndex = i;
                    break;
                }
            }
            return OldDukeIndex != -1;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                float intensity = GetIntensity();
                Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(42, 52, 82) * intensity);
            }
        }

        public override float GetCloudAlpha()
        {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
        }

        public override bool IsActive()
        {
            return isActive || intensity > 0f;
        }
    }
}
