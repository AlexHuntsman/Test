using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Test.View;
namespace Test.Model
{
	public class Missile
	{
		// Image representing the Projectile
		public Animation MissileAnimation;

		// Position of the Projectile relative to the upper left side of the screen
		public Vector2 Position;

		// State of the Projectile
		public bool Active;

		// The amount of damage the projectile can inflict to an enemy
		public int Damage;

		// Represents the viewable boundary of the game
		Viewport viewport;

		// Get the width of the projectile ship
		public int Width
		{
			get { return MissileAnimation.FrameWidth; }
		}

		// Get the height of the projectile ship
		public int Height
		{
			get { return MissileAnimation.FrameHeight; }
		}

		// Determines how fast the projectile moves
		float projectileMoveSpeed;


		public void Initialize(Viewport viewport, Animation animation, Vector2 position)
		{
			MissileAnimation = animation;
			Position = position;
			this.viewport = viewport;

			Active = true;

			Damage = 20;

			projectileMoveSpeed = 20f;
		}
		public void Update(GameTime gameTime)
		{ 
			// The enemy always moves to the left so decrement it's xposition
			Position.X -= projectileMoveSpeed;

			// Update the position of the Animation
			MissileAnimation.Position = Position;

			// Update Animation
			MissileAnimation.Update(gameTime);

			// If the enemy is past the screen or its health reaches 0 then deactivateit
			if (Position.X > Width )
			{
				// By setting the Active flag to false, the game will remove this objet fromthe
				// active game list
				Active = false;
			}
		}
		public void Draw(SpriteBatch spriteBatch)
		{

			MissileAnimation.Draw(spriteBatch);
		}

	
		public Missile ()
		{
		}
	}
}

