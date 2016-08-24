using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Story
    {
        private List<string> LevelPreviews;

        private List<Level> Levels;

        public string FinishingText;

        public Story()
        {
            
        }

        public Story(List<string> levelPreviews, List<Level> levels)
        {
            this.LevelPreviews = levelPreviews;
            this.Levels = levels;
        }

        public void Begin()
        {
            bool isAlive = true;
            int levelCount = Levels.Count;
            Core core = new Core();

            for (int i = 0; i < levelCount; i++)
            {
                DrawLevelPreview(LevelPreviews[i]);
                core.Start(Levels[i]);
                isAlive = core.IsAlive.Value;
                if (!isAlive)
                {
                    return;
                }
            }

            DrawLevelPreview(FinishingText);
        }

        private void DrawLevelPreview(string text)
        {
            Output.Clear();
            Output.WriteCenter(text);
            Console.ReadKey(true);
        }
    }
}
