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
using Microsoft.Xna.Framework.Audio;
using AllInOneMono;

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
    public enum BulletType // Who shot the bullet?
    {
        PLAYER,
        ENEMY
    }

    class Bullet : DrawableGameComponent
    {
        const int WIDTH = 7;
        const int HEIGHT = 19;
        const int SPRITEWIDTH = 26;
        const int SPRITEHEIGHT = 70;
        const float SPEED = 5f;

        SpriteBatch spriteBatch;
        SoundEffect soundEffect;
        Texture2D texture;
        Rectangle bullet;
        Vector2 velocity;

        List<Rectangle> bulletFrames;
        int currentFrame = 0;
        int currentFrameDelayCount = 0;
        const int FRAMEDELAYMAXCOUNT = 5;

        bool isAlive;
        BulletType type;

        float rotation;


        public Bullet(Game game, SpriteBatch spriteBatch, int XPosition, int YPosition, float rotation, BulletType type) : base(game)
        {

            #region Initial Setup

            this.spriteBatch = spriteBatch;
            if (type == BulletType.PLAYER) // Player shot bullet
            {
                texture = game.Content.Load<Texture2D>("Images/bullet");
                soundEffect = game.Content.Load<SoundEffect>("Sounds/shot");
                soundEffect.Play();
            }
            else
            {
                texture = game.Content.Load<Texture2D>("Images/zombullet");
            }
            bullet = new Rectangle(XPosition, YPosition, WIDTH, HEIGHT);
            this.rotation = rotation;
            velocity = RotationToVelocity(rotation);
            isAlive = true;
            this.type = type;

            #endregion

            #region Animation Setup

            bulletFrames = new List<Rectangle>();

            bulletFrames.Add(new Rectangle(0, 0, SPRITEWIDTH, SPRITEHEIGHT));
            bulletFrames.Add(new Rectangle(0, 70, SPRITEWIDTH, SPRITEHEIGHT));
            bulletFrames.Add(new Rectangle(0, 140, SPRITEWIDTH, SPRITEHEIGHT));


            #endregion

        }

        public override void Draw(GameTime gameTime)
        {
            if (isAlive)
            {
                Rectangle sourceRec = bulletFrames.ElementAt<Rectangle>(currentFrame%3);

                Rectangle rotationCorrection; // Draw the texture in the right spot.
                rotationCorrection.X = bullet.X + WIDTH / 2;
                rotationCorrection.Y = bullet.Y + HEIGHT / 2;
                rotationCorrection.Height = bullet.Height;
                rotationCorrection.Width = bullet.Width;

                spriteBatch.Begin();
                if (HUD.isTest)
                {
                    spriteBatch.DrawRectangle(bullet, Color.Red);
                }
                spriteBatch.Draw(texture,
                    rotationCorrection,
                    sourceRec,
                    Color.White,
                    rotation,  // Rotation         
                    new Vector2(sourceRec.Width / 2f, sourceRec.Height / 2f),
                    SpriteEffects.None,
                    0f
                    );

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (isAlive)
            {

                #region Wall Collisions

                foreach (var comp in Game.Components)
                {
                    if (comp is ActionScene)
                    {
                        ActionScene actionScene = comp as ActionScene;
                        foreach (object obj in actionScene.Components)
                        {
                            if (obj is Background)
                            {
                                Background background = obj as Background;
                                Sides backgroundCollisions = bullet.CheckCollisions(background.rigidBodies);
                                if (backgroundCollisions != Sides.None)
                                {
                                    isAlive = false; // Bullet death
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Door Collisions

                foreach (var comp in Game.Components)
                {
                    if (comp is ActionScene)
                    {
                        ActionScene actionScene = comp as ActionScene;
                        foreach (object obj in actionScene.Components)
                        {
                            if (obj is Door)
                            {
                                Door door = obj as Door;
                                Sides doorCollisions = bullet.CheckCollisions(door.door); // door.door is the enemy rectangle
                                if (doorCollisions != Sides.None && door.isLocked)
                                {
                                    isAlive = false; // Bullet death
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Enemy Collisions

                if (type == BulletType.PLAYER) // Player shot this bullet
                {
                    foreach (var comp in Game.Components)
                    {
                        if (comp is ActionScene)
                        {
                            ActionScene actionScene = comp as ActionScene;
                            foreach (object obj in actionScene.Components)
                            {
                                if (obj is Enemy)// Player shot this bullet
                                {
                                    Enemy enemy = obj as Enemy;
                                    Sides enemyCollisions = bullet.CheckCollisions(enemy.enemy); // enemy.enemy is the enemy rectangle
                                    if (enemyCollisions != Sides.None && enemy.health > 0)
                                    {
                                        enemy.health -= 26;
                                        isAlive = false; // Bullet death

                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Player Collisions

                if (type == BulletType.ENEMY) // Enemy shot this bullet
                {
                    foreach (var comp in Game.Components)
                    {
                        if (comp is ActionScene)
                        {
                            ActionScene actionScene = comp as ActionScene;
                            foreach (object obj in actionScene.Components)
                            {
                                if (obj is Player)// Player shot this bullet
                                {
                                    Player player = obj as Player;
                                    Sides playerCollisions = bullet.CheckCollisions(player.player); // enemy.enemy is the enemy rectangle
                                    if (playerCollisions != Sides.None)
                                    {
                                        player.health -= 10;
                                        isAlive = false; // Bullet death

                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                bullet.X += (int)velocity.X;
                bullet.Y += (int)velocity.Y;

                #region Animation

                currentFrameDelayCount++;
                if (currentFrameDelayCount > FRAMEDELAYMAXCOUNT)
                {
                    currentFrameDelayCount = 0;
                    currentFrame++;
                }

                #endregion
            }
            else
            {
                this.Dispose();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Makes bullet travel in same direction player is facing
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        private Vector2 RotationToVelocity(float rotation)
        {
            Vector2 velocity;

            switch (rotation) // Unit Circle direction check
            {
                // Points have -y since y axis is inverted
                case (float)Math.PI / 4f:
                    velocity = new Vector2(1 , -1);
                    break;
                case (float)Math.PI / 2f:
                    velocity = new Vector2(1, 0);
                    break;
                case 3f * (float)Math.PI / 4f:
                    velocity = new Vector2(1, 1);
                    break;
                case (float)Math.PI:
                    velocity = new Vector2(0, 1);
                    break;
                case 5f * (float)Math.PI / 4f:
                    velocity = new Vector2(-1, 1);
                    break;
                case 3f * (float)Math.PI / 2f:
                    velocity = new Vector2(-1, 0);
                    break;
                case 7f * (float)Math.PI / 4f:
                    velocity = new Vector2(-1, -1);
                    break;
                default:
                    velocity = new Vector2(0,-1);
                    break;
            }
            // Scale unit circle to my speed
            velocity.X *= SPEED;
            velocity.Y *= SPEED;

            return velocity;
        }

    }
}
