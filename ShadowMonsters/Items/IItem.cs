using Microsoft.Xna.Framework;
using ShadowMonsters.ShadowMonsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Items
{
    public enum ItemTarget { Self, Enemy }

    public interface IItem
    {
        string Name { get; }
        int Price { get; }
        bool Usable { get; }
        ItemTarget Target { get; }
        bool Apply(ShadowMonster monster);
    }
}
