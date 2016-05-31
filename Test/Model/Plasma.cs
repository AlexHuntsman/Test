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
		public Vector2 position;

		// State of the Projectile
		public bool Active;

		// The amount of damage the projectile can inflict to an enemy
		public int Damage;


		// Represents the viewable boundary of the game
		Viewport Viewport;


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

		public Vector2 Position
		{
			get { return position;}
		}

		// Determines how fast the projectile moves
		float projectileMoveSpeed;


		public void InitializeAnimation(Viewport viewport, Animation animation, Vector2 position)
		{
			PlasmaAnimation = animation;
			this.position = position;
			this.Viewport = viewport;

			Active = true;

			Damage = 5;

			projectileMoveSpeed = 20f;
		}

		public void Update(GameTime gameTime)
		{ 
			// The enemy always moves to the left so decrement it's xposition
			position.X += projectileMoveSpeed;

			// Update the position of the Animation
			PlasmaAnimation.Position = position;

			// Update Animation
			PlasmaAnimation.Update(gameTime);

			// If the enemy is past the screen or its health reaches 0 then deactivateit
			if (position.X > Width)
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

