using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            public TextureManager textures;

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

            public Animation(Form window)
            {
                textures = new TextureManager(window);
                Delay = new int[textures.Count];
                for (int i = 0; i < textures.Count; i++)
                {
                    Delay[i] = 0;
                }
                Loops = -1;
            }

            /// <summary>
            /// Количество кадров
            /// </summary>
            public int FrameCount => this.textures.Count;

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
                Paused = true;
            }

            /// <summary>
            /// Продолжить
            /// </summary>
            public void Resume()
            {
                Paused = false;
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
                    while (Paused == false)
                    {
                        PlayOnce();
                    }
                }
                else
                {
                    for (int i = 0; i < loops; i++)
                    {
                        if (Paused == true)
                        {
                            return;
                        }
                        PlayOnce();
                    }
                }
            }

            /// <summary>
            /// Прорисовывает анимацию один раз
            /// </summary>
            /// <returns>True  в случае успешного проигрыша анимации и false в случае неудачного</returns>
            private bool PlayOnce()
            {
                foreach (var id in textures.Textures.Keys)
                {
                    if (Paused == true)
                    {
                        return false;
                    }
                    textures.DrawTexture(id);
                    System.Threading.Thread.Sleep(Delay[id-1]);
                }
                return true;
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
