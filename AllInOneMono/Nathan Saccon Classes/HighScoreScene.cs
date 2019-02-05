using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    public class HighScoreScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Texture2D background;
        private SpriteFont font;

        const string FILE = "highScores.txt";
        const int SCORECOUNT = 5;

        public HighScoreScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            this.spriteBatch = g.spriteBatch;
            background = g.Content.Load<Texture2D>("Images/highScore");
            font = g.Content.Load<SpriteFont>("Fonts/defaultFont");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            int spaceFromAboveText = 25;
            int place = 1;
            const int TOP = 100;
            const int LEFT = 400;
            foreach (int score in GetHighScores())
            {
                spriteBatch.DrawString(font, place.ToString() + ". " + score.ToString()
                    , new Vector2(LEFT, spaceFromAboveText + TOP), Color.Black);
                spaceFromAboveText += 25;
                place++;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Adds a score to the high score list
        /// </summary>
        /// <param name="score"></param>
        public static void GameFinished(int score)
        {
            List<int> oldHighScores = new List<int>();
            int outputIndex = 0;
            try
            {
                StreamReader reader = new StreamReader(FILE);
                while (!reader.EndOfStream)
                {
                    oldHighScores.Add(Convert.ToInt32(reader.ReadLine()));
                }
                reader.Close();

            }
            catch (Exception)
            {

            }
            oldHighScores.Add(score);
            oldHighScores.Sort();
            oldHighScores.Reverse();
            StreamWriter writer = new StreamWriter(FILE);
            foreach(int num in oldHighScores)
            {
                if(outputIndex < SCORECOUNT)
                {
                    writer.WriteLine(oldHighScores[outputIndex]);
                    outputIndex++;
                }
                else
                {
                    break;
                }
            }
            writer.Close();

        }

        /// <summary>
        /// Returns the currnt list of high scores
        /// </summary>
        /// <returns></returns>
        public static List<int> GetHighScores()
        {
            List<int> highScores = new List<int>();
            try
            {
                StreamReader reader = new StreamReader(FILE);
                while (!reader.EndOfStream)
                {
                    highScores.Add(Convert.ToInt32(reader.ReadLine()));
                }
                reader.Close();

            }
            catch (Exception)
            {

            }
            return highScores;
        }
    }
}
