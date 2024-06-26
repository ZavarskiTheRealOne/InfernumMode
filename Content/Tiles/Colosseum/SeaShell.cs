﻿using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace InfernumMode.Content.Tiles.Colosseum
{
    public class SeaShell : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            LocalizedText name = CreateMapEntryName();
            CalamityUtils.SetMerge(Type, TileID.Sand);
            CalamityUtils.SetMerge(Type, TileID.Sandstone);
            CalamityUtils.SetMerge(Type, ModContent.TileType<SandstoneTile>());
            CalamityUtils.SetMerge(Type, TileID.SandstoneBrick);
            CalamityUtils.SetMerge(Type, TileID.HardenedSand);

            // name.SetDefault("Sea Shell");
            AddMapEntry(new Color(158, 120, 103), name);
        }
    }
}
