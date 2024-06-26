﻿using System;
using InfernumMode.Core.OverridingSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace InfernumMode.Content.BehaviorOverrides.MinibossAIs.Ogre
{
    public class OgreBehaviorOverride2 : NPCBehaviorOverride
    {
        public override int NPCOverrideType => NPCID.DD2OgreT3;

        public override bool PreAI(NPC npc) => OgreBehaviorOverride.DoAI(npc);

        public override void FindFrame(NPC npc, int frameHeight)
        {
            npc.frame.Width = 80;
            npc.frame.Y = (int)Math.Round(npc.localAI[0]);
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color lightColor) => OgreBehaviorOverride.DoDrawing(npc, Main.spriteBatch, lightColor);
    }
}
