using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake;

namespace SnakeLevelDesigner
{
    static class Data
    {
        public static int MapWidth;

        public static int MapHeight;

        public static int FinishingScore;

        public static int Speed;

        public static Direction Direction;

        public static Audio BackgroundMusic;

        public static void SetProperties(Coord mapSize, int finishingScore, int speed, Direction direction, Audio music)
        {
            IsInitialized = true;
            MapWidth = mapSize.X;
            MapHeight = mapSize.Y;
            FinishingScore = finishingScore;
            Speed = speed;
            Direction = direction;
            BackgroundMusic = music;
        }

        public static bool IsInitialized = false;

    }
}
