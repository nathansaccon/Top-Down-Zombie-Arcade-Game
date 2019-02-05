using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.XNA;
using PROG2370CollisionLibrary;
using AllInOneMono;
using System.ComponentModel;

/* Copyright Nathan Saccon 2018
 * 
 * PROG2370: Final Project
 * 
 * Teacher: Steve Hendrikse
 * 
 * Date Started: November 12, 2018
 * Date Completed: December 7, 2018
 * 
 */

namespace NathanSacconFinalProject
{
    class Player : DrawableGameComponent
    {
        const int HEIGHT = 45;
        const int WIDTH = 45;
        const float SPEED = 2.4f;
        const int SHOOTDELAY_MS = 300;
        const int PROXIMITYDAMAGEDELAY_MS = 800;

        const int STANDFRAME = 0;
        const int FIRSTWALKFRAME = 1;
        const int WALKFRAMES = 4;
        const int SHOOTFRAME = 5;

        private int score = 0;
        internal int Score { get => score; set => score = value; }
        Vector2 velocity;
        
        SpriteBatch spriteBatch;

        Texture2D playerTexture;
        internal Rectangle player;

        List<Rectangle> backgroundRectangles = new List<Rectangle>();
        List<Door> doors = new List<Door>();
        List<Key> keys = new List<Key>();

        

        private int currentFrame = STANDFRAME;
        List<Rectangle> playerFrames;
        float spriteDirection;

        const int FRAMEDELAYMAXCOUNT = 9; 
        int currentFrameDelayCount = 0;

        bool isShooting = false;
        bool isAlive = true;
        bool isArmed = false;

        float lastShot = 0;
        float lastProximityDamage = 0;
        internal int health = 100;

