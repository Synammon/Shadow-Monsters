using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.ShadowMonsters
{
    public class ShadowMonsterManager
    {
        #region Field Region

        private static readonly byte[] IV = new byte[]
        {
            067, 197, 032, 010, 211, 090, 192, 076,
            054, 154, 111, 023, 243, 071, 132, 090
        };

        private static readonly byte[] Key = new byte[]
        {
            067, 090, 197, 043, 049, 029, 178, 211,
            127, 255, 097, 233, 162, 067, 111, 022,
        };

        private static readonly Dictionary<string, ShadowMonster> monsterList = new Dictionary<string, ShadowMonster>();

        #endregion

        #region Property Region

        public static Dictionary<string, ShadowMonster> ShadowMonsterList
        {
            get { return monsterList; }
        }

        #endregion

        #region Constructor Region

        #endregion

        #region Method Region

        public static void AddShadowMonster(string name, ShadowMonster monster)
        {
            if (!monsterList.ContainsKey(name))
            {
                monsterList.Add(name, monster);
            }
        }

        public static ShadowMonster GetShadowMonster(string name)
        {
            return monsterList.ContainsKey(name) ? (ShadowMonster)monsterList[name].Clone() : null;
        }

        public static void FromFile(string fileName, ContentManager content)
        {
            using (Aes aes = Aes.Create())
            {
                aes.IV = IV;
                aes.Key = Key;

                try
                {
                    ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                    FileStream stream = new FileStream(
                        fileName,
                        FileMode.Open,
                        FileAccess.Read);
                    using (CryptoStream cryptoStream = new CryptoStream(
                        stream,
                        decryptor,
                        CryptoStreamMode.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(cryptoStream))
                        {
                            int count = reader.ReadInt32();

                            for (int i = 0; i < count; i++)
                            {
                                string data = reader.ReadString();
                                reader.ReadInt32();
                                ShadowMonster monster = ShadowMonster.FromString(data, content);
                                ShadowMonsterManager.AddShadowMonster(monster.Name, monster);
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        #endregion
    }
}
