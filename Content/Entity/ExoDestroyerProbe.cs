using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.Localization;

//Creation of probe
namespace DestroyerTest.Content.Entity
{
	public class ExoProbe : ModNPC
	{

		public override void SetDefaults()
		{
			NPC.aiStyle = 139;
			NPC.width = 16;
			NPC.height = 19;
			NPC.damage = 25;
			NPC.defense = 10;
			NPC.knockBackResist = 0.2f;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCHit42;
			NPC.DeathSound = SoundID.NPCDeath37;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.npcSlots = 0.75f;
			NPC.value = Item.buyPrice(0, 0, 10, 0);

		}
		public override void FindFrame(int frameHeight)
		{
			
		}

		public override void AI()
        {
            NPC.TargetClosest(true);
            Player target = Main.player[NPC.target];

            if (target == null || !target.active)
                return;

            Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero);

            // Random chance to shoot
            if (Main.rand.NextBool(100)) // 1% chance per frame
            {
                ShootLaser(direction);
            }
        }

        private void ShootLaser(Vector2 direction)
        {
            Vector2 spawnPosition = NPC.Center;
            float speed = 8f;
            int damage = 15;
            int type = ProjectileID.DeathLaser; // Use Destroyer Probe laser

            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPosition, direction * speed, type, damage, 1f, Main.myPlayer);
        }
	}
}
