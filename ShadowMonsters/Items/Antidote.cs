using ShadowMonsters.ShadowMonsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Items
{
    public class Antidote : IItem
    {
        public string Name => "Antidote";

        public int Price => 1000;

        public bool Usable => true;


        public void Apply(ShadowMonster monster)
        {
            monster.IsPoisoned = false;
        }
    }
}
