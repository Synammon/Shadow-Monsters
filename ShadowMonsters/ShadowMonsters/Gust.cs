using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.ShadowMonsters
{
    public class Gust : IMove
    {
        #region Field Region

        private int unlockedAt = 3;
        private bool unlocked = false;
        private int duration = 0;

        #endregion

        #region Property Region

        public string Name => "Gust";

        public Target Target => Target.Enemy;

        public MoveType MoveType => MoveType.Attack;

        public MoveElement MoveElement => MoveElement.Wind;

        public int UnlockedAt { get => unlockedAt; set => unlockedAt = value; }

        public bool Unlocked => unlocked;

        public int Duration { get => duration; set => duration = value; }

        public int Attack => 0;

        public int Defense => 0;

        public int Speed => 0;

        public int Health => MoveManager.Random.Next(20, 30);

        #endregion

        #region Method Region

        public object Clone()
        {
            Gust move = new Gust
            {
                duration = this.duration,
                unlocked = this.unlocked,
                unlockedAt = this.unlockedAt
            };

            return move;
        }

        public void Unlock()
        {
            unlocked = true;
        }

        #endregion
    }
}
