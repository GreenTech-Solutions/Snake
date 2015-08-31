using System;
using System.Collections.Generic;
using System.Linq;
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

            /// <summary>
            /// Задержка для каждого кадра
            /// </summary>
            public double[] Delay { get; set; }

            /// <summary>
            /// ID текущего кадра в анимации
            /// </summary>
            public int CurrentFrame { get; }

            /// <summary>
            /// Последнее время обновления
            /// </summary>
            public DateTimeOffset LastTimeUpdate { get; }

            /// <summary>
            /// Приостановлена ли анимация
            /// </summary>
            private bool Paused;

            private int loops;

            #endregion

            #region Methods

            public Animation(int currentFrame)
            {
                CurrentFrame = currentFrame;
            }

            /// <summary>
            /// Количество проигрышей анимации(-1 - бесконечно)
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

            }

            /// <summary>
            /// Перейти к кадру
            /// </summary>
            /// <param name="ID">Номер кадра</param>
            public void JumpToFrame(int ID)
            {

            }

            /// <summary>
            /// Отрисовка текущей текстуры
            /// </summary>
            public void Draw()
            {

            }

            #endregion
        }
    }
}
