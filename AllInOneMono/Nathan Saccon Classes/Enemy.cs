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
    public enum EnemyType
    {
        STILL, 
        WALKER
    }
    class Enemy : DrawableGameComponent
    {
        const int HEIGHT = 55;
        const int WIDTH = 45;
        const int SPRITEWIDTH = 66;
        const int SPRITEHEIGHT = 82;
        const float SPEED = 0.8f;
        const int SHOOTDELAYMS = 800;

        const int STANDFRAME = 0;
        const int FIRSTWALKFRAME = 1;
        const int WALKFRAMES = 10;
        Vector2 velocity;

        SpriteBatch spriteBatch;
        Texture2D enemyTexture;
        internal Rectangle enemy;
        EnemyType type;

        private int currentFrame = STANDFRAME;
        List<Rectangle> enemyFrames;
        float spriteDirection;
        float shotDirection;

        const int FRAMEDELAYMAXCOUNT = 9;
        int currentFrameDelayCount = 0;

        //bool isShooting = false;
        bool isAlive = true;
        float lastShot = 0;
        internal int health = 100;

        // AI-ish variables
        bool increasing = true;
        const int MAXX = 920;
        const int MINX = 550;

        public Enemy(Game game, SpriteBatch spriteBatch, Rectangle enemy, EnemyType type, float rotation) : base(game)
        {
            #region Initial Setup

            this.spriteBatch = spriteBatch;
            enemyTexture = game.Content.Load<Texture2D>("Images/zombie");
            this.enemy = enemy;
            velocity = new Vector2(0);
            this.type = type;
            spriteDirection = rotation;

            #endregion

            #region Animation Setup

            enemyFrames = new List<Rectangle>();

            enemyFrames.Add(new Rectangle(0, 836, SPRITEWIDTH, 69));
            enemyFrames.Add(new Rectangle(0, 0, SPRITEWIDTH, SPRITEHEIGHT));
            enemyFrames.Add(new Rectangle(0, 85, SPRITEWIDTH, SPRITEHEIGHT));
            enemyFrames.Add(new Rectangle(0, 169, SPRITEWIDTH, SPRITEHEIGHT));
            enemyFrames.Add(new Rectangle(0, 251, SPRITEWIDTH, SPRITEHEIGHT));
            enemyFrames.Add(new Rectangle(0, 332, SPRITEWIDTH, SPRITEHEIGHT));
            enemyFrames.Add(new Rectangle(0, 413, SPRITEWIDTH, SPRITEHEIGHT));
            enemyFrames.Add(new Rectangle(0, 500, SPRITEWIDTH, SPRITEHEIGHT));
            enemyFrames.Add(new Rectangle(0, 589, SPRITEWIDTH, SPRITEHEIGHT));
            enemyFrames.Add(new Rectangle(0, 678, SPRITEWIDTH, SPRITEHEIGHT));
            enemyFrames.Add(new Rectangle(0, 761, SPRITEWIDTH, SPRITEHEIGHT));
            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            if (health > 0)
            {
                Rectangle sourceRec = enemyFrames.ElementAt<Rectangle>(currentFrame);

                Rectangle rotationCorrection; // Draw the texture in the right spot.
                rotationCorrection.X = enemy.X + WIDTH / 2;
                rotationCorrection.Y = enemy.Y + HEIGHT / 2;
                rotationCorrection.Height = enemy.Height;
                rotationCorrection.Width = enemy.Width;

                spriteBatch.Begin();
                if (HUD.isTest)
                {
                    spriteBatch.DrawRectangle(enemy, Color.Red);
                }
                spriteBatch.Draw(enemyTexture,
                    rotationCorrection,
                    sourceRec,
                    Color.White,
                    spriteDirection,  // Rotation         
                    new Vector2(sourceRec.Width / 2f, sourceRec.Height / 2f),
                    SpriteEffects.None,
                    0f
                    );

                spriteBatch.End();
            } else
            {
                Dispose();
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (health <= 0) // Player died
            {
                if (isAlive)
                { // Player has now JUST died.
                    foreach (var comp in Game.Components)
                    {
                        if (comp is ActionScene)
                        {
                            ActionScene actionScene = comp as ActionScene;
                            foreach (object obj in actionScene.Components)
                            {
                                if (obj is Player)
                                {
                                    Player player = obj as Player;
                                    player.Score += 500;
                                }
                            }
                        }
                    }
                    if(type == EnemyType.WALKER)
                    {
                        Treasure treasure = new Treasure(Game, spriteBatch, enemy);
                        Game.Components.Add(treasure);
                    }
                    isAlive = false;

                }
            }  
            if(isAlive && gameTime.TotalGameTime.TotalMilliseconds - lastShot > SHOOTDELAYMS)
            {
                lastShot = (float)gameTime.TotalGameTime.TotalMilliseconds;
                Bullet bullet = new Bullet(Game, spriteBatch, enemy.X + 30, enemy.Y + 20, shotDirection ,BulletType.ENEMY);
                Game.Components.Add(bullet);
            }
            if(type == EnemyType.WALKER)
            {
                shotDirection = (float)Math.PI;
                if(enemy.X == MINX)
                {
                    increasing = true;
                } else if(enemy.X == MAXX)
                {
                    increasing = false;
                }
                if (increasing)
                {
                    enemy.X++;
                }
                else
                {
                    enemy.X--;
                }

                #region Moving

                if (increasing)
                {
                    spriteDirection = (float)Math.PI / 2f;
                }
                else
                {
                    spriteDirection = 3f *(float)Math.PI / 2f;
                }

                currentFrameDelayCount++;
                if (currentFrameDelayCount > FRAMEDELAYMAXCOUNT)
                {
                    currentFrameDelayCount = 0;
                    currentFrame++;
                }
                if (currentFrame > WALKFRAMES)
                    currentFrame = FIRSTWALKFRAME;

                #endregion

            }else if(type == EnemyType.STILL)
            {
                shotDirection = (float)Math.PI / 2f;
            }

            base.Update(gameTime);
        }
    }
}
