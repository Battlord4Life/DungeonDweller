using Microsoft.Xna.Framework;
using DungeonDweller.Archetecture;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.Windows.Forms;

namespace DungeonDweller.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class MainMenuScreen : MenuScreen
    {
        private MenuEntry[] _itemEntries = new MenuEntry[3];


        Song MainMenu;

        bool declarecube = false;
        private ContentManager _content;

        public MainMenuScreen() : base("Main Menu")
        {
            var playGameMenuEntry = new MenuEntryImage("Play Game", "ContinueWheel");
            var NewGameMenuEntry = new MenuEntryImage("New Game", "NewWheel");
            var exitMenuEntry = new MenuEntryImage("Exit", "ExitWheel");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            NewGameMenuEntry.Selected += OnNew;
            exitMenuEntry.Selected += OnCancel;

            _itemEntries[0] = playGameMenuEntry;
            _itemEntries[1] = NewGameMenuEntry;
            _itemEntries[2] = exitMenuEntry;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(NewGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            
            


        }

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            GameSaveState gss = ScreenManager.Load();
            this.ScreenManager.GameSaveState = gss;

            GameScreen Lev = new Level1();
            switch (gss.Level)
            {
                case 2:
                    Lev = new Level2();
                    break;
                case 3:
                    Lev = new Level3();
                    break;
                case 4:
                    Lev = new Level4();
                    break;
                case 5:
                    Lev = new Level5();
                    break;
                case 6:
                    Lev = new Level6();
                    break;
            }
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, Lev);
        }

        private void OnNew(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to start a new game?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += NewGameMenuEntrySelected;

            ScreenManager.AddScreen(confirmExitMessageBox, e.PlayerIndex);
        }

        public override void Activate()
        {
            base.Activate();

            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            MainMenu = _content.Load<Song>("CaveEntranceFin");
            MediaPlayer.Play(MainMenu);
            MediaPlayer.IsRepeating = true;
        }


        private void NewGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            this.ScreenManager.GameSaveState = new GameSaveState();
            ScreenManager.Save(new GameSaveState());
            
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new Level1());
        }

        protected override void OnCancel( PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            
            //declarecube = true;

            //cube.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {

           

            var graphics = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;
            var font = ScreenManager.Font;


           

            spriteBatch.Begin();

            //if(!declarecube) cube = new Cube((DungeonDwellerGame)ScreenManager.Game);
            //declarecube = true;

            //cube.Draw();

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                var menuEntry = MenuEntries[i];
                if (menuEntry.Equals(_itemEntries[0])) menuEntry.Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 4), ScreenManager.GraphicsDevice.Viewport.Width / 2);
                if (menuEntry.Equals(_itemEntries[2])) menuEntry.Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 4) * 2, ScreenManager.GraphicsDevice.Viewport.Width / 2 + (128*3));
                if (menuEntry.Equals(_itemEntries[1])) menuEntry.Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 4) * 3 , ScreenManager.GraphicsDevice.Viewport.Width / 2);


                bool isSelected = IsActive && i == SelectedEntry;
                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            var titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            var titleOrigin = font.MeasureString(MenuTitle) / 2;
            var titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            //spriteBatch.DrawString(font, MenuTitle, titlePosition, titleColor,
            //    0, titleOrigin, titleScale*2, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
