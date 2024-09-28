using Microsoft.Xna.Framework;
using DungeonDweller.Archetecture;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DungeonDweller.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class MainMenuScreen : MenuScreen
    {
        private MenuEntry[] _itemEntries = new MenuEntry[2];

        public MainMenuScreen() : base("Main Menu")
        {
            var playGameMenuEntry = new MenuEntryImage("Play Game", "StartGear");
            var exitMenuEntry = new MenuEntryImage("Exit", "ExitGear");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            _itemEntries[0] = playGameMenuEntry;
            _itemEntries[1] = exitMenuEntry;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);


        }

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(), new CutSceneScreen());
        }

        protected override void OnCancel(PlayerIndex playerIndex)
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

        public override void Draw(GameTime gameTime)
        {

           

            var graphics = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;
            var font = ScreenManager.Font;

            

            spriteBatch.Begin();

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                var menuEntry = MenuEntries[i];
                if (menuEntry.Equals(_itemEntries[0])) menuEntry.Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 3), ScreenManager.GraphicsDevice.Viewport.Width / 2);
                if (menuEntry.Equals(_itemEntries[1])) menuEntry.Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 3) * 2 , ScreenManager.GraphicsDevice.Viewport.Width / 2);


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
            const float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, MenuTitle, titlePosition, titleColor,
                0, titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
