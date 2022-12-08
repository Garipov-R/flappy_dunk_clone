using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Game
{
    public static class Score
    {
        private static int _ScoreCount;

        public static int ScoreCount 
        {
            get => _ScoreCount;
            private set
            {
                if (value < 0)
                {
                    _ScoreCount = 0;
                }

                _ScoreCount = value;
            } 
        }

        public static Action<int> OnChanged;

        public static void AddScore()
        {
            ScoreCount++;

            OnChanged?.Invoke(ScoreCount);
        }

        public static void ResetScore()
        {
            ScoreCount = 0;

            OnChanged?.Invoke(ScoreCount);
        }
    }
}
