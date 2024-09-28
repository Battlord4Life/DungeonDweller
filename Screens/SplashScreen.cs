using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using DungeonDweller.Archetecture;

namespace DungeonDweller.Screens
{
    public class SplashScreen : GameScreen
    {
        ContentManager _content;
        Texture2D _background;
        TimeSpan _timeSpan;

        public override void Activate()
        {
            base.Activate();

            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _background = _content.Load<Texture2D>("splash");
            _timeSpan = TimeSpan.FromSeconds(2);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            _timeSpan -= gameTime.ElapsedGameTime;
            if (_timeSpan < TimeSpan.Zero) ExitScreen();
        }

        public override void Draw(GameTime gt)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
            ScreenManager.SpriteBatch.End();
        }

    }
}
