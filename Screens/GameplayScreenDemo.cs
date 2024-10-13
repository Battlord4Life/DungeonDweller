using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DungeonDweller.Archetecture;
using System.Collections.Generic;
using DungeonDweller.Sprites;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Reflection.Metadata;
using SharpDX.Direct2D1;
using Microsoft.Xna.Framework.Audio;




namespace DungeonDweller.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreenDemo : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private Vector2 _playerPosition = new Vector2(100, 100);
        private Vector2 _enemyPosition = new Vector2(100, 100);

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private InputManager _inputManager;

        private ISprite _backgroundSprite;

        private List<ISprite> _backmidgroundSprites;

        private List<ISprite> _midgroundSprites;

        private List<ISprite> _foremidgroundSprites;

        private List<ISprite> _foregroundSprites;

        RainParticleSystem _rain;


        private bool _custBackground = true;

        private int HeroHealth = 3;

        private float _DamageTimer = 0;

        private SoundEffect _Hurt;

        public GameplayScreenDemo()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            _inputManager = new();
            
            
            _backmidgroundSprites = new();
            _midgroundSprites = new();
            _foremidgroundSprites = new();
            _foregroundSprites = new();
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _inputManager = new();


            

            /*_backgroundSprite = new DungeonDweller.Sprites.Background(new Vector2(0, 0));

            _foremidgroundSprites.Add(new Hero(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2), _inputManager));
            _foremidgroundSprites.Add(new Flame(new Vector2(50, ScreenManager.GraphicsDevice.Viewport.Height / 2), ScreenManager.GraphicsDevice.Viewport.Height / 2, false));
            _foremidgroundSprites.Add(new Flame(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 150, ScreenManager.GraphicsDevice.Viewport.Height / 2), ScreenManager.GraphicsDevice.Viewport.Height / 2, true));
            */

            if (_custBackground) _backgroundSprite.LoadContent(_content);
            foreach (ISprite s in _backmidgroundSprites) s.LoadContent(_content);
            foreach (ISprite s in _midgroundSprites) s.LoadContent(_content);
            foreach (ISprite s in _foremidgroundSprites) s.LoadContent(_content);
            foreach (ISprite s in _foregroundSprites) s.LoadContent(_content);

            _gameFont = _content.Load<SpriteFont>("MenuFont");
            //_Hurt = _content.Load<SoundEffect>("Snap");


            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, false);



            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {

                _inputManager.Update(gameTime);
                // Apply some random jitter to make the enemy move around.
                if (_custBackground) _backgroundSprite.Update(gameTime);
                foreach (ISprite s in _backmidgroundSprites) s.Update(gameTime);
                foreach (ISprite s in _midgroundSprites) s.Update(gameTime);
                foreach (ISprite s in _foremidgroundSprites) s.Update(gameTime);
                foreach (ISprite s in _foregroundSprites) s.Update(gameTime);
                // TODO: Add your update logic here

                CollisionChecker(_foremidgroundSprites, gameTime);

                if (HeroHealth <= 0) _inputManager.Active = false;
                // This game isn't very fun! You could probably improve
                // it by inserting something more interesting in this space :-)
            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                _playerPosition += movement * 8f;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();


            if (_custBackground) _backgroundSprite.Draw(gameTime, spriteBatch);
            foreach (ISprite s in _backmidgroundSprites) s.Draw(gameTime, spriteBatch);
            foreach (ISprite s in _midgroundSprites) s.Draw(gameTime, spriteBatch);
            foreach (ISprite s in _foremidgroundSprites) s.Draw(gameTime, spriteBatch);
            foreach (ISprite s in _foregroundSprites) s.Draw(gameTime, spriteBatch);

            


            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private void CollisionChecker(List<ISprite> Collection, GameTime gameTime)
        {
            for (int i = 0; i < Collection.Count; i++)
            {
                for (int j = i + 1; j < Collection.Count; j++)
                {
                    if (Collection[i].Collides(Collection[j]))
                    {

                       

                    }
                }
            }
        }
    }
}
