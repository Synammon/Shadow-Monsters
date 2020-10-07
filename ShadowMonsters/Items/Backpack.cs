using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.Items
{
    public class Item
    {
        public string Name;
        public int Count;

        public void Save(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Count);
        }
    }

    public class Backpack
    {
        private readonly List<Item> items = new List<Item>();

        public List<Item> Items { get => items; }


        public Backpack()
        {
        }

        public void AddItem(IItem item, int count)
        {
            if (items.Any(x => x.Name == item.Name))
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].Name == item.Name)
                    {
                        items[i].Count = items[i].Count + count;
                    }
                }
            }
            else
            {
                Item i = new Item()
                {
                    Name = item.Name,
                    Count = count
                };

                items.Add(i);
            }
        }

        public void AddItem(string item, int count)
        {
            if (items.Any(x => x.Name == item))
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].Name == item)
                    {
                        items[i].Count = items[i].Count + count;
                    }
                }
            }
            else
            {
                Item i = new Item()
                {
                    Name = item,
                    Count = count
                };

                items.Add(i);
            }
        }

        public IItem GetItem(string item)
        {
            if (!items.Any(x => x.Name == item))
            {
                return null;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name == item)
                {
                    items[i].Count--;

                    if (items[i].Count < 1)
                    {
                        items.RemoveAt(i);
                    }

                    break;
                }
            }

            switch (item)
            {
                case "Potion":
                    return new Potion();
                case "Antidote":
                    return new Antidote();
                case "Binding Scroll":
                    return new BindingScroll();
                default:
                    return null;
            }
        }

        public IItem PeekItem(string item)
        {
            if (!items.Any(x => x.Name == item))
            {
                return null;
            }

            switch (item)
            {
                case "Potion":
                    return new Potion();
                case "Antidote":
                    return new Antidote();
                case "Binding Scroll":
                    return new BindingScroll();
                default:
                    return null;
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Items.Count);

            foreach (Item i in Items)
                i.Save(writer);
        }

        public static Backpack Load(BinaryReader reader)
        {
            reader.ReadInt32();

            Backpack b = new Backpack();
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                b.AddItem(reader.ReadString(), reader.ReadInt32());
            }

            return b;
        }
    }
}
