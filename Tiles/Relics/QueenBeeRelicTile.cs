﻿using InfernumMode.Items.Relics;
using Terraria.ModLoader;

namespace InfernumMode.Tiles.Relics
{
    public class QueenBeeRelicTile : BaseInfernumBossRelic
    {
        public override int DropItemID => ModContent.ItemType<QueenBeeRelic>();

        public override string RelicTextureName => "InfernumMode/Tiles/Relics/QueenBeeRelicTile";
    }
}