using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace StrawberryGameEngine
{
    namespace Video
    {
        public class Animation
        {
            #region Vars

            /// <summary>
            /// Текстуры в анимации
            /// </summary>
            private Texture[] AllTextures;

            private TextureManager textures;

            /// <summary>
            /// Задержка для каждого кадра (миллисекунды)
            /// </summary>
            public int[] Delay { get; set; }

            private int _current;

            /// <summary>
            /// ID текущего кадра в анимации
            /// </summary>
            public int CurrentFrame => _current;

            /// <summary>
            /// Последнее время обновления
            /// </summary>
            public DateTimeOffset LastTimeUpdate { get; }

            /// <summary>
            /// Приостановлена ли анимация
            /// </summary>
            private bool Paused;

            // Количество проигрышей анимации
            private int loops;

            /// <summary>
            /// Количество проигрышей анимации(-1/0 - бесконечно)
            /// </summary>
            public int Loops
            {
                set
                {
                    if (value == 0)
                    {
                        this.loops = -1;
                        return;
                    }
                    this.loops = value;
                }
                get { return loops; }
            }

            #endregion

            #region Methods

            public Animation()
            {
            }

            /// <summary>
            /// Количество кадров
            /// </summary>
            public int FrameCount => this.AllTextures.Count();

            /// <summary>
            /// Обновить
            /// </summary>
            public void Update()
            {
            }

            /// <summary>
            /// Приостановить
            /// </summary>
            public void Pause()
            {

            }

            /// <summary>
            /// Продолжить
            /// </summary>
            public void Resume()
            {

            }

            /// <summary>
            /// Вернуться к первому кадру
            /// </summary>
            public void Reset()
            {
                _current = 1;
            }

            /// <summary>
            /// Перейти к кадру
            /// </summary>
            /// <param name="ID">Номер кадра</param>
            public void JumpToFrame(int ID)
            {
                _current = ID;
            }

            /// <summary>
            /// Проиграть анимацию
            /// </summary>
            public void Play()                        
            {
                if (loops == -1)
                {
                    while (true)
                    {
                        PlayOnce();
                    }
                }
                else
                {
                    for (int i = 0; i < loops; i++)
                    {
                        PlayOnce();
                    }
                }
            }

            /// <summary>
            /// Прорисовывает анимацию один раз
            /// </summary>
            private void PlayOnce()
            {
                foreach (var id in textures.Textures.Keys)
                {
                    textures.DrawTexture(id);
                    System.Threading.Thread.Sleep(Delay[id]);
                }
            }

            /// <summary>
            /// Отрисовка текущей текстуры
            /// </summary>
            public void Draw()
            {
                textures.DrawTexture(_current);
            }

            #endregion
        }
    }
}
