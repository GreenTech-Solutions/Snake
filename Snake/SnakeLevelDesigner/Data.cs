using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake;

namespace SnakeLevelDesigner
{
    public class Data
    {
        public int MapWidth;

        public int MapHeight;

        public int FinishingScore;

        public int Speed;

        public Direction Direction;

        public Audio BackgroundMusic;

        public Data()
        {
            IsInitialized = false;
        }

        public void SetProperties(Coord mapSize, int finishingScore, int speed, Direction direction, Audio music)
        {
            IsInitialized = true;
            MapWidth = mapSize.X;
            MapHeight = mapSize.Y;
            FinishingScore = finishingScore;
            Speed = speed;
            Direction = direction;
            BackgroundMusic = music;
        }

        public bool IsInitialized;

    }
}
