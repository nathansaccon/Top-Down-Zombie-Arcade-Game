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
    class Gun : DrawableGameComponent
    {
        internal const int WIDTH = 54;
        internal const int HEIGHT = 30;

        internal bool isPickedUp = false;

        SpriteBatch spriteBatch;
        internal SoundEffect soundEffect;
        Texture2D texture;
        internal Rectangle gun;

        public Gun(Game game, SpriteBatch spriteBatch, Rectangle rectangle) : base(game)
        {
            this.spriteBatch = spriteBatch;
            texture = game.Content.Load<Texture2D>("Images/gun");
            gun = rectangle;
            soundEffect = Game.Content.Load<SoundEffect>("Sounds/gunPickup");
        }

        public override void Draw(GameTime gameTime)
        {
            if (!isPickedUp)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(texture, gun, Color.White);
                if (HUD.isTest)
                {
                    spriteBatch.DrawRectangle(gun, Color.Red);
                }

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
