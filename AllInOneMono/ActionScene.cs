using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NathanSacconFinalProject;

namespace AllInOneMono
{
    public class ActionScene : GameScene
    {
        private SpriteBatch spriteBatch;

        public ActionScene (Game game): base(game)
        {
            Game1 g = (Game1)game;
            this.spriteBatch = g.spriteBatch;

            // Code by Nathan Saccon (start)

            #region Background
            // Background
            Background background = new Background(g, spriteBatch);
            Components.Add(background);
            // Song
            Song song = g.Content.Load<Song>("Sounds/backgroundSong");

            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(song);
            #endregion

            #region Doors

            //Doors
            Texture2D greenDoorTexture = g.Content.Load<Texture2D>("Images/greenDoor");
            Door greenDoor = new Door(g, spriteBatch, greenDoorTexture, new Rectangle(509, 525, 20, 107), Color.Green);
            Components.Add(greenDoor);

            Texture2D redDoorTexture = g.Content.Load<Texture2D>("Images/redDoor");
            Door redDoor = new Door(g, spriteBatch, redDoorTexture, new Rectangle(383, 383, 126, 20), Color.Red);
            Components.Add(redDoor);

            Texture2D blueDoorTexture = g.Content.Load<Texture2D>("Images/blueDoor");
            Door blueDoor = new Door(g, spriteBatch, blueDoorTexture, new Rectangle(162, 180, 111, 20), Color.Blue);
            Components.Add(blueDoor);


            List<Door> allDoors = new List<Door>();
            allDoors.Add(blueDoor);
            allDoors.Add(redDoor);
            allDoors.Add(greenDoor);

            #endregion

            #region Keys

            // Keys
            Texture2D greenKeyTexture = g.Content.Load<Texture2D>("Images/greenKey");
            Key greenKey = new Key(g, spriteBatch, greenKeyTexture, new Rectangle(40, 600, Key.WIDTH, Key.HEIGHT), Color.Green);
            Components.Add(greenKey);

            Texture2D redKeyTexture = g.Content.Load<Texture2D>("Images/redKey");
            Key redKey = new Key(g, spriteBatch, redKeyTexture, new Rectangle(320, 2, Key.WIDTH, Key.HEIGHT), Color.Red);
            Components.Add(redKey);

            Texture2D blueKeyTexture = g.Content.Load<Texture2D>("Images/blueKey");
            Key blueKey = new Key(g, spriteBatch, blueKeyTexture, new Rectangle(162, 2, Key.WIDTH, Key.HEIGHT), Color.Blue);
            Components.Add(blueKey);

            List<Key> allKeys = new List<Key>();
            allKeys.Add(blueKey);
            allKeys.Add(redKey);
            allKeys.Add(greenKey);

            #endregion

            #region Gun

            Gun gun = new Gun(g, spriteBatch, new Rectangle(17, 221, 54, 30));
            Components.Add(gun);

            #endregion

            #region Enemy

            Enemy enemy = new Enemy(g, spriteBatch, new Rectangle(70, 434, 45, 55), EnemyType.STILL, (float)Math.PI / 2f);
            Components.Add(enemy);

            Enemy enemyTwo = new Enemy(g, spriteBatch, new Rectangle(728, 32, 45, 55), EnemyType.WALKER, (float)Math.PI);
            Components.Add(enemyTwo);

            #endregion

            #region Player
            Player player = new Player(g, spriteBatch, background.rigidBodies, allDoors, allKeys);
            Components.Add(player);
            #endregion

            #region HUD

            Texture2D deathScreen = game.Content.Load<Texture2D>("Images/deathScreen");
            Texture2D levelComplete = game.Content.Load<Texture2D>("Images/levelComplete");

            HUD overlay = new HUD(g, spriteBatch, player, deathScreen, levelComplete);
            Components.Add(overlay);
            #endregion

            // (end)
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
