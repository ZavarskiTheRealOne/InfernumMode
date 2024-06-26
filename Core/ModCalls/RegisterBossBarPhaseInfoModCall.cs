﻿using System;
using System.Collections.Generic;
using InfernumMode.Content.BossBars;
using Luminance.Core.ModCalls;
using Microsoft.Xna.Framework.Graphics;

namespace InfernumMode.Core.ModCalls.InfernumCalls
{
    public class RegisterBossBarPhaseInfoModCall : ModCall
    {
        public override IEnumerable<string> GetCallCommands()
        {
            yield return "RegisterBossBarPhaseInfo";
        }

        public override IEnumerable<Type> GetInputTypes()
        {
            yield return typeof(int);
            yield return typeof(List<float>);
            yield return typeof(Texture2D);
        }

        protected override object SafeProcess(params object[] argsWithoutCommand)
        {
            int npcType = (int)argsWithoutCommand[0];
            List<float> phaseThresholds = (List<float>)argsWithoutCommand[1];

            // If it already contains an entry for this, don't add it and log a warning.
            if (!BossBarManager.ModCallPhaseInfos.ContainsKey(npcType))
            {
                BossBarManager.ModCallPhaseInfos.Add(npcType, new(npcType, phaseThresholds));
                BossBarManager.ModCallBossIcons.Add(npcType, (Texture2D)argsWithoutCommand[2]);
                BossBarManager.ModCallNPCsThatCanHaveAHPBar.Add(npcType);
            }
            else
                InfernumMode.Instance.Logger.Warn($"Phase info for npc type {npcType} has already been registered!");

            return ModCallManager.DefaultObject;
        }
    }
}
