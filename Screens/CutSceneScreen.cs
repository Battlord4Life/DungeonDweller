using DungeonDweller.Archetecture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace DungeonDweller.Screens
{
    public class CutSceneScreen : GameScreen
    {

        ContentManager _content;
        Video _video;
        VideoPlayer _videoPlayer;
        bool _isPlaying = false;
        InputAction _skip;

        public CutSceneScreen()
        {
            _videoPlayer = new();
            _skip = new InputAction(new Buttons[] { Buttons.A }, new Keys[] { Keys.Space, Keys.Enter }, true);
        }

        public override void Activate()
        {
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
                
            }
            _video = _content.Load<Video>("liftoff_of_smap");
            _videoPlayer.Play(_video);
            _isPlaying = true;
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (!_isPlaying)
            {
                _videoPlayer.Play(_video);
                _isPlaying = true;
            }

            PlayerIndex player;
            if(_skip.Occurred(input, null,out player))
            {
                ExitScreen();
            }
            
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (_isPlaying && _videoPlayer.PlayPosition >= _video.Duration) ExitScreen();
        }

        public override void Deactivate()
        {
            _videoPlayer.Pause();
            _isPlaying = false;
        }

        public override void Draw(GameTime gameTime)
        {
            if (_isPlaying)
            {
                ScreenManager.SpriteBatch.Begin();
                ScreenManager.SpriteBatch.Draw(_videoPlayer.GetTexture(), Vector2.Zero, Color.White);
                ScreenManager.SpriteBatch.End();
            }
        }

    }
}
