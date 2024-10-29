using DungeonDweller.Archetecture;
using DungeonDweller.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;




namespace DungeonDweller.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class Level1 : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private Vector2 _playerPosition = new Vector2(100, 100);
        private Vector2 _enemyPosition = new Vector2(100, 100);

        private Hero _hero;

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private InputManager _inputManager;

        private Tilemap _backgroundSprite;

        private List<ISprite> _backmidgroundSprites;

        private List<ISprite> _midgroundSprites;

        private List<ISprite> _foremidgroundSprites;

        private LightTileMap _darkTileset;

        private List<ISprite> _UISprites;

        private float zoom = 1f;

        RainParticleSystem rain;


        private bool _custBackground = true;

        private int HeroHealth = 3;
        private int MaxHeroHealth = 3;

        private float _DamageTimer = 0;

        private SoundEffect _Hurt;

        public Level1()
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
            _UISprites = new();
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _inputManager = new();



            _backgroundSprite = new Tilemap("Level1Map.txt");
            _darkTileset = new LightTileMap("LightMap.txt", _backgroundSprite);
            _hero = new Hero(new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 20) * 10, (ScreenManager.GraphicsDevice.Viewport.Height / 20) * 10), _inputManager);
            _foremidgroundSprites.Add(_hero);
            _foremidgroundSprites.Add(new Flame(new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 20) * 5, (ScreenManager.GraphicsDevice.Viewport.Height / 20) * 10), (ScreenManager.GraphicsDevice.Viewport.Width / 20) * 5, false));
            _foremidgroundSprites.Add(new Flame(new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 20) * 15, (ScreenManager.GraphicsDevice.Viewport.Height / 20) * 10), (ScreenManager.GraphicsDevice.Viewport.Width / 20) * 5, true));
            _foremidgroundSprites.Add(new LanternSprite(new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 20) * 10, (ScreenManager.GraphicsDevice.Viewport.Height / 20) * 9)));
            _foremidgroundSprites.Add(new CameraSprite(new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 20) * 19, (ScreenManager.GraphicsDevice.Viewport.Height / 20) * 19)));
            _foremidgroundSprites.Add(new Torch(new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 20) * 14, (ScreenManager.GraphicsDevice.Viewport.Height / 20) * 19)));
            _foremidgroundSprites.Add(new Torch(new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 20) * 8, (ScreenManager.GraphicsDevice.Viewport.Height / 20) * 3)));
            _foremidgroundSprites.Add(new Torch(new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 20) * 10, (ScreenManager.GraphicsDevice.Viewport.Height / 20) * 7)));


            rain = new(ScreenManager.Game, new(0, 0, 1280, 10));
            ScreenManager.Game.Components.Add(rain);

            if (_custBackground) _backgroundSprite.LoadContent(_content);
            foreach (ISprite s in _backmidgroundSprites) s.LoadContent(_content);
            foreach (ISprite s in _midgroundSprites) s.LoadContent(_content);
            foreach (ISprite s in _foremidgroundSprites) s.LoadContent(_content);
            foreach (ISprite s in _UISprites) s.LoadContent(_content);
            _darkTileset.LoadContent(_content);


            _hero.UpdateTile(_backgroundSprite);
            HeroHealth = ScreenManager.GameSaveState.CurrHealth;
            MaxHeroHealth = ScreenManager.GameSaveState.MaxHealth;
            if (ScreenManager.GameSaveState.HasFlash) _hero.Items.Add("Flashlight");
            if (ScreenManager.GameSaveState.HasLantern) _hero.Items.Add("Lantern");
            if (ScreenManager.GameSaveState.HasCamera) _hero.Items.Add("Camera");
            if (ScreenManager.GameSaveState.HasNightVision) _hero.Items.Add("Night Vision");



            _gameFont = _content.Load<SpriteFont>("MenuFont");
            _Hurt = _content.Load<SoundEffect>("Snap");

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


            if (!otherScreenHasFocus && !coveredByOtherScreen && !ScreenManager.Game.Components.Contains(rain))
            {
                ScreenManager.Game.Components.Add(rain);
            }

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                _darkTileset.Reset();
                _inputManager.Update(gameTime);


                // Apply some random jitter to make the enemy move around.
                //if (_custBackground) _backgroundSprite.Update(gameTime);
                foreach (ISprite s in _backmidgroundSprites) s.Update(gameTime);
                foreach (ISprite s in _midgroundSprites) s.Update(gameTime);
                foreach (ISprite s in _foremidgroundSprites) s.Update(gameTime);
                foreach (ISprite s in _UISprites) s.Update(gameTime);
                _darkTileset.Update(gameTime);

                foreach (ISprite s in _backmidgroundSprites) s.UpdateLightMap(_darkTileset);
                foreach (ISprite s in _midgroundSprites) s.UpdateLightMap(_darkTileset);
                foreach (ISprite s in _foremidgroundSprites) s.UpdateLightMap(_darkTileset);
                foreach (ISprite s in _UISprites) s.UpdateLightMap(_darkTileset);
                // TODO: Add your update logic here

                //if (!_hero.Items.Contains("Lantern")) if ((_hero.Position / _backgroundSprite._tileWidth) == new Vector2(10, 9)) { ScreenManager.GameSaveState.HasLantern = true; _hero.Items.Add("Lant"); }
                //if (!_hero.Items.Contains("Camera")) if((_hero.Position/_backgroundSprite._tileWidth) == new Vector2(19, 19)) { ScreenManager.GameSaveState.HasCamera = true; _hero.Items.Add("Cam"); }
                if ((_hero.Position / _backgroundSprite._tileWidth) == new Vector2(0, 0))
                {
                    OnLevelEnd();
                }

                CollisionChecker(_foremidgroundSprites, gameTime);

                Vector2 XY = _hero.Position / _backgroundSprite._tileWidth;

                if (_inputManager.Active)
                {
                    if (_backgroundSprite.IsHaz((int)XY.X, (int)XY.Y))
                    {
                        if (_DamageTimer <= gameTime.TotalGameTime.TotalSeconds)
                        {
                            ScreenManager.GameSaveState.CurrHealth--;
                            if (ScreenManager.GameSaveState.CurrHealth < 0) ScreenManager.GameSaveState.CurrHealth = ScreenManager.GameSaveState.MaxHealth;

                            HeroHealth--;
                            _Hurt.Play();
                            _DamageTimer = (float)gameTime.TotalGameTime.TotalSeconds + 5f;
                        }
                    }
                }
                if (HeroHealth <= 0) _inputManager.Active = false;
                // This game isn't very fun! You could probably improve
                // it by inserting something more interesting in this space :-)

                if (_hero.UpdateSave)
                {
                    _hero.UpdateSave = false;
                    ScreenManager.GameSaveState.Batteries = _hero.Batteries;
                    ScreenManager.GameSaveState.Bulbs = _hero.Bulbs;
                    ScreenManager.GameSaveState.Oil = _hero.OilBottle;

                }
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
                ScreenManager.Game.Components.Remove(rain);
                ScreenManager.AddScreen(new PauseMenuScreen(1), ControllingPlayer);

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



            Matrix zoomTranslation = Matrix.CreateTranslation(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2, 0);

            zoom += RandomHelper.NextFloat(-.001f, .001f);
            if (zoom >= 1.25f) zoom = 1.25f;
            if (zoom <= .75f) zoom = 0.75f;
            Matrix zoomScale = Matrix.CreateScale(zoom);

            Matrix zoomTransform = zoomTranslation * zoomScale * Matrix.Invert(zoomTranslation);
            //transformMatrix: zoomTransform
            spriteBatch.Begin();


            if (_custBackground) _backgroundSprite.Draw(gameTime, spriteBatch);
            foreach (ISprite s in _backmidgroundSprites) s.Draw(gameTime, spriteBatch);
            foreach (ISprite s in _midgroundSprites) s.Draw(gameTime, spriteBatch);
            foreach (ISprite s in _foremidgroundSprites) s.Draw(gameTime, spriteBatch);
            _darkTileset.Draw(gameTime, spriteBatch);
            foreach (ISprite s in _UISprites) s.Draw(gameTime, spriteBatch);




            spriteBatch.DrawString(_gameFont, "Level 1", new Vector2(0, 0), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, HeroHealth.ToString() + "/" + MaxHeroHealth.ToString(), new Vector2(0, 20), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, ((float)(Math.Max(0, _DamageTimer - gameTime.TotalGameTime.TotalSeconds))).ToString(), new Vector2(0, 40), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, ((_hero.Batteries)).ToString() + " Batteries", new Vector2(0, 60), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, ((_hero.OilBottle)).ToString() + " Bottles of Oil", new Vector2(0, 80), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, ((_hero.Bulbs)).ToString() + " Bulbs", new Vector2(0, 100), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);

            int displace = 0;

            foreach (string s in _hero.Items)
            {
                if (s == "None")
                {
                    spriteBatch.DrawString(_gameFont, s, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, displace), 0 == _hero.SelectedItem ? Color.Yellow : Color.White, 0, new Vector2(_gameFont.MeasureString(s).X + ((displace / 20) == _hero.SelectedItem ? 30 : 0), 0), 1.5f, SpriteEffects.None, 0);
                }
                if (s == "Flashlight")
                {
                    spriteBatch.DrawString(_gameFont, "Flashlight: " + (_hero.FlashlightLeft * 2).ToString("N2") + "%", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, displace), 1 == _hero.SelectedItem ? Color.Yellow : Color.White, 0, new Vector2(_gameFont.MeasureString("Flashlight: " + _hero.FlashlightLeft.ToString("N2") + "%").X + ((displace / 20) == _hero.SelectedItem ? 30 : 0), 0), 1.5f, SpriteEffects.None, 0);
                }
                if (s == "Lantern")
                {
                    spriteBatch.DrawString(_gameFont, "Lantern: " + _hero.LanternLeft.ToString("N2") + "s of Oil", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, displace), 2 == _hero.SelectedItem ? Color.Yellow : Color.White, 0, new Vector2(_gameFont.MeasureString("Lantern: " + _hero.LanternLeft.ToString("N2") + "s of Oil").X + ((displace / 20) == _hero.SelectedItem ? 30 : 0), 0), 1.5f, SpriteEffects.None, 0);
                }
                if (s == "Camera")
                {
                    spriteBatch.DrawString(_gameFont, "Camera: " + (_hero.BulbBroken ? ("bulb broken") : ("used " + _hero.CameraUse.ToString("N2") + " times")), new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, displace), 3 == _hero.SelectedItem ? Color.Yellow : Color.White, 0, new Vector2(_gameFont.MeasureString("Camera: used " + _hero.CameraUse.ToString("N2") + " times").X + ((displace / 20) == _hero.SelectedItem ? 30 : 0), 0), 1.5f, SpriteEffects.None, 0);
                }
                if (s == "Night Vision")
                {
                    spriteBatch.DrawString(_gameFont, "Night Vision: " + _hero.NightVisionLeft.ToString("N2") + "s left", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, displace), 4 == _hero.SelectedItem ? Color.Yellow : Color.White, 0, new Vector2(_gameFont.MeasureString("Night Vision: " + _hero.NightVisionLeft.ToString("N2") + "s left").X + ((displace / 20) == _hero.SelectedItem ? 30 : 0), 0), 1.5f, SpriteEffects.None, 0);
                }

                displace += 20;
            }

            if (HeroHealth <= 0)
            {
                spriteBatch.DrawString(_gameFont, "You Are Dead!", new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2), ScreenManager.GraphicsDevice.Viewport.Height - 200), Color.White, 0, new Vector2(63, 10), 3.5f, SpriteEffects.None, 0);
                spriteBatch.DrawString(_gameFont, "ESC while you can!", new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2), ScreenManager.GraphicsDevice.Viewport.Height - 100), Color.White, 0, new Vector2(63, 10), 3.5f, SpriteEffects.None, 0);
            }



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

                        if (((Collection[i].Name == "Hero" || Collection[j].Name == "Hero") && (Collection[i].Name == "Flame" || Collection[j].Name == "Flame")))
                        {

                            if (_DamageTimer <= gameTime.TotalGameTime.TotalSeconds)
                            {
                                if (_inputManager.Active)
                                {
                                    ScreenManager.GameSaveState.CurrHealth--;
                                    if (ScreenManager.GameSaveState.CurrHealth < 0) ScreenManager.GameSaveState = ScreenManager.Load();
                                    HeroHealth--;
                                    _Hurt.Play();
                                    _DamageTimer = (float)gameTime.TotalGameTime.TotalSeconds + 5f;
                                }
                            }

                        }
                        if (((Collection[i].Name == "Hero" || Collection[j].Name == "Hero") && (Collection[i].Name == "FlashLightSprite" || Collection[j].Name == "FlashLightSprite")))
                        {
                            bool col = true;
                            if (Collection[i].Name == "FlashLightSprite")
                            {
                                col = ((FlashlightSprite)Collection[i]).Collected;
   
                                
                            }
                            else
                            {
                                col = ((FlashlightSprite)Collection[j]).Collected;
                                
                            }
                            if (!col)
                            {
                                ScreenManager.GameSaveState.HasFlash = true;
                                ScreenManager.GameSaveState.Batteries = _hero.Batteries;
                                if (_hero.Items.Contains("Flashlight"))
                                {
                                    _hero.Batteries += 2;
                                }
                                else
                                {
                                    _hero.Items.Add("Flashlight");
                                }
                                if (Collection[i].Name == "FlashLightSprite")
                                {
                                    ((FlashlightSprite)Collection[i]).Collected = true;
                                }
                                else
                                {
                                    ((FlashlightSprite)Collection[j]).Collected = true;
                                }
                            }
                        }
                        if (((Collection[i].Name == "Hero" || Collection[j].Name == "Hero") && (Collection[i].Name == "LanternSprite" || Collection[j].Name == "LanternSprite")))
                        {
                            bool col = true;
                            if (Collection[i].Name == "LanternSprite")
                            {
                                col = ((LanternSprite)Collection[i]).Collected;


                            }
                            else
                            {
                                col = ((LanternSprite)Collection[j]).Collected;

                            }
                            if (!col)
                            {
                                ScreenManager.GameSaveState.HasLantern = true;
                                ScreenManager.GameSaveState.Oil = _hero.OilBottle;
                                if (_hero.Items.Contains("Lantern"))
                                {
                                    _hero.OilBottle++;
                                }
                                else
                                {
                                    _hero.Items.Add("Lantern");

                                }
                                if (Collection[i].Name == "LanternSprite")
                                {
                                    ((LanternSprite)Collection[i]).Collected = true;
                                }
                                else
                                {
                                    ((LanternSprite)Collection[j]).Collected = true;
                                }
                            }
                        }
                        if (((Collection[i].Name == "Hero" || Collection[j].Name == "Hero") && (Collection[i].Name == "CameraSprite" || Collection[j].Name == "CameraSprite")))
                        {
                            bool col = true;
                            if (Collection[i].Name == "CameraSprite")
                            {
                                col = ((CameraSprite)Collection[i]).Collected;


                            }
                            else
                            {
                                col = ((CameraSprite)Collection[j]).Collected;

                            }
                            if (!col)
                            {
                                ScreenManager.GameSaveState.HasCamera = true;
                                ScreenManager.GameSaveState.Bulbs = _hero.Bulbs;
                                if (_hero.Items.Contains("Camera"))
                                {
                                    _hero.Bulbs += 2;
                                }
                                else
                                {
                                    _hero.Items.Add("Camera");
                                }
                                if (Collection[i].Name == "CameraSprite")
                                {
                                    ((CameraSprite)Collection[i]).Collected = true;
                                }
                                else
                                {
                                    ((CameraSprite)Collection[j]).Collected = true;
                                }
                            }

                        }
                        if (((Collection[i].Name == "Hero" || Collection[j].Name == "Hero") && (Collection[i].Name == "NightVisionSprite" || Collection[j].Name == "NightVisionSprite")))
                        {
                            bool col = true;
                            if (Collection[i].Name == "NightVisionSprite")
                            {
                                col = ((NightVisionSprite)Collection[i]).Collected;


                            }
                            else
                            {
                                col = ((NightVisionSprite)Collection[j]).Collected;

                            }
                            if (!col)
                            {
                                ScreenManager.GameSaveState.HasNightVision = true;
                                ScreenManager.GameSaveState.Batteries = _hero.Batteries;
                                if (_hero.Items.Contains("Night Vision"))
                                {
                                    _hero.Batteries += 4;
                                }
                                else
                                {
                                    _hero.Items.Add("Night Vision");
                                }
                                if (Collection[i].Name == "NightVisionSprite")
                                {
                                    ((NightVisionSprite)Collection[i]).Collected = true;
                                }
                                else
                                {
                                    ((NightVisionSprite)Collection[j]).Collected = true;
                                }
                            }
                        }
                        if (((Collection[i].Name == "Hero" || Collection[j].Name == "Hero") && (Collection[i].Name == "Torch" || Collection[j].Name == "Torch")))
                        {
                            if (_hero.SelectedItem == 2 && _hero.ToolActive)
                            {
                                if (Collection[i].Name == "Torch")
                                {
                                    ((Torch)Collection[i]).Active = true;
                                }
                                else
                                {
                                    ((Torch)Collection[j]).Active = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnLevelEnd()
        {
            ScreenManager.Game.Components.Remove(rain);
            ScreenManager.GameSaveState.Level = 2;
            ScreenManager.Save(ScreenManager.GameSaveState);
            ScreenManager.AddScreen(new Level2(), ControllingPlayer);
            ExitScreen();
        }
    }
}
