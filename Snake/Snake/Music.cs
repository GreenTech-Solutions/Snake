using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using NAudio.Wave;

namespace Snake
{
    // TODO Создать статический класс для хранения глобальных экземпляров классов
    /// <summary>
    /// Класс для управления музыкой и звуками
    /// </summary>
    static class MusicManager
    {
        private static Dictionary<AudioInfo, Player> PlayerCollection;

        private static bool _mute;
        public static bool Mute {
            get
            {
                return _mute;
            }
            set {
                _mute = value;
                MuteChanged?.Invoke(value);
            }
        }

        public delegate void ValueChanged(dynamic value);

        public static event ValueChanged MuteChanged;

        /// <summary>
        /// Создание нового экзепляра MusicManager
        /// </summary>
        static MusicManager()
        {
            PlayerCollection = new Dictionary<AudioInfo, Player>();
            MusicCollection = new List<AudioInfo>();
            EffectsCollection = new List<AudioInfo>();
            Mute = false;
            MuteChanged += value =>
            {
                if (_mute)
                {
                    foreach (var music in MusicCollection)
                    {
                        PlayerCollection[music].Mute();
                    }
                    foreach (var effect in EffectsCollection)
                    {
                        PlayerCollection[effect].Mute();
                    }
                }
                else
                {
                    foreach (var music in MusicCollection)
                    {
                        PlayerCollection[music].UnMute();
                    }
                    foreach (var effect in EffectsCollection)
                    {
                        PlayerCollection[effect].UnMute();
                    }
                }
            };

        }

        /// <summary>
        /// Загрузка аудио файла в память
        /// </summary>
        /// <param name="audio">Аудио файл</param>
        public static void Add(Audio audio, SoundType soundType)
        {
            var audioInfo = new AudioInfo(audio,soundType);

            switch (soundType)
            {
                case SoundType.Music:
                    if (MusicCollection.Exists(x => audioInfo.Name == x.Name))
                    {
                        return;
                    }
                    MusicCollection.Add(audioInfo);
                    break;
                case SoundType.Effect:
                    if (EffectsCollection.Exists(x => audioInfo.Name == x.Name))
                    {
                        return;
                    }
                    EffectsCollection.Add(audioInfo);
                    break;
            }

            PlayerCollection.Add(audioInfo,new Player(audio));
            if (_mute)
            {
                PlayerCollection[audioInfo].Mute();
            }
        }

        public static void Remove(string name, SoundType soundType)
        {
            AudioInfo audioInfo;
            switch (soundType)
            {
                case SoundType.Music:
                    audioInfo = MusicCollection.Find(x => x.Name == name);
                    break;
                case SoundType.Effect:
                    audioInfo = EffectsCollection.Find(x => x.Name == name);
                    break;
                default:
                    throw new ArgumentException(nameof(soundType));
            }
            PlayerCollection.Remove(audioInfo);
        }

        public static void Play(string name, SoundType soundType)
        {
            try
            {
                AudioInfo audioInfo;
                switch (soundType)
                {
                    case SoundType.Music:
                        audioInfo = MusicCollection.Find(x => x.Name == name);
                        PlayerCollection[audioInfo].PlayLoop();
                        break;
                    case SoundType.Effect:
                        audioInfo = EffectsCollection.Find(x => x.Name == name);
                        PlayerCollection[audioInfo].Play();
                        break;
                    default:
                        throw new ArgumentException(nameof(soundType));
                }
            }
            catch
            {
                AudioInfo audioInfo;
                switch (soundType)
                {
                    case SoundType.Music:
                        audioInfo = MusicCollection.Find(x => x.Name == name);
                        break;
                    case SoundType.Effect:
                        audioInfo = EffectsCollection.Find(x => x.Name == name);
                        break;
                    default:
                        throw new ArgumentException(nameof(soundType));
                }
                PlayerCollection.Remove(audioInfo);
                PlayerCollection.Add(audioInfo, new Player(audioInfo.Audio));

                switch (soundType)
                {
                    case SoundType.Music:
                        audioInfo = MusicCollection.Find(x => x.Name == name);
                        PlayerCollection[audioInfo].PlayLoop();
                        break;
                    case SoundType.Effect:
                        audioInfo = EffectsCollection.Find(x => x.Name == name);
                        PlayerCollection[audioInfo].Play();
                        break;
                }
            }
        }

        public static void Stop(string name, SoundType soundType)
        {
            AudioInfo audioInfo;
            switch (soundType)
            {
                case SoundType.Music:
                    audioInfo = MusicCollection.Find(x => x.Name == name);
                    break;
                case SoundType.Effect:
                    audioInfo = EffectsCollection.Find(x => x.Name == name);
                    break;
                default:
                    throw new ArgumentException(nameof(soundType));
            }
            PlayerCollection[audioInfo].Stop();
        }

