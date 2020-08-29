using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.ShadowMonsters
{
    public enum ShadowMonsterElement
    {
        Dark, Earth, Fire, Light, Water, Wind
    }

    public class ShadowMonster
    {
        #region Field Region

        private static readonly Random random = new Random();

        private Texture2D texture;
        private string name;
        private string displayName;
        private ShadowMonsterElement element;
        private int level;
        private long experience;
        private int costToBuy;
        private int speed;
        private int attack;
        private int defense;
        private int health;
        private int currentHealth;
        public Rectangle Source { get; private set; }

        private readonly List<IMove> effects;
        private Dictionary<string, IMove> knownMoves;
        private bool isAsleep;
        private bool isPoisoned;
        private bool isConfused;
        private bool isParalyzed;

        #endregion

        #region Property Region

        public string Name
        {
            get { return name; }
        }

        public string DisplayName
        {
            get { return displayName; }
        }

        public int Level
        {
            get { return level; }
            set { level = (int)MathHelper.Clamp(value, 1, 100); }
        }

        public long Experience
        {
            get { return experience; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Dictionary<string, IMove> KnownMoves
        {
            get { return knownMoves; }
        }

        public ShadowMonsterElement Element
        {
            get { return element; }
        }

        public List<IMove> Effects
        {
            get { return effects; }
        }

        public static Random Random
        {
            get { return random; }
        }

        public int BaseAttack
        {
            get { return attack; }
        }

        public int BaseDefense
        {
            get { return defense; }
        }

        public int BaseSpeed
        {
            get { return speed; }
        }

        public int BaseHealth
        {
            get { return health; }
        }

        public int CurrentHealth
        {
            get { return currentHealth; }
        }

        public bool Alive
        {
            get { return (currentHealth > 0); }
        }

        public bool IsAsleep { get => isAsleep; set => isAsleep = value; }
        public bool IsPoisoned { get => isPoisoned; set => isPoisoned = value; }
        public bool IsConfused { get => isConfused; set => isConfused = value; }
        public bool IsParalyzed { get => isParalyzed; set => isParalyzed = value; }
        public bool IsFainted { get; set; }
        #endregion

        #region Constructor Region

        private ShadowMonster()
        {
            level = 1;
            knownMoves = new Dictionary<string, IMove>();
            effects = new List<IMove>();
            IsAsleep = false;
            IsParalyzed = false;
            IsPoisoned = false;
        }

        #endregion

        #region Method region

        public void ResoleveMove(IMove move, ShadowMonster target)
        {
            bool found;
            switch (move.Target)
            {
                case Target.Self:
                    if (move.MoveType == MoveType.Buff)
                    {
                        found = false;
                        for (int i = 0; i < effects.Count; i++)
                        {
                            if (effects[i].Name == move.Name)
                            {
                                effects[i].Duration += move.Duration;
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            effects.Add((IMove)move.Clone());
                        }
                    }
                    else if (move.MoveType == MoveType.Heal)
                    {
                        currentHealth += move.Health;
                        if (currentHealth > health)
                        {
                            currentHealth = health;
                        }
                    }

                    break;
                case Target.Enemy:
                    if (move.MoveType == MoveType.Debuff)
                    {
                        found = false;
                        for (int i = 0; i < target.Effects.Count; i++)
                        {
                            if (target.Effects[i].Name == move.Name)
                            {
                                target.Effects[i].Duration += move.Duration;
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            target.Effects.Add((IMove)move.Clone());
                        }
                    }
                    else if (move.MoveType == MoveType.Attack)
                    {
                        float modifier = GetMoveModifier(move.MoveElement, target.Element);

                        float tDamage = GetAttack() + move.Health * modifier - target.GetDefense();

                        if (tDamage < 1f)
                        {
                            tDamage = 1f;
                        }

                        target.ApplyDamage((int)tDamage);
                    }

                    break;
            }
        }

        public static float GetMoveModifier(MoveElement moveElement, ShadowMonsterElement avatarElement)
        {
            float modifier = 1f;

            switch (moveElement)
            {
                case MoveElement.Dark:
                    if (avatarElement == ShadowMonsterElement.Light)
                    {
                        modifier += .25f;
                    }
                    else if (avatarElement == ShadowMonsterElement.Wind)
                    {
                        modifier -= .25f;
                    }

                    break;
                case MoveElement.Earth:
                    if (avatarElement == ShadowMonsterElement.Water)
                    {
                        modifier += .25f;
                    }
                    else if (avatarElement == ShadowMonsterElement.Wind)
                    {
                        modifier -= .25f;
                    }

                    break;
                case MoveElement.Fire:
                    if (avatarElement == ShadowMonsterElement.Wind)
                    {
                        modifier += .25f;
                    }
                    else if (avatarElement == ShadowMonsterElement.Water)
                    {
                        modifier -= .25f;
                    }

                    break;
                case MoveElement.Light:
                    if (avatarElement == ShadowMonsterElement.Dark)
                    {
                        modifier += .25f;
                    }
                    else if (avatarElement == ShadowMonsterElement.Earth)
                    {
                        modifier -= .25f;
                    }

                    break;
                case MoveElement.Water:
                    if (avatarElement == ShadowMonsterElement.Fire)
                    {
                        modifier += .25f;
                    }
                    else if (avatarElement == ShadowMonsterElement.Earth)
                    {
                        modifier -= .25f;
                    }

                    break;
                case MoveElement.Wind:
                    if (avatarElement == ShadowMonsterElement.Light)
                    {
                        modifier += .25f;
                    }
                    else if (avatarElement == ShadowMonsterElement.Earth)
                    {
                        modifier -= .25f;
                    }

                    break;

            }

            return modifier;
        }

        public void ApplyDamage(int tDamage)
        {
            currentHealth -= tDamage;

            if (currentHealth > health)
            {
                currentHealth = health;
            }
        }

        public void Heal(int amount)
        {
            currentHealth += amount;

            if (currentHealth > BaseHealth)
            {
                currentHealth = BaseHealth;
            }
        }

        public void Update()
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Duration--;

                if (effects[i].Duration < 1)
                {
                    effects.RemoveAt(i);
                    i--;
                }
            }
        }

        public int GetAttack()
        {
            int attackMod = 0;

            foreach (IMove move in effects)
            {
                if (move.MoveType == MoveType.Buff)
                {
                    attackMod += move.Attack;
                }

                if (move.MoveType == MoveType.Debuff)
                {
                    attackMod -= move.Attack;
                }
            }

            return attack + attackMod;
        }

        public int GetDefense()
        {
            int defenseMod = 0;

            foreach (IMove move in effects)
            {
                if (move.MoveType == MoveType.Buff)
                {
                    defenseMod += move.Defense;
                }

                if (move.MoveType == MoveType.Debuff)
                {
                    defenseMod -= move.Defense;
                }
            }

            return defense + defenseMod;
        }

        public int GetSpeed()
        {
            int speedMod = 0;

            foreach (IMove move in effects)
            {
                if (move.MoveType == MoveType.Buff)
                {
                    speedMod += move.Speed;
                }

                if (move.MoveType == MoveType.Debuff)
                {
                    speedMod -= move.Speed;
                }
            }

            return speed + speedMod;
        }

        public int GetHealth()
        {
            int healthMod = 0;

            foreach (IMove move in effects)
            {
                if (move.MoveType == MoveType.Buff)
                {
                    healthMod += move.Health;
                }

                if (move.MoveType == MoveType.Debuff)
                {
                    healthMod += move.Health;
                }
            }

            return health + healthMod;
        }

        public void StartCombat()
        {
            effects.Clear();
            //currentHealth = health;
        }

        public long WinBattle(ShadowMonster target)
        {
            int levelDiff = target.Level - level;
            long expGained;
            if (levelDiff <= -10)
            {
                expGained = 10;
            }
            else if (levelDiff <= -5)
            {
                expGained = (long)(100f * (float)Math.Pow(2, levelDiff));
            }
            else if (levelDiff <= 0)
            {
                expGained = (long)(50f * (float)Math.Pow(2, levelDiff));
            }
            else if (levelDiff <= 5)
            {
                expGained = (long)(5f * (float)Math.Pow(2, levelDiff));
            }
            else if (levelDiff <= 10)
            {
                expGained = (long)(10f * (float)Math.Pow(2, levelDiff));
            }
            else
            {
                expGained = (long)(50f * (float)Math.Pow(2, target.Level));
            }

            experience += expGained;
            return expGained;
        }

        public long LoseBattle(ShadowMonster target)
        {
            long expGained = (long)((float)WinBattle(target) * .5f);
            experience += expGained;

            return expGained;
        }

        public bool CheckLevelUp()
        {
            bool leveled = false;

            if (experience >= 50 * (1 + (long)Math.Pow((level - 1), 2.5)))
            {
                leveled = true;
                level++;
            }

            return leveled;
        }

        public object Clone()
        {
            ShadowMonster monster = new ShadowMonster
            {
                name = this.name,
                displayName = this.displayName,
                texture = this.texture,
                element = this.element,
                costToBuy = this.costToBuy,
                level = this.level,
                experience = this.experience,
                attack = this.attack,
                defense = this.defense,
                speed = this.speed,
                health = this.health,
                currentHealth = this.health,
                Source = this.Source
            };

            foreach (string s in this.knownMoves.Keys)
            {
                monster.knownMoves.Add(s, this.knownMoves[s]);
            }

            return monster;
        }

        #endregion

        public void AssignPoint(string s, int p)
        {
            switch (s)
            {
                case "Attack":
                    attack += p;
                    break;
                case "Defense":
                    defense += p;
                    break;
                case "Speed":
                    speed += p;
                    break;
                case "Health":
                    health += p;

                    if (currentHealth > 0)
                    {
                        currentHealth += p;
                    }

                    break;
            }
        }

        public static ShadowMonster FromString(string description, ContentManager content)
        {
            ShadowMonster monster = new ShadowMonster();
            string[] parts = description.Split(',');

            monster.name = parts[0];
            monster.displayName = parts[1];
            monster.texture = content.Load<Texture2D>(@"ShadowMonsterImages\" + parts[1]);
            monster.element = (ShadowMonsterElement)Enum.Parse(typeof(ShadowMonsterElement), parts[2]);
            monster.costToBuy = int.Parse(parts[3]);
            monster.level = int.Parse(parts[4]);
            monster.attack = int.Parse(parts[5]);
            monster.defense = int.Parse(parts[6]);
            monster.speed = int.Parse(parts[7]);
            monster.health = int.Parse(parts[8]);
            monster.currentHealth = monster.health;
            monster.Source = new Rectangle(int.Parse(parts[9]), int.Parse(parts[10]), 64, 64);

            monster.knownMoves = new Dictionary<string, IMove>();

            for (int i = 11; i < parts.Length; i++)
            {
                string[] moveParts = parts[i].Split(':');

                if (moveParts[0] != "None")
                {
                    IMove move = MoveManager.GetMove(moveParts[0]);
                    move.UnlockedAt = int.Parse(moveParts[1]);

                    if (move.UnlockedAt <= monster.Level)
                    {
                        move.Unlock();
                    }

                    monster.knownMoves.Add(move.Name, move);
                }
            }
            return monster;
        }

        public bool Save(BinaryWriter writer)
        {
            StringBuilder b = new StringBuilder();

            b.Append(name);
            b.Append(",");
            b.Append(displayName);
            b.Append(",");
            b.Append(element);
            b.Append(",");
            b.Append(experience);
            b.Append(",");
            b.Append(costToBuy);
            b.Append(",");
            b.Append(level);
            b.Append(",");
            b.Append(attack);
            b.Append(",");
            b.Append(defense);
            b.Append(",");
            b.Append(speed);
            b.Append(",");
            b.Append(health);
            b.Append(",");
            b.Append(currentHealth);
            b.Append(",");
            b.Append(IsAsleep);
            b.Append(",");
            b.Append(IsConfused);
            b.Append(",");
            b.Append(IsParalyzed);
            b.Append(",");
            b.Append(IsPoisoned);
            b.Append(",");
            b.Append(Source.X);
            b.Append(",");
            b.Append(Source.Y);

            foreach (string s in knownMoves.Keys)
            {
                b.Append(",");
                b.Append(s);
                b.Append(":");
                b.Append(knownMoves[s].UnlockedAt);
            }

            writer.Write(b.ToString());

            return true;
        }

        public static ShadowMonster Load(ContentManager content, string monster)
        {
            ShadowMonster s = new ShadowMonster();

            string[] parts = monster.Split(',');
            s.name = parts[0];
            s.displayName = parts[1];
            s.texture = content.Load<Texture2D>(@"ShadowMonsterImages\" + parts[1]);
            s.element = (ShadowMonsterElement)Enum.Parse(typeof(ShadowMonsterElement), parts[2]);

            int.TryParse(parts[3], out int value);
            s.experience = value;

            int.TryParse(parts[4], out value);
            s.costToBuy = value;

            int.TryParse(parts[5], out value);
            s.level = value;

            int.TryParse(parts[6], out value);
            s.attack = value;

            int.TryParse(parts[7], out value);
            s.defense = value;

            int.TryParse(parts[8], out value);
            s.speed = value;

            int.TryParse(parts[9], out value);
            s.health = value;

            int.TryParse(parts[10], out value);
            s.currentHealth = value;

            bool.TryParse(parts[11], out bool value1);
            s.IsAsleep = value1;

            bool.TryParse(parts[12], out value1);
            s.IsConfused = value1;

            bool.TryParse(parts[13], out value1);
            s.IsParalyzed = value1;

            bool.TryParse(parts[14], out value1);
            s.IsPoisoned = value1;

            int.TryParse(parts[15], out int x);
            int.TryParse(parts[16], out int y);
            s.Source = new Rectangle(x, y, 64, 64);

            for (int i = 17; i < parts.Length; i++)
            {
                string[] moveParts = parts[i].Split(':');

                if (moveParts[0] != "None")
                {
                    IMove move = MoveManager.GetMove(moveParts[0]);
                    int.TryParse(moveParts[1], out value);
                    move.UnlockedAt = value;

                    if (move.UnlockedAt <= s.Level)
                    {
                        move.Unlock();
                    }

                    s.knownMoves.Add(move.Name, move);
                }
            }

            return s;
        }
    }
}
