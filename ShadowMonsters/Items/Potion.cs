using Microsoft.Xna.Framework;
using ShadowMonsters.ShadowMonsters;

namespace ShadowMonsters.Items
{
    public class Potion : IItem
    {
        public string Name { get { return "Potion"; } }
        public int Price { get { return 200; } }
        public bool Usable => true;

        public void Apply(ShadowMonster monster)
        {
            monster.Heal(50);
        }
    }
}
