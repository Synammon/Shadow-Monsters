using ShadowMonsters.ShadowMonsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Items
{
    public class BindingScroll : IItem
    {
        public string Name => "Binding Scroll";

        public int Price => 200;

        public bool Usable => true;

        public ItemTarget Target => ItemTarget.Enemy;

        public bool Apply(ShadowMonster monster)
        {
            if (monster.CurrentHealth < monster.BaseHealth * 0.25f && ShadowMonster.Random.Next(0, 101) < 95)
            {
                if (monster.Level < 20)
                {
                    Game1.Player.AddShadowMonster(monster);
                    return true;
                }
                else if (monster.Level < 30)
                {
                    if (ShadowMonster.Random.Next(0, 101) < 50)
                    {
                        Game1.Player.AddShadowMonster(monster);
                        return true;
                    }
                }
                else if (monster.Level < 40)
                {
                    if (ShadowMonster.Random.Next(0, 101) < 25)
                    {
                        Game1.Player.AddShadowMonster(monster);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
