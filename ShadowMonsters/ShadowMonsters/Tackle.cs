using ShadowMonsters.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.ShadowMonsters
{
    public class Tackle : IMove
    {
        #region Field Region

        private readonly string name;
        private readonly Target target;
        private readonly MoveType moveType;
        private readonly MoveElement moveElement;
        private bool unlocked;
        private int unlockedAt;
        private int duration;
        private readonly int attack;
        private readonly int defense;
        private readonly int speed;
        private readonly int health;

        #endregion

        #region Property Region

        public string Name
        {
            get { return name; }
        }

        public Target Target
        {
            get { return target; }
        }

        public MoveType MoveType
        {
            get { return moveType; }
        }

        public MoveElement MoveElement
        {
            get { return moveElement; }
        }

        public int UnlockedAt
        {
            get { return unlockedAt; }
            set { unlockedAt = value; }
        }

        public bool Unlocked
        {
            get { return unlocked; }
        }

        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public int Attack
        {
            get { return attack; }
        }

        public int Defense
        {
            get { return defense; }
        }

        public int Speed
        {
            get { return speed; }
        }

        public int Health
        {
            get { return health; }
        }

        #endregion

        #region Constructor region

        public Tackle()
        {
            name = "Tackle";
            target = Target.Enemy;
            moveType = MoveType.Attack;
            moveElement = MoveElement.None;
            duration = 1;
            unlocked = false;
            attack = MoveManager.Random.Next(0, 0);
            defense = MoveManager.Random.Next(0, 0);
            speed = MoveManager.Random.Next(0, 0);
            health = MoveManager.Random.Next(10, 15);
        }

        #endregion

        #region Method Region

        public void Unlock()
        {
            unlocked = true;
        }

        public object Clone()
        {
            Tackle tackle = new Tackle
            {
                unlocked = this.unlocked
            };

            return tackle;
        }

        #endregion
    }
}
