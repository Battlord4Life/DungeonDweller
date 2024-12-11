using DungeonDweller.Archetecture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DungeonDweller.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    public class VictoryScreen : MenuScreen
    {


        public VictoryScreen() : base("Victory")
        {
            

            var quitGameMenuEntry = new MenuEntry("Quit Game");

          
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            
            MenuEntries.Add(quitGameMenuEntry);
        }

        public override void Draw(GameTime gameTime)
        {
            var graphics = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;
            var font = ScreenManager.Font;

            spriteBatch.Begin();

            //Draw Element

            spriteBatch.End();



            base.Draw(gameTime);




            spriteBatch.Begin();

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            var titlePosition = new Vector2(graphics.Viewport.Width / 2, 100);
            var titleOrigin = font.MeasureString("You collected the secrets of fire!") / 2;
            var titleColor = new Color(192, 192, 192) * TransitionAlpha;
            const float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, "You collected the secrets of fire!", titlePosition, titleColor,
                0, titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";
            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

       

        // This uses the loading screen to transition from the game back to the main menu screen.
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }
    }
}