        public Player(Game game, SpriteBatch spriteBatch, List<Rectangle> rigidBodies, List<Door> doors, List<Key> keys) : base(game)
        {
            #region Initial Setup
            
            this.spriteBatch = spriteBatch;
            backgroundRectangles = rigidBodies;
            playerTexture = game.Content.Load<Texture2D>("Images/player");
            this.doors = doors;
            this.keys = keys;
            player = new Rectangle(2, 2, WIDTH, HEIGHT);
            velocity = new Vector2(0);

            #endregion

            #region Animation Setup

            playerFrames = new List<Rectangle>();

            playerFrames.Add(new Rectangle(15, 10, 74, 52)); // Standframe
            
            playerFrames.Add(new Rectangle(8, 119, 70, 96)); // Walkframes
            playerFrames.Add(new Rectangle(8, 261, 75, 90));
            playerFrames.Add(new Rectangle(117, 10, 72, 90));
            playerFrames.Add(new Rectangle(121, 240, 70, 98));

            playerFrames.Add(new Rectangle(12, 382, 70, 98)); // Shoot frame

            spriteDirection = 0f;

            #endregion

        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle sourceRec = playerFrames.ElementAt<Rectangle>(currentFrame);

            Rectangle rotationCorrection; // Draw the texture in the right spot.
            rotationCorrection.X = player.X + WIDTH/2;
            rotationCorrection.Y = player.Y + HEIGHT/2;
            rotationCorrection.Height = player.Height;
            rotationCorrection.Width = player.Width;

            spriteBatch.Begin();
            if (HUD.isTest)
            {
                spriteBatch.DrawRectangle(player, Color.Red);
            }
            spriteBatch.Draw(playerTexture,
                rotationCorrection,
                sourceRec,    
                Color.White,
                spriteDirection,  // Rotation         
                new Vector2(sourceRec.Width/2f, sourceRec.Height/2f),
                SpriteEffects.None, 
                0f
                );

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (isAlive)
            {
                if(health <= 0)
                {
                    isAlive = false;
                }

                KeyboardState ks = Keyboard.GetState();

                velocity.X = 0;
                velocity.Y = 0;


                if (ks.IsKeyDown(Keys.Up))
                {
                    velocity.Y -= SPEED;
                }
                if (ks.IsKeyDown(Keys.Down))
                {
                    velocity.Y += SPEED;
                }
                if (ks.IsKeyDown(Keys.Right))
                {
                    velocity.X += SPEED;
                }
                if (ks.IsKeyDown(Keys.Left))
                {
                    velocity.X -= SPEED;
                }
                

                Rectangle proposedPlayer = new Rectangle(player.X + (int)velocity.X,
                                                                player.Y + (int)velocity.Y,
                                                                player.Width,
                                                                player.Height);
                #region Wall Collision

                // Don't allow movement through walls
                // Get all walls
                List<Rectangle> currentWalls = new List<Rectangle>();
                foreach (Rectangle rec in backgroundRectangles)
                {
                    currentWalls.Add(rec);
                }
                // Make locked doors act as a wall
                for (int i = 0; i < doors.Count; i++)
                {
                    if (doors[i].isLocked)
                    {
                        currentWalls.Add(doors[i].door);
                    }
                }

                // If you're touching a wall dont allow movement
                Sides wallCollisions = proposedPlayer.CheckCollisions(currentWalls);

                if ((wallCollisions & Sides.RIGHT) == Sides.RIGHT)
                {
                    if (velocity.X > 0)
                        velocity.X = 0;
                }
                if ((wallCollisions & Sides.LEFT) == Sides.LEFT)
                {
                    if (velocity.X < 0)
                        velocity.X = 0;
                }
                if ((wallCollisions & Sides.TOP) == Sides.TOP)
                {
                    if (velocity.Y < 0)
                        velocity.Y = 0;
                }
                if ((wallCollisions & Sides.BOTTOM) == Sides.BOTTOM)
                {
                    if (velocity.Y > 0)
                        velocity.Y = 0;
                }

                #endregion

                #region Enemy Collision

                foreach (var comp in Game.Components)
                {
                    if (comp is ActionScene)
                    {
                        ActionScene actionScene = comp as ActionScene;
                        foreach (object obj in actionScene.Components)
                        {
                            if (obj is Enemy)
                            {
                                Enemy enemy = obj as Enemy;
                                Sides enemyCollisions = player.CheckCollisions(enemy.enemy); // enemy.enemy is the enemy rectangle
                                if (enemy.health > 0)
                                {
                                    if (enemyCollisions != Sides.None &&
                                        ((float)gameTime.TotalGameTime.TotalMilliseconds - lastProximityDamage > PROXIMITYDAMAGEDELAY_MS))
                                    {
                                        health -= 10;
                                        lastProximityDamage = (float)gameTime.TotalGameTime.TotalMilliseconds;

                                    }
                                    if ((enemyCollisions & Sides.RIGHT) == Sides.RIGHT)
                                    {
                                        if (velocity.X > 0)
                                            velocity.X = 0;
                                    }
                                    if ((enemyCollisions & Sides.LEFT) == Sides.LEFT)
                                    {
                                        if (velocity.X < 0)
                                            velocity.X = 0;
                                    }
                                    if ((enemyCollisions & Sides.TOP) == Sides.TOP)
                                    {
                                        if (velocity.Y < 0)
                                            velocity.Y = 0;
                                    }
                                    if ((enemyCollisions & Sides.BOTTOM) == Sides.BOTTOM)
                                    {
                                        if (velocity.Y > 0)
                                            velocity.Y = 0;
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Key Collision

                for (int i = 0; i < keys.Count; i++) // If touching key, pick it up.
                {
                    if (!keys[i].isPickedUp)
                    {
                        Sides keyCollisions = proposedPlayer.CheckCollisions(keys[i].key);
                        if (keyCollisions != Sides.None)
                        {
                            keys[i].isPickedUp = true;
                            score += 100;
                            keys[i].soundEffect.Play();
                        }
                    }
                }

                #endregion

                #region Door Collision

                for (int i = 0; i < doors.Count; i++) // Unlock door if holding key
                {
                    if (doors[i].isLocked)
                    {
                        Sides doorCollisions = proposedPlayer.CheckCollisions(doors[i].door);
                        if (doorCollisions != Sides.None)
                        {
                            foreach (Key key in keys)
                            {
                                if (key.isPickedUp && key.color == doors[i].color)
                                {
                                    doors[i].isLocked = false;
                                    score += 100;
                                    doors[i].soundEffect.Play();
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Gun Collision

                foreach (var comp in Game.Components)
                {
                    if (comp is ActionScene)
                    {
                        ActionScene actionScene = comp as ActionScene;
                        foreach (var obj in actionScene.Components) // If touching gun, pick it up.
                        {
                            if (obj is Gun)
                            {
                                Gun gun = obj as Gun;
                                if (!gun.isPickedUp)
                                {
                                    Sides gunCollisions = proposedPlayer.CheckCollisions(gun.gun); // gun.gun is the gun rectangle
                                    if (gunCollisions != Sides.None)
                                    {
                                        gun.isPickedUp = true;
                                        score += 100;
                                        isArmed = true;
                                        gun.soundEffect.Play();
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Animation

                #region Direction

                // Unit circle numbers for 8 directions of travel.
                if (velocity.X > 0)
                {
                    if (velocity.Y > 0)
                    {
                        spriteDirection = 3f * (float)Math.PI / 4f;
                    }
                    else if (velocity.Y < 0)
                    {
                        spriteDirection = (float)Math.PI / 4f;
                    }
                    else
                    {
                        spriteDirection = (float)Math.PI / 2f;
                    }
                }
                else if (velocity.X < 0)
                {
                    if (velocity.Y > 0)
                    {
                        spriteDirection = 5f * (float)Math.PI / 4f;
                    }
                    else if (velocity.Y < 0)
                    {
                        spriteDirection = 7f * (float)Math.PI / 4f;
                    }
                    else
                    {
                        spriteDirection = 3f * (float)Math.PI / 2f;
                    }
                }
                else if (velocity.Y > 0)
                {
                    spriteDirection = (float)Math.PI;
                }
                else if (velocity.Y < 0)
                {
                    spriteDirection = 0f;
                }

                #endregion

                #region Timing

                if (isShooting)
                {
                    currentFrame = SHOOTFRAME;
                }
                else
                {
                    if (nearlyZero(velocity.X) && nearlyZero(velocity.Y))
                    {
                        currentFrame = STANDFRAME;
                    }
                    else
                    {
                        currentFrameDelayCount++;
                        if (currentFrameDelayCount > FRAMEDELAYMAXCOUNT)
                        {
                            currentFrameDelayCount = 0;
                            currentFrame++;
                        }
                        if (currentFrame > WALKFRAMES)
                            currentFrame = FIRSTWALKFRAME;
                    }
                }

                #endregion


                #endregion

                #region Shooting

                if (((float)gameTime.TotalGameTime.TotalMilliseconds - lastShot) > SHOOTDELAY_MS) // Allows shooting again after delay
                {
                    isShooting = false;
                }
                if (ks.IsKeyDown(Keys.Space) && !isShooting && isArmed)
                {
                    Bullet bullet = new Bullet(this.Game, spriteBatch, player.X + RotationToPoint(spriteDirection).X, player.Y + RotationToPoint(spriteDirection).Y, spriteDirection, BulletType.PLAYER);
                    Game.Components.Add(bullet);
                    score -= 10;
                    lastShot = (float)gameTime.TotalGameTime.TotalMilliseconds;
                    isShooting = true;

                }

                #endregion

                player.X += (int)velocity.X;
                player.Y += (int)velocity.Y;

            } else // PLAYER DIED
            {
                foreach (var comp in Game.Components)
                {
                    if (comp is ActionScene)
                    {
                        ActionScene actionScene = comp as ActionScene;
                        foreach (var obj in actionScene.Components) // If you die, show death screen
                        {
                            if (obj is HUD)
                            {
                                HUD hud = obj as HUD;
                                hud.showDeathScreen = true;
                            }
                        }
                    }
                }

            }

            base.Update(gameTime);
        }

        #region Helpers

        /// <summary>
        /// Makes bullet come from gun instead of top corner of player.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        private Point RotationToPoint(float rotation)
        {
            Point point;

            switch (rotation)
            {
                // Numbers come from unit circle arithmetic and some trial and error
                case (float)Math.PI / 4f:
                    point = new Point(WIDTH - 5, -5);
                    break;
                case (float)Math.PI / 2f:
                    point = new Point(WIDTH, (HEIGHT/2)-10);
                    break;
                case 3f * (float)Math.PI / 4f:
                    point = new Point(WIDTH, HEIGHT);
                    break;
                case (float)Math.PI:
                    point = new Point(WIDTH/2, HEIGHT);
                    break;
                case 5f * (float)Math.PI / 4f:
                    point = new Point(0, HEIGHT);
                    break;
                case 3f * (float)Math.PI / 2f:
                    point = new Point(0, (HEIGHT/2)-5);
                    break;
                case 7f * (float)Math.PI / 4f:
                    point = new Point(0, 0);
                    break;
                default:
                    point = new Point((WIDTH/2)-5, 0);
                    break;
            }
            return point;
        }


        private bool nearlyZero(float f1)
        {
            // sometimes 0 is not 0 when its a float or double
            // float.Epsilon is the variance around zero
            return (Math.Abs(f1) < float.Epsilon);
        }
        private bool nearlyEqual(float f1, float f2)
        {
            // sometimes 0 is not 0 when its a float or double
            // float.Epsilon is the variance around zero
            return nearlyZero(f1 - f2);
        }

        #endregion
        

    }
}
