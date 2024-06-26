﻿using System;
using System.Collections.Generic;
using InfernumMode.Core.GlobalInstances.Systems;
using Luminance.Core.ModCalls;

namespace InfernumMode.Core.ModCalls.InfernumCalls
{
    public class SetInfernumActiveModCall : ModCall
    {
        public override IEnumerable<string> GetCallCommands()
        {
            yield return "SetInfernumActive";
        }

        public override IEnumerable<Type> GetInputTypes()
        {
            yield return typeof(bool);
        }

        protected override object SafeProcess(params object[] argsWithoutCommand)
        {
            WorldSaveSystem.InfernumModeEnabled = (bool)argsWithoutCommand[0];
            return ModCallManager.DefaultObject;
        }
    }
}
