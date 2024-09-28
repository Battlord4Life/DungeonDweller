using DungeonDweller.Archetecture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace DungeonDweller.Screens
{
    // The background screen sits behind all the other menu screens.
    // It draws a background image that remains fixed in place regardless
    // of whatever transitions the screens on top of it may be doing.
    public class BackgroundScreenAnim : GameScreen
    {
        private ContentManager _content;
        Video _video;
        VideoPlayer _videoPlayer;
        bool _isPlaying = false;
        private Song _backGround;

        string _videoname;

        public BackgroundScreenAnim(string videoname)
        {
            _videoPlayer = new();
            _videoname = videoname;

        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, whereas if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate()
        {
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            }

            _backGround = _content.Load<Song>("GearShiftWIP");
            _video = _content.Load<Video>(_videoname);
            _videoPlayer.Play(_video);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_backGround);
            _isPlaying = true;
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // Unlike most screens, this should not transition off even if
        // it has been covered by another screen: it is supposed to be
        // covered, after all! This overload forces the coveredByOtherScreen
        // parameter to false in order to stop the base Update method wanting to transition off.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (_videoPlayer.State == MediaState.Stopped)
            {
                _videoPlayer.Play(_video);
                _isPlaying = true;
            }

            


            base.Update(gameTime, otherScreenHasFocus, false);

        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            var viewport = ScreenManager.GraphicsDevice.Viewport;
            var fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            Texture2D? frame = null;

            try
            {
                frame = _videoPlayer.GetTexture();
            }
            catch (InvalidOperationException IOE)
            {

            }

            if (!(frame is null))
            {
                spriteBatch.Begin();


                spriteBatch.Draw(frame, fullscreen,
                    new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

                spriteBatch.End();
            }
        }

    }
}

