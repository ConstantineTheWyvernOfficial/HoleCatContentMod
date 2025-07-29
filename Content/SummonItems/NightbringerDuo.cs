using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.SummonItems
{
public class NightbringerDuoBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
			Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
		}

		public override void Update(Player player, ref int buffIndex) {
			// If the minions exist reset the buff time, otherwise remove the buff from the player
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Dawn>()] > 0) {
				player.buffTime[buffIndex] = 18000;
			}
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

	public class NightbringerDuo : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

			ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f; // The default value is 1, but other values are supported. See the docs for more guidance. 
		}

		public override void SetDefaults() {
			Item.damage = 120;
			Item.knockBack = 0f;
			Item.mana = 100; // mana cost
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.RaiseLamp; // how the player's arm moves when using the item
			Item.value = 18000;
			Item.rare = ItemRarityID.Expert;
			Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/StarBurst") with {
				Volume = 1.0f, 
    			Pitch = 0.0f, 
    			PitchVariance = 0.5f, 
			}; // The sound when the weapon is being used.

			// These below are needed for a minion weapon
			Item.noMelee = true; // this item doesn't do any melee damage
			Item.DamageType = DamageClass.Summon; // Makes the damage register as summon. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type
			Item.buffType = ModContent.BuffType<NightbringerDuoBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<Dawn>(); // This item creates the minion projectile
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
			position = Main.MouseWorld;
		}

       
       // Define minionTypes as a class field so both methods can access it
    private static readonly List<int> minionTypes = new List<int>
    {
        ModContent.ProjectileType<Dawn>(),
        ModContent.ProjectileType<Dusk>()
    };

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
        player.AddBuff(Item.buffType, 6000);

        // Iterate through the list and spawn each minion
        foreach (int minionType in minionTypes)
        {
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, minionType, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;
        }

        // Since we spawned the projectile manually already, return false so the game doesn't spawn another one
        return false;
    }

	}

    public class Dawn : ModProjectile
	{
        private bool attacking = false;
        private float orbitDistance = 60f;
        private float orbitSpeed = 0.05f;
        private int shootCooldown = 30;
        private int shootTimer = 0;
        NPC closestNPC = null;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
		}

		public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            NPC target = FindTarget();

            float angle = Projectile.ai[0]; // Maintain fixed rotation
            float centerX = player.Center.X + (float)Math.Cos(angle) * orbitDistance;
            float centerY = player.Center.Y + (float)Math.Sin(angle) * orbitDistance;
            Projectile.position = new Vector2(centerX - Projectile.width / 2, centerY - Projectile.height / 2);

            Projectile.ai[0] += orbitSpeed;

            if (target != null)
            {
                attacking = true;
                if (shootTimer <= 0)
                {
                    SoundEngine.PlaySound(SoundID.Item39);
                    ShootArrow(target);
                    shootTimer = shootCooldown;
                }
            }
            else
            {
                attacking = false;
            }

            shootTimer--;
        }

        private NPC FindTarget()
        {
            float maxDetectDistance = 500f;
            
            float closestDist = maxDetectDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy() && Vector2.Distance(npc.Center, Projectile.Center) < closestDist)
                {
                    closestDist = Vector2.Distance(npc.Center, Projectile.Center);
                    closestNPC = npc;
                }
            }
            return closestNPC;
        }

        private void ShootArrow(NPC target)
        {
            Vector2 velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 40f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileID.WoodenArrowFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(attacking ? "DestroyerTest/Content/SummonItems/Dawn/DawnAttack" : "DestroyerTest/Content/SummonItems/Dawn/DawnIdle").Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, 0f, texture.Size() / 2, 1f, SpriteEffects.None, 0);
             // Default to no mirroring
            SpriteEffects effects = SpriteEffects.None;

            // If lunging AND the enemy is to the right of the projectile, flip horizontally
            if (closestNPC.Center.X > Projectile.Center.X)
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            Main.EntitySpriteDraw(texture, 
                Projectile.Center - Main.screenPosition, 
                null, 
                lightColor, 
                0f, 
                texture.Size() / 2, 
                1f, 
                effects, 
                0
            );

            return false; // Prevents default drawing
        }

		public override bool OnTileCollide(Vector2 oldVelocity) {
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) {
				Projectile.Kill();
			}
			else {
				Projectile.ai[0] += 0.1f;
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				Projectile.velocity *= 0.75f;
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			}
			return false;
		}

		public override void OnKill(int timeLeft) {
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Dirt, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
			}
			SoundEngine.PlaySound(SoundID.Item25, Projectile.position);
		}
	}

    public class Dusk : ModProjectile
	{
            private enum AIState { Idle, Lunging, Returning }
            private AIState state = AIState.Idle;

            private NPC targetNPC = null;
            private float maxDetectDistance = 500f;
            private int attackCooldown = 60;
            private int returnDelay = 20;
            private int attackTimer = 0;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
            }

            private bool CheckActive(Player owner) {
			if (owner.dead || !owner.active) {
				owner.ClearBuff(ModContent.BuffType<NightbringerDuoBuff>());

				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<NightbringerDuoBuff>())) {
				Projectile.timeLeft = 2;
			}

            
			return true;
		}

            public override void AI()
            {

                
            Player player = Main.player[Projectile.owner];

            switch (state)
            {
                case AIState.Idle:
                    OrbitPlayer(player);
                    if (attackTimer <= 0)
                    {
                        targetNPC = FindTarget();
                        if (targetNPC != null)
                        {
                            state = AIState.Lunging;
                        }
                    }
                    break;

                case AIState.Lunging:
                    if (targetNPC == null || !targetNPC.active)
                    {
                        state = AIState.Returning;
                        break;
                    }

                    
                    // Move towards the target
                    Vector2 moveDirection = (targetNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 15f;
                    Projectile.velocity = moveDirection;
                    SoundEngine.PlaySound(SoundID.Item66);

                    // If we reach the target, switch to Returning state
                    if (Vector2.Distance(Projectile.Center, targetNPC.Center) < 10f)
                    {
                        state = AIState.Returning;
                        attackTimer = attackCooldown;
                    }
                    break;

                case AIState.Returning:
                    // Instantly teleport back to the player
                    SoundEngine.PlaySound(SoundID.Item9);
                    Projectile.Center = player.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20));
                    Projectile.velocity = Vector2.Zero;
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Excalibur,
                    new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(Projectile.Hitbox) },
                    Projectile.owner);
                    state = AIState.Idle;
                    break;
            }

            attackTimer--;
        }

        private void OrbitPlayer(Player player)
        {
            float orbitRadius = 60f;
            float orbitSpeed = 0.05f;
            float angle = Projectile.ai[0];

            Vector2 orbitPosition = player.Center + new Vector2((float)Math.Cos(angle) * orbitRadius, (float)Math.Sin(angle) * orbitRadius);
            Projectile.position = orbitPosition - new Vector2(Projectile.width / 2, Projectile.height / 2);
            Projectile.ai[0] += orbitSpeed;
        }

        private NPC FindTarget()
        {
            NPC closestNPC = null;
            float closestDist = maxDetectDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy() && Vector2.Distance(npc.Center, Projectile.Center) < closestDist)
                {
                    closestDist = Vector2.Distance(npc.Center, Projectile.Center);
                    closestNPC = npc;
                }
            }
            return closestNPC;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(state == AIState.Lunging ? "DestroyerTest/Content/SummonItems/Dusk/DuskAttack" : "DestroyerTest/Content/SummonItems/Dusk/DuskIdle").Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, 0f, texture.Size() / 2, 1f, SpriteEffects.None, 0);
             // Default to no mirroring
            SpriteEffects effects = SpriteEffects.None;

            // If lunging AND the enemy is to the right of the projectile, flip horizontally
            if (targetNPC.Center.X > Projectile.Center.X)
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            Main.EntitySpriteDraw(texture, 
                Projectile.Center - Main.screenPosition, 
                null, 
                lightColor, 
                0f, 
                texture.Size() / 2, 
                1f, 
                effects, 
                0
            );

            return false; // Prevents default drawing
        }
    }
}


