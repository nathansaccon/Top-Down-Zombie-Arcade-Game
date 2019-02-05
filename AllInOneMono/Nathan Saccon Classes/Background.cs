using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.XNA;

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
    class Background : DrawableGameComponent
    {
        internal const int WIDTH = 1000;
        internal const int HEIGHT = 700;

        SpriteBatch spriteBatch;
        Texture2D backgroundTexture;
        Rectangle backgroundContainer = new Rectangle(0, 0, WIDTH, HEIGHT);

        internal List<Rectangle> rigidBodies = new List<Rectangle>();

        public Background(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            backgroundTexture = game.Content.Load<Texture2D>("Images/background");

            rigidBodies.Add(new Rectangle(0, 0, 1000, 1));
            rigidBodies.Add(new Rectangle(0, 0, 1, 700));
            rigidBodies.Add(new Rectangle(0, 700, 1000, 1));
            rigidBodies.Add(new Rectangle(1000, 0, 1, 700));
            rigidBodies.Add(new Rectangle(0, 180, 161, 20));
            rigidBodies.Add(new Rectangle(273, 0, 20, 200));
            rigidBodies.Add(new Rectangle(0, 383, 383, 20));
            rigidBodies.Add(new Rectangle(509, 0, 20, 524));
            rigidBodies.Add(new Rectangle(509, 631, 20, 143));
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, backgroundContainer, Color.White);

            if (HUD.isTest)
            {
                foreach (Rectangle rec in rigidBodies)
                {
                    spriteBatch.DrawRectangle(rec, Color.Red);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
