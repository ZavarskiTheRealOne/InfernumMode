﻿using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernumMode.Content.Cooldowns
{
    public class InfiniteFlight : CooldownHandler
    {
        public static new string ID => "InfiniteFlight";

        public override bool ShouldDisplay => true;

        public override LocalizedText DisplayName => Language.GetText("Mods.InfernumMode.Cooldowns.InfiniteFlightCooldown");

        public override string Texture => "InfernumMode/Content/Cooldowns/InfiniteFlight";

        public override Color OutlineColor => new(187, 137, 255);

        public override Color CooldownStartColor => Color.Lerp(new(234, 197, 234), new(207, 234, 234), 1f - instance.Completion);

        public override Color CooldownEndColor => CooldownStartColor;
    }
}
