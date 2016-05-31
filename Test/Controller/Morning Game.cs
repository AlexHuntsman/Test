using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Test.Model;
using Test.View;

namespace Test.Controller
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class MorningGame : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private Player player; 

		// Keyboard states used to determine key presses
		private KeyboardState currentKeyboardState;
		private KeyboardState previousKeyboardState;

		// Gamepad states used to determine button presses
		private GamePadState currentGamePadState;
		private GamePadState previousGamePadState; 

		// A movement speed for the player
		float playerMoveSpeed;

		private Texture2D mainBackground;

		private ParallaxingBackground bgLayer1;
		private ParallaxingBackground bgLayer2;

		// Enemies
		Texture2D enemyTexture;
		List<Enemy> enemies;

		// The rate at which the enemies appear
		TimeSpan enemySpawnTime;
		TimeSpan previousSpawnTime;

		// A random number generator
		Random random;

		Texture2D projectileTexture;
		List<Projectile> projectiles;

		// The rate of fire of the player laser
		TimeSpan fireTime;
		TimeSpan previousFireTime;

		TimeSpan plasmaFireTime;
		TimeSpan previousPlasmaTime;
		List<Plasma> fireballs;

		Texture2D plasmaTexture;

		TimeSpan missileFireTime;
		TimeSpan previousMisileTime;
		List<Missile> Missiles;

		Texture2D missileTexture;

		public MorningGame ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// Initialize the player class
			player = new Player();
			playerMoveSpeed = 8.0f;
			bgLayer1 = new ParallaxingBackground();
			bgLayer2 = new ParallaxingBackground();
			// Initialize the enemies list
			enemies = new List<Enemy> ();

			// Set the time keepers to zero
			previousSpawnTime = TimeSpan.Zero;

			// Used to determine how fast enemy respawns
			enemySpawnTime = TimeSpan.FromSeconds(1.0f);

			// Initialize our random number generator
			random = new Random();

			projectiles = new List<Projectile>();

			// Set the laser to fire every quarter second
			fireTime = TimeSpan.FromSeconds(.15f);

			fireballs = new List<Plasma>();

			// Set the laser to fire every half second
			plasmaFireTime = TimeSpan.FromSeconds(.25f);

			Missiles = new List<Missile>();

			// Set the laser to fire every second
			missileFireTime = TimeSpan.FromSeconds(.5f);



			base.Initialize ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			// Load the player resources
			Animation playerAnimation = new Animation();
			Texture2D playerTexture = Content.Load<Texture2D>("Animation/shipAnimation");
			playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);

			Vector2 playerPosition = new Vector2 (GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y
				+ GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
			player.Initialize(playerAnimation, playerPosition);

			Animation plasmaAnimation = new Animation();
			Animation missileAnimation = new Animation();

			bgLayer1.Initialize(Content, "Texture/bgLayer1", GraphicsDevice.Viewport.Width, -1);
			bgLayer2.Initialize(Content, "Texture/bgLayer2", GraphicsDevice.Viewport.Width, -2);
			enemyTexture = Content.Load<Texture2D>("Animation/mineAnimation");
			mainBackground = Content.Load<Texture2D>("Texture/mainbackground");
			projectileTexture = Content.Load<Texture2D>("Texture/laser");

			 plasmaTexture = Content.Load<Texture2D>("Animation/Plasma");
			 missileTexture = Content.Load<Texture2D> ("Animation/Missile");

			plasmaAnimation.Initialize(plasmaTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);
			missileAnimation.Initialize(missileTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);
		}
		private void UpdatePlayer(GameTime gameTime)
		{
			player.Update(gameTime);


			// Get Thumbstick Controls
			player.Position.X += currentGamePadState.ThumbSticks.Left.X *playerMoveSpeed;
			player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y *playerMoveSpeed;

			// Use the Keyboard / Dpad
			if (currentKeyboardState.IsKeyDown(Keys.Left) ||
				currentGamePadState.DPad.Left == ButtonState.Pressed)
			{
				player.Position.X -= playerMoveSpeed;
			}
			if (currentKeyboardState.IsKeyDown(Keys.Right) ||
				currentGamePadState.DPad.Right == ButtonState.Pressed)
			{
				player.Position.X += playerMoveSpeed;
			}
			if (currentKeyboardState.IsKeyDown(Keys.Up) ||
				currentGamePadState.DPad.Up == ButtonState.Pressed)
			{
				player.Position.Y -= playerMoveSpeed;
			}
			if (currentKeyboardState.IsKeyDown(Keys.Down) ||
				currentGamePadState.DPad.Down == ButtonState.Pressed)
			{
				player.Position.Y += playerMoveSpeed;
			}
			if (currentKeyboardState.IsKeyDown(Keys.Q))
			{
				// Fire only every interval we set as the fireTime
				if (gameTime.TotalGameTime - previousFireTime > fireTime)
				{
					// Reset our current time
					previousFireTime = gameTime.TotalGameTime;

					// Add the projectile, but add it to the front and center of the player
					AddProjectile(player.Position + new Vector2(player.Width / 2, 0));
				}
			}
			if (currentKeyboardState.IsKeyDown(Keys.W))
			{
				// Fire only every interval we set as the fireTime
				if (gameTime.TotalGameTime - previousPlasmaTime > plasmaFireTime)
				{
					// Reset our current time
					previousPlasmaTime = gameTime.TotalGameTime;

					// Add the projectile, but add it to the front and center of the player
					AddPlasma(player.Position + new Vector2(player.Width / 2, 0));
				}
			}
			if (currentKeyboardState.IsKeyDown(Keys.E))
			{
				// Fire only every interval we set as the fireTime
				if (gameTime.TotalGameTime - previousMisileTime > missileFireTime)
				{
					// Reset our current time
					previousMisileTime = gameTime.TotalGameTime;

					// Add the projectile, but add it to the front and center of the player
					AddMissile(player.Position + new Vector2(player.Width / 2, 0));
				}
			}

			// Make sure that the player does not go out of bounds
			player.Position.X = MathHelper.Clamp(player.Position.X, player.Width/2,GraphicsDevice.Viewport.Width - player.Width);
			player.Position.Y = MathHelper.Clamp(player.Position.Y, player.Width/2,GraphicsDevice.Viewport.Height - player.Height);




		}

		private void AddEnemy()
		{ 
			// Create the animation object
			Animation enemyAnimation = new Animation();

			// Initialize the animation with the correct animation information
			enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 30,Color.White, 1f, true);

			// Randomly generate the position of the enemy
			Vector2 position = new Vector2(GraphicsDevice.Viewport.Width +enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height -100));

			// Create an enemy
			Enemy enemy = new Enemy();

			// Initialize the enemy
			enemy.Initialize(enemyAnimation, position); 

			// Add the enemy to the active enemies list
			enemies.Add(enemy);
		}

		private void AddProjectile(Vector2 position)
		{
			Projectile projectile = new Projectile(); 
			projectile.Initialize(GraphicsDevice.Viewport, projectileTexture,position); 
			projectiles.Add(projectile);
		}

		private void AddPlasma(Vector2 position)
		{
			Plasma plasma = new Plasma(); 
			Animation plasmaAnimation = new Animation ();
			plasmaAnimation.Initialize(plasmaTexture, Vector2.Zero, 47, 61, 8, 30,Color.White, 1f, true);
			plasma.Initialize(GraphicsDevice.Viewport, plasmaAnimation,position); 
			fireballs.Add(plasma);
		}

		private void AddMissile(Vector2 position)
		{
			Missile missile = new Missile();
			Animation missileAnimation = new Animation ();
			missileAnimation.Initialize (missileTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);
			missile.Initialize(GraphicsDevice.Viewport, missileAnimation,position); 
			Missiles.Add(missile);
		}

		private void UpdateEnemies(GameTime gameTime)
		{
			// Spawn a new enemy enemy every 1.5 seconds
			if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime) 
			{
				previousSpawnTime = gameTime.TotalGameTime;

				// Add an Enemy
				AddEnemy();
			}

			// Update the Enemies
			for (int i = enemies.Count - 1; i >= 0; i--) 
			{
				enemies[i].Update(gameTime);

				if (enemies[i].Active == false)
				{
					enemies.RemoveAt(i);
				} 
			}
		}

		private void UpdateProjectiles()
		{
			// Update the Projectiles
			for (int i = projectiles.Count - 1; i >= 0; i--) 
			{
				projectiles[i].Update();

				if (projectiles[i].Active == false)
				{
					projectiles.RemoveAt(i);
				} 
			}
		}

		private void UpdatePlasma(GameTime gameTime)
		{
			// Update the Projectiles
			for (int i = fireballs.Count - 1; i >= 0; i--) 
			{
				fireballs[i].Update(gameTime);

				if (fireballs[i].Active == false)
				{
					fireballs.RemoveAt(i);
				} 
			}
		}

		private void UpdateMissiles(GameTime gameTime)
		{
			// Update the Projectiles
			for (int i = Missiles.Count - 1; i >= 0; i--) 
			{
				Missiles[i].Update(gameTime);

				if (Missiles[i].Active == false)
				{
					Missiles.RemoveAt(i);
				} 
			}
		}


		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__ &&  !__TVOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape))
				Exit ();
			#endif

			// Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
			previousGamePadState = currentGamePadState;
			previousKeyboardState = currentKeyboardState;

			// Read the current state of the keyboard and gamepad and store it
			currentKeyboardState = Keyboard.GetState();
			currentGamePadState = GamePad.GetState(PlayerIndex.One);


			//Update the player
			UpdatePlayer(gameTime);

			bgLayer1.Update();
			bgLayer2.Update();
			// Update the enemies
			UpdateEnemies(gameTime);
			// Update the collision
			UpdateCollision();
			// Update the projectiles
			UpdateProjectiles();
			UpdateMissiles (gameTime);
			UpdatePlasma (gameTime);
			
			base.Update (gameTime);
		}
		private void UpdateCollision()
		{
			// Use the Rectangle's built-in intersect function to 
			// determine if two objects are overlapping
			Rectangle rectangle1;
			Rectangle rectangle2;

			// Only create the rectangle once for the player
			rectangle1 = new Rectangle((int)player.Position.X,
				(int)player.Position.Y,
				player.Width,
				player.Height);

			// Do the collision between the player and the enemies
			for (int i = 0; i <enemies.Count; i++)
			{
				rectangle2 = new Rectangle((int)enemies[i].Position.X,
					(int)enemies[i].Position.Y,
					enemies[i].Width,
					enemies[i].Height);

				// Determine if the two objects collided with each
				// other
				if(rectangle1.Intersects(rectangle2))
				{
					// Subtract the health from the player based on
					// the enemy damage
					player.Health -= enemies[i].Damage;

					// Since the enemy collided with the player
					// destroy it
					enemies[i].Health = 0;

					// If the player health is less than zero we died
					if (player.Health <= 0)
						player.Active = false; 


				}

			}
			// Projectile vs Enemy Collision
			for (int i = 0; i < projectiles.Count; i++)
			{
				for (int j = 0; j < enemies.Count; j++)
				{
					// Create the rectangles we need to determine if we collided with each other
					rectangle1 = new Rectangle((int)projectiles[i].Position.X - 
						projectiles[i].Width / 2,(int)projectiles[i].Position.Y - 
						projectiles[i].Height / 2,projectiles[i].Width, projectiles[i].Height);

					rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
						(int)enemies[j].Position.Y - enemies[j].Height / 2,
						enemies[j].Width, enemies[j].Height);

					// Determine if the two objects collided with each other
					if (rectangle1.Intersects(rectangle2))
					{
						enemies[j].Health -= projectiles[i].Damage;
						projectiles[i].Active = false;
					}
				}
			}
			for (int i = 0; i < fireballs.Count; i++)
			{
				for (int j = 0; j < enemies.Count; j++)
				{
					// Create the rectangles we need to determine if we collided with each other
					rectangle1 = new Rectangle((int)fireballs[i].Position.X - 
						fireballs[i].Width / 2,(int)fireballs[i].Position.Y - 
						fireballs[i].Height / 2,fireballs[i].Width, fireballs[i].Height);

					rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
						(int)enemies[j].Position.Y - enemies[j].Height / 2,
						enemies[j].Width, enemies[j].Height);

					// Determine if the two objects collided with each other
					if (rectangle1.Intersects(rectangle2))
					{
						enemies[j].Health -= fireballs[i].Damage;
						fireballs[i].Active = false;
					}
				}
			}
			for (int i = 0; i < Missiles.Count; i++)
			{
				for (int j = 0; j < enemies.Count; j++)
				{
					// Create the rectangles we need to determine if we collided with each other
					rectangle1 = new Rectangle((int)Missiles[i].Position.X - 
						Missiles[i].Width / 2,(int)Missiles[i].Position.Y - 
						Missiles[i].Height / 2,Missiles[i].Width, Missiles[i].Height);

					rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
						(int)enemies[j].Position.Y - enemies[j].Height / 2,
						enemies[j].Width, enemies[j].Height);

					// Determine if the two objects collided with each other
					if (rectangle1.Intersects(rectangle2))
					{
						enemies[j].Health -= Missiles[i].Damage;
						Missiles[i].Active = false;
					}
				}
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			// Start drawing
			spriteBatch.Begin();

			spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

			// Draw the moving background
			bgLayer1.Draw(spriteBatch);
			bgLayer2.Draw(spriteBatch);
			// Draw the Enemies
			for (int i = 0; i < enemies.Count; i++)
			{
				enemies[i].Draw(spriteBatch);
			}

			// Draw the Projectiles
			for (int i = 0; i < projectiles.Count; i++)
			{
				projectiles[i].Draw(spriteBatch);
			}

			for (int i = 0; i < fireballs.Count; i++)
			{
				fireballs[i].Draw(spriteBatch);
			}

			for (int i = 0; i < Missiles.Count; i++)
			{
				Missiles[i].Draw(spriteBatch);
			}

			// Draw the Player
			player.Draw(spriteBatch);

			// Stop drawing
			spriteBatch.End();

			base.Draw (gameTime);
		}
	}
}

