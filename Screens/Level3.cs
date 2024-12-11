using DungeonDweller.Archetecture;
using DungeonDweller.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;




namespace DungeonDweller.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class Level3 : GameScreen
    {
        //Used for ScreenManager

        /// <summary>
        /// Gets the Content from Content Manager
        /// </summary>
        private ContentManager _content;

        /// <summary>
        /// Gets the font for the Game
        /// </summary>
        private SpriteFont _gameFont;



        //Resources used for the level

        /// <summary>
        /// Hero to control
        /// </summary>
        private Hero _hero;

        /// <summary>
        /// Exit to go to
        /// </summary>
        private ExitSprite _exit;
        private ExitSprite _exit2;

        /// <summary>
        /// Random 
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// How transparent the screen is
        /// </summary>
        private float _pauseAlpha;

        /// <summary>
        /// Action to pause the game
        /// </summary>
        private readonly InputAction _pauseAction;

        /// <summary>
        /// Class to handle all the player inputs
        /// </summary>
        private InputManager _inputManager;

        /// <summary>
        /// TileMap class that is the background
        /// </summary>
        private Tilemap _backgroundSprite;

        /// <summary>
        /// List of background sprites - Decor
        /// </summary>
        private List<ISprite> _backmidgroundSprites;

        /// <summary>
        /// List of Midground sprites - Decor
        /// </summary>
        private List<ISprite> _midgroundSprites;

        /// <summary>
        /// List of Game Sprites - Gameplay
        /// </summary>
        private List<ISprite> _GameplaySprites;

        /// <summary>
        /// Tileset to draw for the darkness in the level
        /// </summary>
        private LightTileMap _darkTileset;

        /// <summary>
        /// List of sprites for the UI
        /// </summary>
        private List<ISprite> _UISprites;

        /// <summary>
        /// How Zoomed in the level is - Do I Use this?
        /// </summary>
        private float zoom = 1f;

        /// <summary>
        /// Rain particle to resemble falling debris
        /// </summary>
        RainParticleSystem rain;

        private Texture2D _UIBackground;
        private Texture2D _UIPatch;
        private Texture2D _UIBar;

        private Texture2D _UIFlashlight;
        private Texture2D _UILanternSel;
        private Texture2D _UILanternDeSel;
        private Texture2D _UICamera;
        private Texture2D _UICameraBroke;
        private Texture2D _UINightVision;

        /// <summary>
        /// Bool to set if the background is custom - Can be removed
        /// </summary>
        private bool _custBackground = true;

        /// <summary>
        /// How much Health the hero has
        /// </summary>
        private int HeroHealth = 3;

        /// <summary>
        /// How much Health the hero can use
        /// </summary>
        private int MaxHeroHealth = 3;

        /// <summary>
        /// Private Timer for how fast the hero gets hurt
        /// </summary>
        private float _DamageTimer = 0;

        public bool SFX = true;

        public Vector2 PrevHeroPos;

        /// <summary>
        /// Hurt Sound Effect
        /// </summary>
        private SoundEffect _Hurt;

        private SoundEffect _VentHurt;
        private SoundEffect _SpikeHurt;
        private SoundEffect _GearHurt;
        private SoundEffect _Objective;
        private SoundEffect _Pickup;
        private SoundEffect _DoorOpen;

        private SoundEffect _SpikeWarn;
        private SoundEffect _VentWarn;
        private SoundEffect _GearWarn;
        private SoundEffect _PickupWarn;
        private SoundEffect _ObjectiveWarn;



        /// <summary>
        /// Level Gimmick to count lanterns lit
        /// </summary>
        public int LanterLeft = 2;
        public int KeyLeft = 2;


        public bool Refill = false;
        private Song _Song;

        public Level3()
        {
            //Sets up Screen Transistions
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            //Adds the pause action to pause the level
            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            //Creates a new input mangager
            _inputManager = new();


            //Sets up lists for sprites
            _backmidgroundSprites = new();
            _midgroundSprites = new();
            _GameplaySprites = new();
            _UISprites = new();



        }

        // Load graphics content for the game
        public override void Activate()
        {
            //Gets the content if its null
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");



            //Dont know why I do this - Maybe hold over
            _inputManager = new();


            //Sets the background and Darkness Overlay
            _backgroundSprite = new Tilemap("Level3Map.txt");
            _darkTileset = new LightTileMap("LightMap.txt", _backgroundSprite);
            _UIBackground = _content.Load<Texture2D>("UIBackground1280x256");
            _UIBar = _content.Load<Texture2D>("UIBar");
            _UIPatch = _content.Load<Texture2D>("UIPatch");
            _UIFlashlight = _content.Load<Texture2D>("UIFlashLight");
            _UILanternSel = _content.Load<Texture2D>("UILanternSelected");
            _UILanternDeSel = _content.Load<Texture2D>("UILanternUnselected");
            _UICamera = _content.Load<Texture2D>("UICamera");
            _UICameraBroke = _content.Load<Texture2D>("UICameraBroken");
            _UINightVision = _content.Load<Texture2D>("UINightVision");

            //Creates a new hero and exit then adds them to the gameplay sprites
            _hero = new Hero(new Vector2(10, 19), _inputManager);

            _exit = new ExitSprite(new(19, 0));
            _exit2 = new ExitSprite(new(0, 0));

            _GameplaySprites.Add(_exit);
            _GameplaySprites.Add(_exit2);
            _GameplaySprites.Add(_hero);

            //Adds other set sprites that dont change


            _GameplaySprites.Add(new Torch(new(0, 13)));
            _GameplaySprites.Add(new Torch(new(19, 14)));
            _GameplaySprites.Add(new NonLightGearEnemy(new Vector2(9, 13), 9, true, 150));
            _GameplaySprites.Add(new LightGearEnemy(new Vector2(9, 16), 9, true, 100));
            _GameplaySprites.Add(new LightGearEnemy(new Vector2(9, 10), 9, true, 200));

            _GameplaySprites.Add(new MedicPickup(new(12, 19)));
            _GameplaySprites.Add(new MedicPickup(new(7, 19)));
            _GameplaySprites.Add(new FlameKey(new(5, 2)));
            _GameplaySprites.Add(new FlameKey(new(14, 3)));


            //adds the particle system
            rain = new(ScreenManager.Game, new(0, 0, 1280, 10));
            ScreenManager.Game.Components.Add(rain);

            //loads the content for all the lists of sprites
            if (_custBackground) _backgroundSprite.LoadContent(_content);
            foreach (ISprite s in _backmidgroundSprites) s.LoadContent(_content);
            foreach (ISprite s in _midgroundSprites) s.LoadContent(_content);
            foreach (ISprite s in _GameplaySprites) s.LoadContent(_content);
            foreach (ISprite s in _UISprites) s.LoadContent(_content);
            _darkTileset.LoadContent(_content);








            //updates the hero's stats
            _hero.UpdateTile(_backgroundSprite);
            HeroHealth = ScreenManager.GameSaveState.CurrHealth;
            MaxHeroHealth = ScreenManager.GameSaveState.MaxHealth;
            if (ScreenManager.GameSaveState.HasFlash) _hero.Items.Add("Flashlight");
            if (ScreenManager.GameSaveState.HasLantern) _hero.Items.Add("Lantern");
            if (ScreenManager.GameSaveState.HasCamera) _hero.Items.Add("Camera");
            if (ScreenManager.GameSaveState.HasNightVision) _hero.Items.Add("Night Vision");
            _hero.Batteries = ScreenManager.GameSaveState.Batteries;
            _hero.OilBottle = ScreenManager.GameSaveState.Oil;
            _hero.Bulbs = ScreenManager.GameSaveState.Bulbs;
            _hero.SelectedItem = 1;


            //Gets the font and sound
            _gameFont = _content.Load<SpriteFont>("PixelFont");
            //_Hurt = _content.Load<SoundEffect>("Snap");


            _VentHurt = _content.Load<SoundEffect>("VentHurt");
            _SpikeHurt = _content.Load<SoundEffect>("SpikeHurt");
            _GearHurt = _content.Load<SoundEffect>("GearHurt");
            _Objective = _content.Load<SoundEffect>("Objective");
            _Pickup = _content.Load<SoundEffect>("Pickup");
            _DoorOpen = _content.Load<SoundEffect>("DoorOpen");

            _SpikeWarn = _content.Load<SoundEffect>("SpikeWarn");
            _GearWarn = _content.Load<SoundEffect>("GearWarn");
            _ObjectiveWarn = _content.Load<SoundEffect>("ObjectiveWarn");
            _PickupWarn = _content.Load<SoundEffect>("PickupWarn");
            _VentWarn = _content.Load<SoundEffect>("VentWarn");
            _Song = _content.Load<Song>("BattleWornFin");

            MediaPlayer.Play(_Song);
            MediaPlayer.IsRepeating = true;

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.

            int spikesLeft = 10;

            for (int i = 0; i < 20; i++)
            {
                if (RandomHelper.Next(0, 2) == 1 && spikesLeft > 0)
                {
                    spikesLeft--;
                    _backgroundSprite.SetCell(i, 12, 3);
                }
            }

            spikesLeft = 10;

            for (int i = 0; i < 20; i++)
            {
                if (RandomHelper.Next(0, 2) == 1 && spikesLeft > 0)
                {
                    spikesLeft--;
                    _backgroundSprite.SetCell(i, 15, 3);
                }
            }

            //if (RandomHelper.Next(2) == 0)
            //{
            //    _backgroundSprite.SetCell(5, 7, 2);
            //    _backgroundSprite.SetCell(13, 7, 4);
            //}
            //else
            //{
            //    _backgroundSprite.SetCell(5, 7, 4);
            //    _backgroundSprite.SetCell(13, 7, 2);
            //}
            //int Spike1 = RandomHelper.Next(5);
            //int Spike2 = RandomHelper.Next(5);
            //_backgroundSprite.SetCell(5, Spike1, 2);
            //_backgroundSprite.SetCell(13, Spike2, 2);

            //if (RandomHelper.Next(2) == 0)
            //{
            //    _backgroundSprite.SetCell(2, 0, 2);

            //}
            //else
            //{
            //    _backgroundSprite.SetCell(0, 2, 2);
            //}

            //if (RandomHelper.Next(2) == 0)
            //{
            //    _backgroundSprite.SetCell(17, 0, 2);

            //}
            //else
            //{
            //    _backgroundSprite.SetCell(19, 2, 2);
            //}

            //if (RandomHelper.Next(2) == 0)
            //{
            //    _backgroundSprite.SetCell(0, 17, 2);

            //}
            //else
            //{
            //    _backgroundSprite.SetCell(19, 2, 2);
            //}

            //if (RandomHelper.Next(2) == 0)
            //{
            //    _backgroundSprite.SetCell(17, 19, 2);

            //}
            //else
            //{
            //    _backgroundSprite.SetCell(19, 17, 2);
            //}




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
                foreach (ISprite s in _GameplaySprites) s.Update(gameTime);
                foreach (ISprite s in _UISprites) s.Update(gameTime);
                _darkTileset.Update(gameTime);

                if (PrevHeroPos != _hero.Position) SFX = true;
                PrevHeroPos = _hero.Position;

                foreach (ISprite s in _backmidgroundSprites) s.UpdateLightMap(_darkTileset);
                foreach (ISprite s in _midgroundSprites) s.UpdateLightMap(_darkTileset);
                foreach (ISprite s in _GameplaySprites) s.UpdateLightMap(_darkTileset);
                foreach (ISprite s in _UISprites) s.UpdateLightMap(_darkTileset);
                // TODO: Add your update logic here

                //if (!_hero.Items.Contains("Lantern")) if ((_hero.Position / _backgroundSprite._tileWidth) == new Vector2(10, 9)) { ScreenManager.GameSaveState.HasLantern = true; _hero.Items.Add("Lant"); }
                //if (!_hero.Items.Contains("Camera")) if((_hero.Position/_backgroundSprite._tileWidth) == new Vector2(19, 19)) { ScreenManager.GameSaveState.HasCamera = true; _hero.Items.Add("Cam"); }


                CollisionChecker(_GameplaySprites, gameTime);

                if (SFX)
                {

                    foreach (ISprite sprite in _GameplaySprites)
                    {
                        if (sprite.Name == "Flame" || sprite.Name == "Gear")
                        {

                            if ((_hero.TilePosition.X == (sprite.Position / 64).X - 1 && _hero.TilePosition.Y == (sprite.Position / 64).Y) ||
                            (_hero.TilePosition.X == (sprite.Position / 64).X - 1 && _hero.TilePosition.Y == (sprite.Position / 64).Y + 1) ||
                        (_hero.TilePosition.X == (sprite.Position / 64).X + 2 && _hero.TilePosition.Y == (sprite.Position / 64).Y) ||
                        (_hero.TilePosition.X == (sprite.Position / 64).X + 2 && _hero.TilePosition.Y == (sprite.Position / 64).Y + 1) ||
                        (_hero.TilePosition.Y == (sprite.Position / 64).Y - 1 && _hero.TilePosition.X == (sprite.Position / 64).X) ||
                        (_hero.TilePosition.Y == (sprite.Position / 64).Y - 1 && _hero.TilePosition.X == (sprite.Position / 64).X + 1) ||
                        (_hero.TilePosition.Y == (sprite.Position / 64).Y + 2 && _hero.TilePosition.X == (sprite.Position / 64).X) ||
                        (_hero.TilePosition.Y == (sprite.Position / 64).Y + 2 && _hero.TilePosition.X == (sprite.Position / 64).X + 1)) _GearWarn.Play();
                        }

                        if ((_hero.TilePosition.X == (sprite.Position / 64).X - 1 && _hero.TilePosition.Y == (sprite.Position / 64).Y) ||
                            (_hero.TilePosition.X == (sprite.Position / 64).X + 1 && _hero.TilePosition.Y == (sprite.Position / 64).Y) ||
                            (_hero.TilePosition.Y == (sprite.Position / 64).Y - 1 && _hero.TilePosition.X == (sprite.Position / 64).X) ||
                            (_hero.TilePosition.Y == (sprite.Position / 64).Y + 1 && _hero.TilePosition.X == (sprite.Position / 64).X))
                        {


                            if ((sprite.Name == "NightVisionSprite" && !((NightVisionSprite)sprite).Collected) ||
                                (sprite.Name == "LanternSprite" && !((LanternSprite)sprite).Collected) ||
                                (sprite.Name == "FlashLightSprite" && !((FlashlightSprite)sprite).Collected) ||
                                (sprite.Name == "MedicPickup" && !((MedicPickup)sprite).Collected) ||
                                (sprite.Name == "CameraSprite" && !((CameraSprite)sprite).Collected)) _PickupWarn.Play();

                            if (sprite.Name == "Torch" || (sprite.Name == "Exit" && _exit.Open) || (sprite.Name == "FlameKey" && !((FlameKey)sprite).Collected)) _ObjectiveWarn.Play();
                        }

                        if (_backgroundSprite.IsHaz((int)_hero.TilePosition.X + 1, (int)_hero.TilePosition.Y) ||
                            _backgroundSprite.IsHaz((int)_hero.TilePosition.X - 1, (int)_hero.TilePosition.Y) ||
                            _backgroundSprite.IsHaz((int)_hero.TilePosition.X, (int)_hero.TilePosition.Y - 1) ||
                            _backgroundSprite.IsHaz((int)_hero.TilePosition.X, (int)_hero.TilePosition.Y + 1))
                        {
                            if (_backgroundSprite._map[(int)_hero.TilePosition.X, (int)_hero.TilePosition.Y] == 5) _VentWarn.Play(0.125f, 0, 0);
                            else _SpikeWarn.Play(.125f, 0, 0);
                        }

                    }
                    SFX = false;
                }


                Vector2 XY = _hero.Position / _backgroundSprite._tileWidth;

                if (_inputManager.Active)
                {
                    if (_backgroundSprite.IsHaz((int)XY.X, (int)XY.Y))
                    {
                        if (_DamageTimer <= gameTime.TotalGameTime.TotalSeconds)
                        {
                            ScreenManager.GameSaveState.CurrHealth--;
                            if (ScreenManager.GameSaveState.CurrHealth <= 0) ScreenManager.GameSaveState.CurrHealth = ScreenManager.GameSaveState.MaxHealth;


                            HeroHealth--;

                            if (_backgroundSprite._map[(int)_hero.TilePosition.X, (int)_hero.TilePosition.Y] == 3) _SpikeHurt.Play();
                            else _VentHurt.Play();

                            _DamageTimer = (float)gameTime.TotalGameTime.TotalSeconds + 5f;
                        }
                    }
                }

                if (LanterLeft == 0 && !_exit.Open && KeyLeft == 0)
                {
                    _exit.Open = true;
                    _exit2.Open = true;
                    _DoorOpen.Play();
                }

                if (HeroHealth <= 0) _inputManager.Active = false;


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
                ScreenManager.AddScreen(new PauseMenuScreen(3), ControllingPlayer);

            }

        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);



            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;



            Matrix zoomTranslation = Matrix.CreateTranslation(1280 / 2, 1280 / 2, 0);

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
            foreach (ISprite s in _GameplaySprites) s.Draw(gameTime, spriteBatch);
            _darkTileset.Draw(gameTime, spriteBatch);
            foreach (ISprite s in _UISprites) s.Draw(gameTime, spriteBatch);


            spriteBatch.Draw(_UIBackground, new Vector2(0, 1280), Color.White);


            string title = "War-torn Base";

            spriteBatch.DrawString(_gameFont, title, new Vector2(640, _gameFont.MeasureString(title).Y / 2), Color.White, 0, _gameFont.MeasureString(title) / 2, 1.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, HeroHealth.ToString() + "/" + MaxHeroHealth.ToString(), new Vector2(640, 1280 + 55), Color.Crimson, 0, _gameFont.MeasureString(HeroHealth.ToString() + "/" + MaxHeroHealth.ToString()) / 2, 4f, SpriteEffects.None, 0);

            StringBuilder Bar = new();
            float displace = 0;

            for (int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(_gameFont, "#", new Vector2((i * _gameFont.MeasureString("# ").X * 1.5f) + 425, 1280 + 165), _DamageTimer - gameTime.TotalGameTime.TotalSeconds > displace ? Color.Red : Color.White, 0, new Vector2(0, 0), 4F, SpriteEffects.None, 0);
                displace += .5f;
            }

            //spriteBatch.DrawString(_gameFont, ((float)(Math.Max(0, _DamageTimer - gameTime.TotalGameTime.TotalSeconds))).ToString(), new Vector2(0, 50), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);



            spriteBatch.DrawString(_gameFont, ((_hero.Batteries)).ToString() + " Batteries", new Vector2(40, 1280 + 41), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, ((_hero.OilBottle)).ToString() + " Bottles of Oil", new Vector2(20, 1280 + 120), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, ((_hero.Bulbs)).ToString() + " Bulbs", new Vector2(80, 1280 + 200), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);



            foreach (string s in _hero.Items)
            {
                if (s == "None")
                {

                    //spriteBatch.DrawString(_gameFont, s, new Vector2(1280, 1280 + 13), 0 == _hero.SelectedItem ? Color.Yellow : Color.White, 0, new Vector2(_gameFont.MeasureString(s).X + (0 == _hero.SelectedItem ? 30 : 0), 0), 1.25f, SpriteEffects.None, 0);
                }
                if (s == "Flashlight")
                {
                    int Vert = 1280 + (24 * 4);
                    if (_hero.SelectedItem == 1) Vert = 1280 + 8;
                    spriteBatch.Draw(_UIFlashlight, new Vector2((240 * 4) + 2, Vert), Color.White);



                    for (int i = 0; i < 100; i++)
                    {
                        spriteBatch.Draw(_UIBar, new Vector2((240 * 4) + 28, Vert + 20 + i), _hero.FlashlightLeft >= 100 - 1 - i ? Color.Yellow : Color.Transparent);
                    }

                    spriteBatch.Draw(_UIPatch, new Vector2((240 * 4) + 20, 1280 + (38 * 4)), Color.White);

                    //spriteBatch.DrawString(_gameFont, "Flashlight: " + (_hero.FlashlightLeft * 2).ToString("N1") + "%", new Vector2(1280, 1280+62), 1 == _hero.SelectedItem ? Color.Yellow : Color.White, 0, new Vector2(_gameFont.MeasureString("Flashlight: " + _hero.FlashlightLeft.ToString("N1") + "%").X + (1 == _hero.SelectedItem ? 30 : 0), 0), 1.25f, SpriteEffects.None, 0);
                }
                if (s == "Lantern")
                {
                    int Vert = 1280 + (24 * 4) - 16;
                    if (_hero.SelectedItem == 2)
                    {
                        Vert = 1280 + 8;
                        spriteBatch.Draw(_UILanternSel, new Vector2((240 * 4) + 82, Vert), Color.Gray);
                    }
                    else
                    {
                        spriteBatch.Draw(_UILanternDeSel, new Vector2((240 * 4) + 82, Vert), Color.Gray);
                    }

                    spriteBatch.Draw(_UIPatch, new Vector2((240 * 4) + 20 + 80, 1280 + (38 * 4)), Color.White);

                    for (int i = 0; i < 25; i++)
                    {
                        spriteBatch.Draw(_UIBar, new Vector2((240 * 4) + 28 + 80, Vert + 80 + i), _hero.LanternLeft >= 25 - 1 - i ? _hero.LanternLeft < 25 - i + 2 ? _hero.SelectedItem == 2 ? Color.Red : Color.Tan : Color.Tan : Color.Transparent);
                    }


                    //spriteBatch.DrawString(_gameFont, "Lantern: " + _hero.LanternLeft.ToString("N0") + "s of Oil", new Vector2(1280, 1280+ 113), 2 == _hero.SelectedItem ? Color.Yellow : Color.White, 0, new Vector2(_gameFont.MeasureString("Lantern: " + _hero.LanternLeft.ToString("N0") + "s of Oil").X + (2 == _hero.SelectedItem ? 30 : 0), 0), 1.25f, SpriteEffects.None, 0);
                }
                if (s == "Camera")
                {
                    int Vert = 1280 + (24 * 4) - 16;
                    if (_hero.SelectedItem == 3)
                    {
                        Vert = 1280 + 8;

                    }

                    if (_hero.BulbBroken)
                    {
                        spriteBatch.Draw(_UICameraBroke, new Vector2((240 * 4) + 162, Vert), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(_UICamera, new Vector2((240 * 4) + 162, Vert), Color.White);
                    }


                    //spriteBatch.DrawString(_gameFont, "Camera: " + (_hero.BulbBroken ? ("bulb broken") : ("used " + _hero.CameraUse.ToString("N0") + " times")), new Vector2(1280, 1280+ 162), 3 == _hero.SelectedItem ? Color.Yellow : Color.White, 0, new Vector2(_gameFont.MeasureString("Camera: used " + _hero.CameraUse.ToString("N0") + " times").X + (3 == _hero.SelectedItem ? 30 : 0), 0), 1.25f, SpriteEffects.None, 0);
                }
                if (s == "Night Vision")
                {

                    int Vert = 1280 + (24 * 4) - 16;
                    if (_hero.SelectedItem == 4)
                    {
                        Vert = 1280 + 8;

                    }

                    Color nvcolor = new Color((int)((_hero.NightVisionLeft) * 50) + 100, (int)((_hero.NightVisionLeft) * 50) + 100, (int)((_hero.NightVisionLeft) * 50) + 100);

                    spriteBatch.Draw(_UINightVision, new Vector2((240 * 4) + 242, Vert), nvcolor);
                    spriteBatch.Draw(_UIPatch, new Vector2((240 * 4) + 20 + 240, 1280 + (38 * 4)), Color.White);

                    //spriteBatch.DrawString(_gameFont, "Night Vision: " + _hero.NightVisionLeft.ToString("N0") + "s left", new Vector2(1280, 1280 +213), 4 == _hero.SelectedItem ? Color.Yellow : Color.White, 0, new Vector2(_gameFont.MeasureString("Night Vision: " + _hero.NightVisionLeft.ToString("N0") + "s left").X + (4 == _hero.SelectedItem ? 30 : 0), 0), 1.25f, SpriteEffects.None, 0);
                }


            }

            if (HeroHealth <= 0)
            {
                spriteBatch.DrawString(_gameFont, "You Are Dead!", new Vector2((1280 / 2), 1280 - 200), Color.White, 0, new Vector2(63, 10), 3.5f, SpriteEffects.None, 0);
                spriteBatch.DrawString(_gameFont, "ESC while you can!", new Vector2((1280 / 2), 1280 - 100), Color.White, 0, new Vector2(63, 10), 3.5f, SpriteEffects.None, 0);
            }





            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha);

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
                                    _GearHurt.Play();
                                    _DamageTimer = (float)gameTime.TotalGameTime.TotalSeconds + 5f;
                                }
                            }

                        }
                        if (((Collection[i].Name == "Hero" || Collection[j].Name == "Hero") && (Collection[i].Name == "Gear" || Collection[j].Name == "Gear")))
                        {

                            if (_DamageTimer <= gameTime.TotalGameTime.TotalSeconds)
                            {
                                if (_inputManager.Active)
                                {
                                    ScreenManager.GameSaveState.CurrHealth--;
                                    if (ScreenManager.GameSaveState.CurrHealth < 0) ScreenManager.GameSaveState = ScreenManager.Load();
                                    HeroHealth--;
                                    _GearHurt.Play();
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
                                _Pickup.Play();
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
                                _Pickup.Play();
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
                        if (((Collection[i].Name == "Hero" || Collection[j].Name == "Hero") && (Collection[i].Name == "MedicPickup" || Collection[j].Name == "MedicPickup")))
                        {
                            bool col = true;
                            if (Collection[i].Name == "MedicPickup")
                            {
                                col = ((MedicPickup)Collection[i]).Collected;


                            }
                            else
                            {
                                col = ((MedicPickup)Collection[j]).Collected;

                            }
                            if (!col)
                            {
                                ScreenManager.GameSaveState.CurrHealth++;
                                HeroHealth++;
                                if (HeroHealth > MaxHeroHealth) HeroHealth = MaxHeroHealth;
                                _Pickup.Play();
                                if (Collection[i].Name == "MedicPickup")
                                {
                                    ((MedicPickup)Collection[i]).Collected = true;
                                }
                                else
                                {
                                    ((MedicPickup)Collection[j]).Collected = true;
                                }
                            }
                        }

                        if (((Collection[i].Name == "Hero" || Collection[j].Name == "Hero") && (Collection[i].Name == "FlameKey" || Collection[j].Name == "FlameKey")))
                        {
                            bool col = true;
                            if (Collection[i].Name == "FlameKey")
                            {
                                col = ((FlameKey)Collection[i]).Collected;


                            }
                            else
                            {
                                col = ((FlameKey)Collection[j]).Collected;

                            }
                            if (!col)
                            {


                                if (Collection[i].Name == "FlameKey")
                                {
                                    ((FlameKey)Collection[i]).Collected = true;
                                }
                                else
                                {
                                    ((FlameKey)Collection[j]).Collected = true;
                                }
                                _Objective.Play();
                                KeyLeft--;
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
                                _Pickup.Play();
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
                                _Pickup.Play();
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

                                    if (!((Torch)Collection[i]).Active)
                                    {
                                        LanterLeft--;
                                        ((Torch)Collection[i]).Active = true;
                                        _Objective.Play();
                                    }
                                }
                                else
                                {
                                    if (!((Torch)Collection[j]).Active)
                                    {
                                        LanterLeft--;
                                        ((Torch)Collection[j]).Active = true;
                                        _Objective.Play();

                                    }
                                }
                            }
                        }
                        if (((Collection[i].Name == "Hero" || Collection[j].Name == "Hero") && (Collection[i].Name == "Exit" || Collection[j].Name == "Exit")))
                        {
                            if (_exit.Open)
                            {
                                if (_hero.TilePosition == _exit.TilePosition)
                                {
                                    OnLevelEnd();
                                }
                                else
                                {
                                    OnLevelEnd2();
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
            ScreenManager.GameSaveState.Level = 4;
            ScreenManager.Save(ScreenManager.GameSaveState);
            ScreenManager.AddScreen(new Level4(), ControllingPlayer);
            ExitScreen();
        }

        private void OnLevelEnd2()
        {
            ScreenManager.Game.Components.Remove(rain);
            ScreenManager.GameSaveState.Level = 5;
            ScreenManager.Save(ScreenManager.GameSaveState);
            ScreenManager.AddScreen(new Level5(), ControllingPlayer);
            ExitScreen();
        }
    }
}
