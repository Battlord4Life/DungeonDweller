using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonDweller.Archetecture;
using Microsoft.Xna.Framework.Content;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace DungeonDweller.Screens
{
    // Helper class represents a single entry in a MenuScreen. By default this
    // just draws the entry text string, but it can be customized to display menu
    // entries in different ways. This also provides an event that will be raised
    // when the menu entry is selected.
    public class MenuEntryImage : MenuEntry
    {
        private string _text;
        private float _selectionFade;    // Entries transition out of the selection effect when they are deselected
        private Vector2 _position;    // This is set by the MenuScreen each frame in Update
        private string _imageName;
        private Texture2D _texture;
        private ContentManager _content;
        private ScreenManager ScreenManager;
        private bool _activate = false;


        

       
        public MenuEntryImage(string text, string image) : base(text)
        {
            _text = text;
            _imageName = image;
            
           

        }

        public void Activate(ScreenManager screen)
        { 

            if (_content == null) _content = new ContentManager(screen.Game.Services, "Content");

            _texture = _content.Load<Texture2D>(_imageName);
            _activate = true;
            
        }



        // This can be overridden to customize the appearance.
        public override void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {

            if (!_activate) Activate(screen.ScreenManager);

            var color = isSelected ? Color.Yellow : Color.White;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * 0.05f * _selectionFade;

            float rotation = (float)Math.Sin(time);

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            var screenManager = screen.ScreenManager;
            var spriteBatch = screenManager.SpriteBatch;

            float rot = isSelected ? rotation : 0f;


            var origin = new Vector2(64, 64);
            spriteBatch.Draw(_texture, Position, new Rectangle(0, 0, 128, 128), Color.White, rot, origin, scale, SpriteEffects.None, 0);
        }

        public override int GetHeight(MenuScreen screen)
        {
            return _texture != null ? _texture.Height : 0;
        }

        public override int GetWidth(MenuScreen screen)
        {
            return _texture != null ? _texture.Width: 0;
        }
    }
}
