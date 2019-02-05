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
    class Treasure : DrawableGameComponent
    {
        const int HEIGHT = 70;
        const int WIDTH = 70;
        const int SPRITEHEIGHT = 200;
        const int SPRITEWIDTH = 200;


        SpriteBatch spriteBatch;
        Texture2D texture;
        internal SoundEffect soundEffect;
        internal Rectangle treasure;

        List<Rectangle> treasureFrames;
        int currentFrame = 0;
        int currentFrameDelayCount = 0;
        const int FRAMEDELAYMAXCOUNT = 5;

        const int BASEFRAME = 0;
        const int FRAMECOUNT = 8;

        internal bool isPickedUp = false;


        public Treasure(Game game, SpriteBatch spriteBatch, Rectangle rectangle) : base(game)
        {
            this.spriteBatch = spriteBatch;
            texture = Game.Content.Load<Texture2D>("Images/treasure");
            treasure = new Rectangle(rectangle.X, rectangle.Y, WIDTH, HEIGHT); // Setup to spawn treasure where the enemy dies
            soundEffect = Game.Content.Load<SoundEffect>("Sounds/treasurePickup");

            #region Animation Setup

            treasureFrames = new List<Rectangle>();

            treasureFrames.Add(new Rectangle(0, 0, SPRITEWIDTH, SPRITEHEIGHT));
            treasureFrames.Add(new Rectangle(0, 200, SPRITEWIDTH, SPRITEHEIGHT));
            treasureFrames.Add(new Rectangle(0, 400, SPRITEWIDTH, SPRITEHEIGHT));
            treasureFrames.Add(new Rectangle(0, 600, SPRITEWIDTH, SPRITEHEIGHT));
            treasureFrames.Add(new Rectangle(0, 800, SPRITEWIDTH, SPRITEHEIGHT));
            treasureFrames.Add(new Rectangle(0, 1000, SPRITEWIDTH, SPRITEHEIGHT));
            treasureFrames.Add(new Rectangle(0, 1200, SPRITEWIDTH, SPRITEHEIGHT));
            treasureFrames.Add(new Rectangle(0, 1400, SPRITEWIDTH, SPRITEHEIGHT));


            #endregion


        }

        public override void Draw(GameTime gameTime)
        {
            if (!isPickedUp)
            {
                spriteBatch.Begin();
                if (HUD.isTest)
                {
                    spriteBatch.DrawRectangle(treasure, Color.Red);
                }
                spriteBatch.Draw(texture,
                    treasure,
                    treasureFrames.ElementAt<Rectangle>(currentFrame % FRAMECOUNT),
                    Color.White,
                    0f,  // Rotation         
                    new Vector2(0),
                    SpriteEffects.None,
                    0f
                    );

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {

            #region Player Collisions

            if (!isPickedUp)
            {
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
                                Sides playerCollisions = treasure.CheckCollisions(player.player); // player.player is the player rectangle
                                if (playerCollisions != Sides.None)
                                {
                                    player.Score += 1500;
                                    isPickedUp = true;
                                    soundEffect.Play();
                                    HighScoreScene.GameFinished(player.Score + player.health);

                                    foreach (object obj2 in actionScene.Components)
                                    {
                                        if (obj2 is HUD)// 
                                        {
                                            if (obj2 is HUD)
                                            {
                                                HUD hud = obj2 as HUD;
                                                hud.showLevelComplete = true;
                                            }
                                        }

                                    }

                                }
                            }
                            
                        }
                    }
                }
            }

            #endregion

            #region Animation

            currentFrameDelayCount++;
            if (currentFrameDelayCount > FRAMEDELAYMAXCOUNT)
            {
                currentFrameDelayCount = 0;
                currentFrame++;
            }

            #endregion

            base.Update(gameTime);
        }
    }
}
