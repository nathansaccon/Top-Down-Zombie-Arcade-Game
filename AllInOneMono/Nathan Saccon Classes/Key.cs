using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.XNA;
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
    class Key : DrawableGameComponent
    {
        internal const int WIDTH = 64;
        internal const int HEIGHT = 27;

        internal bool isPickedUp = false;
        internal Color color;

        SpriteBatch spriteBatch;
        internal SoundEffect soundEffect;
        Texture2D texture;
        internal Rectangle key;

        public Key(Game game, SpriteBatch spriteBatch, Texture2D texture, Rectangle key, Color color) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.texture = texture;
            this.key = key;
            this.color = color;
            soundEffect = Game.Content.Load<SoundEffect>("Sounds/keyPickup");
        }

        public override void Draw(GameTime gameTime)
        {

            if (!isPickedUp)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(texture, key, Color.White);
                if (HUD.isTest)
                {
                    spriteBatch.DrawRectangle(key, Color.Red);
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
