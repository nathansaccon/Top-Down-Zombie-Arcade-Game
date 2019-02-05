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
    class HUD : DrawableGameComponent
    {
        public static bool isTest = true;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Player player;

        const int FRAMEDELAY = 4;

        // Death Screen
        internal bool showDeathScreen = false;
        Texture2D deathScreen;

        // Level Complete
        internal bool showLevelComplete = false;
        Texture2D levelComplete;

        public HUD(Game game, SpriteBatch spriteBatch, Player player, Texture2D deathScreen, Texture2D levelComplete) : base(game)
        {
            this.spriteBatch = spriteBatch;
            font = game.Content.Load<SpriteFont>("Fonts/defaultFont");
            this.player = player;

            this.deathScreen = deathScreen;
            this.levelComplete = levelComplete;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Score: " + player.Score.ToString(), new Vector2(900, 3), Color.Red);
            spriteBatch.DrawString(font, "Health: " + player.health.ToString(), new Vector2(900, 20), Color.Red);

            if (showDeathScreen)
            {
                spriteBatch.Draw(deathScreen, Vector2.Zero, Color.White);
            }

            if (showLevelComplete)
            {
                spriteBatch.Draw(levelComplete, Vector2.Zero, Color.White);
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
