using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Mime;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NAudio.Wave;

namespace Snake
{
    class Music
    {
        private WaveStream mainOutputStream;

        private IWavePlayer player;

        public Music(Audio audio)
        {
            Load(audio);
        }

        public void PlayLoop()
        {
            player.PlaybackStopped += PlayerOnPlaybackStopped;
            PlayOnce();
        }

        private void PlayerOnPlaybackStopped(object sender, StoppedEventArgs stoppedEventArgs)
        {
            PlayOnce();
        }

        public void PlayOnce()
        {
            if (player.PlaybackState == PlaybackState.Playing || player.PlaybackState == PlaybackState.Stopped)
            {
                player.Stop();
                player.Dispose();
                player = new WaveOut();
                player.Init(mainOutputStream);
            }
            player.Play();
        }

        public void Stop()
        {
            player.Stop();
        }

        public void Load(Audio audio)
        {
            mainOutputStream = new WaveFileReader(audio.File);            

            player = new WaveOut();
            player.Init(mainOutputStream);
        }

        ~Music()
        {
            mainOutputStream.Close();
            player.Dispose();
            mainOutputStream.Dispose();
        }
    }

    class Audio
    {
        public readonly string Name;

        public readonly Stream File;

        public Audio(string name, Stream file)
        {
            Name = name;
            File = file;
        }

        public Audio(Stream file)
            : this(nameof(file),file)
        {
            
        }

        ~Audio()
        {
            File.Close();
            File.Dispose();

        }
    }
}