        public static void StopAll(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.Music:
                    foreach (var music in MusicCollection)
                    {
                        PlayerCollection[music].Stop();
                    }
                    break;
                case SoundType.Effect:
                    foreach (var effect in EffectsCollection)
                    {
                        PlayerCollection[effect].Stop();
                    }
                    break;
                default:
                    throw new ArgumentException(nameof(soundType));
            }
        }

        public static void StopAll()
        {
            StopAll(SoundType.Effect);
            StopAll(SoundType.Music);
        }

        public static List<AudioInfo> MusicCollection;

        public static List<AudioInfo> EffectsCollection;
    }

    class AudioInfo
    {
        public string Name;

        public SoundType SoundType;

        public Audio Audio;

        public AudioInfo(Audio audio,SoundType soundType)
        {
            this.Audio = audio;
            Name = audio.Name;
            SoundType = soundType;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    interface IPlayer
    {
        void Play();

        void Stop();

        void Mute();

        void UnMute();
    }

    class Player : IPlayer
    {
        /// <summary>
        /// Обработчик Wave файлов
        /// </summary>
        private WaveFileReader _wave;

        /// <summary>
        /// Откалиброванный звуковой файл
        /// </summary>
        private WaveChannel32 _waveChannel32;

        /// <summary>
        /// Плеер для проигрывания
        /// </summary>
        private DirectSoundOut _player;

        public Player(Audio audio)
        {
            audio.File.Position = 0;
            _wave = new WaveFileReader(audio.File);

            _player = new DirectSoundOut();
            _waveChannel32 = new WaveChannel32(_wave)
            {
            };

            _player.Init(_waveChannel32);
        }

        public Player(Audio audio, bool mute)
            : this(audio)
        {
            Mute();
        }

        /// <summary>
        /// Играть файл один раз
        /// </summary>
        public void Play()
        {
            try
            {
                _wave.Position = 0;
            }
            catch 
            {
            }
            _player.Play();
        }

        /// <summary>
        /// Играть файл в бесконечном цикле
        /// </summary>
        public void PlayLoop()
        {
            _player.PlaybackStopped += PlayerOnPlaybackStopped;
            Play();
        }

        /// <summary>
        /// Остановить проигрывание файла
        /// </summary>
        public void Stop()
        {
            _player.PlaybackStopped -= PlayerOnPlaybackStopped;
            _player.Stop();
        }

        public void Mute()
        {
            _waveChannel32.Volume = 0;
        }

        public void UnMute()
        {
            _waveChannel32.Volume = 1;
        }

        private void PlayerOnPlaybackStopped(object sender, StoppedEventArgs stoppedEventArgs)
        {
            Play();
        }

        //public void Dispose()
        //{
        //    if (_player != null)
        //    {
        //        if (_player.PlaybackState == PlaybackState.Playing) _player.Stop();
        //        _player.Dispose();
        //        _player = null;
        //    }
        //    if (_wave != null)
        //    {
        //        _wave.Close();
        //        _wave.Dispose();
        //        _wave = null;
        //    }
        //}
    }

    public enum SoundType
    {
        Music,
        Effect
    }

    /// <summary>
    /// Инкапсуляция аудио файла
    /// </summary>
    [Serializable]
    public class Audio
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name = "Some AudioFile Name";

        /// <summary>
        /// Ссылка на аудио-файл в памяти
        /// </summary>
        public readonly Stream File;

        /// <summary>
        /// Сериализованный музыкальный файл
        /// </summary>
        public byte[] FileBytes;

        /// <summary>
        /// Создаёт новый экземпляр класса Audio с именем файла, из полученного потока
        /// </summary>
        /// <param name="file">Ссылка на поток с файлом</param>
        public Audio(Stream file)
        {
            File = file;
        }

        /// <summary>
        /// Создаёт новый экземпляр класса Audio
        /// </summary>
        /// <param name="name">Имя файла</param>
        /// <param name="file">Ссылка на файл в памяти</param>
        public Audio(string name, Stream file)
            :this(file)
        {
            Name = name;
        }

        public Audio(byte[] file)
        {
            this.FileBytes = file;
            this.File = ConvertBytesToStream(file);
        }

        public Audio(string name, byte[] file)
            : this(file)
        {
            Name = name;
        }

        ~Audio()
        {
            File?.Close();
            File?.Dispose();
        }

        public override string ToString()
        {
            return Name;
        }

        public static Stream ConvertBytesToStream(byte[] bytes)
        {
            Stream s = new MemoryStream(bytes);
            
                return s;
            
        }
    }
}
