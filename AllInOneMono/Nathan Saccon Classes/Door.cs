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
    class Door : DrawableGameComponent
    {
        internal bool isLocked = true;
        internal Color color;

        SpriteBatch spriteBatch;
        internal SoundEffect soundEffect;
        Texture2D texture;

        internal Rectangle door;

        public Door(Game game, SpriteBatch spriteBatch, Texture2D texture, Rectangle door, Color color) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.texture = texture;
            this.door = door;
            this.color = color;
            soundEffect = Game.Content.Load<SoundEffect>("Sounds/door");
        }

        public override void Draw(GameTime gameTime)
        {
            if (isLocked)
            {
                spriteBatch.Begin();
                if (HUD.isTest)
                {
                    spriteBatch.DrawRectangle(door, Color.Red);
                }

                spriteBatch.Draw(texture, door, Color.White);
                spriteBatch.End();
            }
            else
            {
                Dispose();
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
