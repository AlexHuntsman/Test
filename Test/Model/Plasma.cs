using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Test.View;
namespace Test.Model
{
	public class Plasma
	{
		// Image representing the Projectile
		public Animation PlasmaAnimation;

		// Position of the Projectile relative to the upper left side of the screen
		public Vector2 Position;

		// State of the Projectile
		public bool Active;

		// The amount of damage the projectile can inflict to an enemy
		public int Damage;

		public Texture2D plasmaTexture;

		// Represents the viewable boundary of the game
		Viewport viewport;


		// Get the width of the projectile ship
		public int Width
		{
			get { return PlasmaAnimation.FrameWidth; }
		}

		// Get the height of the projectile ship
		public int Height
		{
			get { return PlasmaAnimation.FrameHeight; }
		}

		// Determines how fast the projectile moves
		float projectileMoveSpeed;


		public void Initialize(Texture2D texture, Vector2 position)
		{

			Active = true;
			this.Damage = 5;

			this.plasmaTexture = texture;
			this.Position = position;

		}
		public void Initialize(Viewport viewport, Animation animation, Vector2 position)
		{
			PlasmaAnimation = animation;
			Position = position;
			this.viewport = viewport;

			Active = true;

			Damage = 5;

			projectileMoveSpeed = 20f;
		}

		public void Update(GameTime gameTime)
		{ 
			// The enemy always moves to the left so decrement it's xposition
			Position.X += projectileMoveSpeed;

			// Update the position of the Animation
			PlasmaAnimation.Position = Position;

			// Update Animation
			PlasmaAnimation.Update(gameTime);

			// If the enemy is past the screen or its health reaches 0 then deactivateit
			if (Position.X > Width)
			{
				// By setting the Active flag to false, the game will remove this objet fromthe
				// active game list
				Active = false;
			}
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			
			PlasmaAnimation.Draw(spriteBatch);
		}

	}

}

