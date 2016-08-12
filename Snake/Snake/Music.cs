using System;
using System.IO;
using NAudio.Wave;

namespace Snake
{
    /// <summary>
    /// Класс для управления музыкой и звуками
    /// </summary>
    class Music : IDisposable
    {
        /// <summary>
        /// Обработчик Wave файлов
        /// </summary>
        private WaveFileReader _wave;

        /// <summary>
        /// Плеер для проигрывания
        /// </summary>
        private DirectSoundOut _player;

        /// <summary>
        /// Откалиброванный звуковой файл
        /// </summary>
        private WaveChannel32 _waveChannel32;

        private bool _canPlay;
        public bool CanPlay {
            get
            {
                return _canPlay;
            }
            set {
                _canPlay = value;
                CanPlayChanged?.Invoke(value);
            }
        }

        public delegate void ValueChanged(dynamic value);

        public event ValueChanged CanPlayChanged;

        /// <summary>
        /// Создание нового экзепляра Music
        /// </summary>
        /// <param name="audio">Аудио файл</param>
        public Music(Audio audio)
        {
            Load(audio);
            CanPlayChanged += value =>
            {
                _waveChannel32.Volume = _canPlay ? 1 : 0;
            };

            CanPlay = true;
        }

        /// <summary>
        /// Загрузка аудио файла в память
        /// </summary>
        /// <param name="audio">Аудио файл</param>
        public void Load(Audio audio)
        {
            
            _wave = new WaveFileReader(audio.File);

            this.audio = audio;
            _player = new DirectSoundOut();
            _waveChannel32 = new WaveChannel32(_wave)
            {
                Volume = _canPlay ? 1 : 0
            };

            _player.Init(_waveChannel32);
        }

        /// <summary>
        /// Играть файл в бесконечном цикле
        /// </summary>
        public void PlayLoop()
        {
            _player.PlaybackStopped += PlayerOnPlaybackStopped;
            PlayOnce();
        }

        private void PlayerOnPlaybackStopped(object sender, StoppedEventArgs stoppedEventArgs)
        {
            PlayOnce();
        }

        /// <summary>
        /// Играть файл один раз
        /// </summary>
        public void PlayOnce()
        {
            try
            {
                _wave.Position = 0;
            }
            catch (ObjectDisposedException ex)
            {
                throw;
            }
            _player.Play();
        }

        public Audio audio;

        /// <summary>
        /// Остановить проигрыш файла
        /// </summary>
        public void Stop()
        {
            _player.PlaybackStopped -= PlayerOnPlaybackStopped;
            _player.Stop();
        }

        

        /// <summary>
        /// Удаление экземпляра Music
        /// </summary>
        ~Music()
        {
            if (_player != null)
            {
                if (_player.PlaybackState == PlaybackState.Playing) _player.Stop();
                _player.Dispose();
                _player = null;
            }
            if (_wave != null)
            {
                _wave.Close();
                _wave.Dispose();
                _wave = null;
            }
        }

        public void Dispose()
        {
            if (_player != null)
            {
                if (_player.PlaybackState == PlaybackState.Playing) _player.Stop();
                _player.Dispose();
                _player = null;
            }
            if (_wave != null)
            {
                _wave.Close();
                _wave.Dispose();
                _wave = null;
            }
        }
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
        public readonly string Name = "Some AudioFile Name";

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
        }

        public Audio(string name, byte[] file)
            : this(file)
        {
            Name = name;
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

        public static Stream ConvertBytesToStream(byte[] bytes)
        {
            Stream s = new MemoryStream(bytes);
            
                return s;
            
        }
    }
}
