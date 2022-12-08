using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Game
{
    public interface IGameLogic
    {
        public void StartGame();

        public void GameEnd();

        public void Pause();

        public void RestartGame();
    }
}
