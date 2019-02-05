using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AllInOneMono
{
    public class StartScene : GameScene
    {
        public MenuComponent Menu { get; set; }

        private SpriteBatch spriteBatch;
        string[] menuItems = {"Start Game",
                                "Help",
                                "High Score",
                                "Credit",
                                "Quit"};
        public StartScene(Game game): base(game)
        {
            Game1 g = (Game1)game;

            this.spriteBatch = g.spriteBatch;
            SpriteFont regularFont = g.Content.Load<SpriteFont>("Fonts/regularFont");
            SpriteFont highlightFont = game.Content.Load<SpriteFont>("Fonts/hilightFont");


            Menu = new MenuComponent(game, spriteBatch,regularFont,highlightFont, menuItems);
            this.Components.Add(Menu);
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
