using DungeonDweller.Archetecture;

namespace DungeonDweller.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    public class PauseMenuScreen : MenuScreen
    {

        int Level;

        public PauseMenuScreen(int level) : base("Paused")
        {
            Level = level;

            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var Restart = new MenuEntry("Restart Level");
            var HTP = new MenuEntry("How to Play");
            var quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Selected += OnCancel;
            Restart.Selected += RestartLevel;
            HTP.Selected += HowToPlay;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(Restart);
            MenuEntries.Add(HTP);
            MenuEntries.Add(quitGameMenuEntry);
        }



        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";
            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        private void HowToPlay(object sender, PlayerIndexEventArgs e)
        {
            const string message = "WASD to Move, R to Refill";
            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += OnCancel;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        private void RestartLevel(object sender, PlayerIndexEventArgs e)
        {

            ScreenManager.GameSaveState = ScreenManager.Load();

            GameScreen Lev = new Level1();
            switch (Level)
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

            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, Lev);
        }

        // This uses the loading screen to transition from the game back to the main menu screen.
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }
    }
}
