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
        private NAudio.Wave.WaveFileReader wave = null;

        private WaveFileWriter writer = null;

        private NAudio.Wave.DirectSoundOut output = null;

        private Stream file = null;

        private Audio song;


        public Music(Audio audio)
        {
            Load(audio);
        }

        public void PlayLoop()
        {
            output.PlaybackStopped += PlayerOnPlaybackStopped;
            PlayOnce();
        }

        private void PlayerOnPlaybackStopped(object sender, StoppedEventArgs stoppedEventArgs)
        {
            PlayOnce();
        }

        public void PlayOnce()
        {
            //if (wave.CanWrite)
            wave.Position = 0;
            output.Play();
        }

        public void Stop()
        {
            output.PlaybackStopped -= PlayerOnPlaybackStopped;
            output.Stop();
        }

        public void Load(Audio audio)
        {
            file = audio.File;
            song = audio;
            wave = new NAudio.Wave.WaveFileReader(audio.File);

            output = new NAudio.Wave.DirectSoundOut();
            output.Init(new NAudio.Wave.WaveChannel32(wave));
        }

        ~Music()
        {
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Stop();
                output.Dispose();
                output = null;
            }
            if (wave != null)
            {
                wave.Close();
                wave.Dispose();
                wave = null;
            }
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

        public override string ToString()
        {
            return Name;
        }
    }
}
