using DestroyerTest.Content.RiftBiome;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DestroyerTest.Content.Entity
{
	/// <summary>
	/// This file shows off a critter npc. The unique thing about critters is how you can catch them with a bug net.
	/// The important bits are: Main.npcCatchable, NPC.catchItem, and Item.makeNPC.
	/// We will also show off adding an item to an existing RecipeGroup (see ExampleRecipes.AddRecipeGroups).
	/// Additionally, this example shows an involved IL edit.
	/// </summary>
	public class RiftSchade : ModNPC
	{

		/// <summary>
		/// Change the following code sequence in Wiring.HitWireSingle
		/// <code>
		///case 61:
		///num115 = 361;
		/// </code>
		/// to
		/// <code>
		///case 61:
		///num115 = Main.rand.NextBool() ? 361 : NPC.type
		/// </code>
		/// This causes the frog statue to spawn this NPC 50% of the time
		/// </summary>
		/// <param name="ilContext"> </param>

		public override void SetStaticDefaults() {
			// The frog is immune to confused
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
		}

		public override void SetDefaults() {
			NPC.width = 30;
			NPC.height = 20;
			NPC.aiStyle = -1;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 1080;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.Item46;
            NPC.noGravity = true;
			// Sets the above
			NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            SpawnModBiomes = [ModContent.GetInstance<RiftSurface>().Type];
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("An anerobic blob of solar energy. If the gunk can be cleared away, one will find the soul of an unfortunate bunny or inchworm that was sprayed over in rift solution."),

				// You can add multiple elements if you really wanted to
				// You can also use localization keys (see Localization/en-US.lang)
				//new FlavorTextBestiaryInfoElement("Mods.ExampleMod.Bestiary.ExamplePerson")
                
			});
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			// Check if the player is in the Shimmer zone and the Cultist Boss has been defeated
			if (ModContent.GetInstance<RiftSurface>().IsBiomeActive(spawnInfo.Player)) // Ensure the Cultist Boss has been defeated
			{
					// Set spawn chance relative to standard overworld night monsters
					return SpawnCondition.OverworldDaySlime.Chance; // 10% of regular zombie spawn rate
			
			}
			return 0f; // Prevent spawning otherwise
		}
        internal int MoveForwardForce = 2;
        internal int radiusofselection = 100;
        private Vector2 targetPosition;
        private int state = 0; // 0 = idle, 1 = turning, 2 = moving
        private float turnSpeed = 0.1f; // Speed at which the entity turns
        public override void AI()
        {
            switch (state)
            {
                case 0: // Idle state
                    SelectNewTargetPosition();
                    state = 1; // Transition to turning state
                    break;

                case 1: // Turning state
                    TurnTowardsTarget();
                    if (HasTurnedToTarget())
                    {
                        SoundStyle Dash = new SoundStyle("DestroyerTest/Assets/Audio/RiftSchadeDash");
                        SoundEngine.PlaySound(Dash);
                        state = 2; // Transition to moving state
                    }
                    break;

                case 2: // Moving state
                    MoveTowardsTarget();
                    if (HasReachedTarget())
                    {
                        state = 0; // Transition to idle state
                    }
                    break;
            }
        }
            private void SelectNewTargetPosition()
        {
            // Randomly select an angle between 0 and 2π radians
            float angle = Main.rand.NextFloat() * MathHelper.TwoPi;

            // Calculate the coordinates of the point on the circumference
            targetPosition = NPC.Center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radiusofselection;
        }

        private void TurnTowardsTarget()
        {
            // Calculate the angle to the target position
            float targetAngle = (targetPosition - NPC.Center).ToRotation();
            float currentAngle = NPC.rotation;

            // Smoothly turn towards the target angle
            NPC.rotation = MathHelper.Lerp(currentAngle, targetAngle, turnSpeed);
        }

        private bool HasTurnedToTarget()
        {
            // Check if the entity has turned to face the target position
            float targetAngle = (targetPosition - NPC.Center).ToRotation();
            return Math.Abs(MathHelper.WrapAngle(NPC.rotation - targetAngle)) < 0.1f;
        }

        private void MoveTowardsTarget()
        {
            // Calculate the direction vector to the target position
            Vector2 direction = (targetPosition - NPC.Center).SafeNormalize(Vector2.Zero);
            NPC.velocity = direction * MoveForwardForce;
        }

        private bool HasReachedTarget()
        {
            // Check if the entity has reached the target position
            return Vector2.Distance(NPC.Center, targetPosition) < 10f;
        }

    }
}