using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ViewModel
{
    class PlaySound
    {
        public static void Play()
        {
            MediaPlayer mediaPlayer = new MediaPlayer();
            string path = AppDomain.CurrentDomain.BaseDirectory+"noti-sound.mp3";
            Uri uri = new Uri(path);
            mediaPlayer.Open(uri);
            mediaPlayer.Play();
        }
    }
}
