using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonDweller.Archetecture
{
    public class BGMusicPlayer
    {

        Song[] songs;

        int currenttrack = 0;

        public bool Activated;

        ContentManager _content;

        TimeSpan currentpostion;



        string[] songNames;

        public BGMusicPlayer(string[] titles)
        {
            songNames = titles;
        }

        public void Activate(ScreenManager screen)
        {

            if (_content == null) _content = new ContentManager(screen.Game.Services, "Content");

            for(int i = 0; i < songNames.Length; i++)
            {
                songs[i] = _content.Load<Song>(songNames[i]);
            }

            Activated = true;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(songs[0]);
            currentpostion = TimeSpan.Zero;

        }

        public void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            currentpostion += TimeSpan.FromSeconds(1 / 60);
            if(currentpostion >= songs[currenttrack].Duration)
            {
                currentpostion = TimeSpan.Zero;
            }
            
        }



        public void ChangeSong(int index)
        {
            currenttrack = index;
            MediaPlayer.Play(songs[index], currentpostion);
        }





    }
}
