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
    public class ShadowMonsterManager
    {
        #region Field Region

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
            using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (TextReader reader = new StreamReader(stream))
                    {
                        try
                        {
                            string lineIn = "";

                            do
                            {
                                lineIn = reader.ReadLine();
                                if (lineIn != null)
                                {
                                    ShadowMonster monster = ShadowMonster.FromString(lineIn, content);
                                    if (!monsterList.ContainsKey(monster.Name.ToLowerInvariant()))
                                        monsterList.Add(monster.Name.ToLowerInvariant(), monster);
                                }
                            } while (lineIn != null);
                        }
                        catch (Exception exc)
                        {
                            exc.GetType();
                        }
                        finally
                        {
                            if (reader != null)
                                reader.Close();
                        }
                    }
                }
                catch (Exception exc)
                {
                    exc.GetType();
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
        }

        #endregion
    }
}
