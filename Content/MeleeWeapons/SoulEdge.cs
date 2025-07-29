using System;
using DestroyerTest.Content.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.MeleeWeapons
{
    public class SoulEdgeProjection : ModProjectile
    {

        public int TileCollisions = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 70;
            Projectile.friendly = true;
            Projectile.penetrate = -1; // Infinite pierce
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180; // 10 seconds max lifespan
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.tileCollide = false;
        }

        public int trailLength = 10;
		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = new Color(211, 240, 246);

			SpriteBatch spriteBatch = Main.spriteBatch;
			Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;

			// --- Draw the main projectile ---
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Main.EntitySpriteDraw(
				projectileTexture,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor,
				Projectile.rotation,
				projectileTexture.Size() / 2,
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			// Restore normal batch
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}

        public override void AI()
        {
            Projectile.velocity *= 1.2f;

            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha -= 5;
            }
            
            Player player = Main.player[Projectile.owner];

            // Always spinning
            Projectile.rotation += 0.4f * Projectile.direction;

              // Generate flying dust effect
            if (Main.rand.NextBool(3)) // 33% chance per tick
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.DungeonSpirit, Projectile.velocity * 0.2f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Chilled, 240);
           
        }

    }
	public class SoulEdge : ModItem
	{
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults() {

            Item.UseSound = SoundID.Item71;
			Item.width = 40; // The item texture's width.
			Item.height = 40; // The item texture's height.

			Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
			Item.useTime = 20; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 20; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
			Item.damage = 15; // The damage your item deals.
			Item.knockBack = 6; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 6; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 1); // The value of the weapon in copper coins.
			Item.rare = ModContent.RarityType<GoliathRarity>(); // Give this item our custom rarity.
			Item.shoot = ModContent.ProjectileType<SoulEdgeProjection>();
            Item.shootSpeed = 0.12f;
		}

        

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Spawn the custom True Night's Edge projectile with an initial slow speed
            int proj = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SoulEdgeProjection>(), damage, knockback, player.whoAmI);
            
            if (proj >= 0 && proj < Main.maxProjectiles)
            {
                Main.projectile[proj].ai[0] = 1f; // Custom AI behavior flag
            }

           
            return false; // Prevents the default projectile from spawning
        }


		public override void MeleeEffects(Player player, Rectangle hitbox) {
			if (Main.rand.NextBool(3)) {
				// Emit dusts when the sword is swung
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.Sparkle>());
			}
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			// Inflict the OnFire debuff for 1 second onto any NPC/Monster that this hits.
			// 60 frames = 1 second
			target.AddBuff(BuffID.BrokenArmor, 120);
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient(ItemID.WoodenSword, 1)
                .AddIngredient<LifeEcho>(20)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}