using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.ShadowMonsters
{
    public class Heal : IMove
    {
        #region Field Region

        private int unlockedAt = 3;
        private bool unlocked = false;
        private int duration = 0;

        #endregion

        #region Property Region

        public string Name => "Heal";

        public Target Target => Target.Enemy;

        public MoveType MoveType => MoveType.Heal;

        public MoveElement MoveElement => MoveElement.Light;

        public int UnlockedAt { get => unlockedAt; set => unlockedAt = value; }

        public bool Unlocked => unlocked;

        public int Duration { get => duration; set => duration = value; }

        public int Attack => 0;

        public int Defense => 0;

        public int Speed => 0;

        public int Health => MoveManager.Random.Next(25, 50);

        #endregion

        #region Method Region

        public object Clone()
        {
            Heal move = new Heal
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
