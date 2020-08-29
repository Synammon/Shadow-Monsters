using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.ShadowMonsters
{
    public static class MoveManager
    {
        #region Field Region

        private static readonly Dictionary<string, IMove> allMoves = new Dictionary<string, IMove>();
        private static readonly Random random = new Random();

        #endregion

        #region Property Region

        public static Random Random => random;

        #endregion

        #region Constructor Region
        #endregion

        #region Method Region

        public static void FillMoves()
        {
            allMoves.Add("Tackle", new Tackle());
            allMoves.Add("Block", new Block());
        }

        public static IMove GetMove(string name)
        {
            if (allMoves.ContainsKey(name))
                return (IMove)allMoves[name].Clone();

            return null;
        }

        public static void AddMove(IMove move)
        {
            if (!allMoves.ContainsKey(move.Name))
                allMoves.Add(move.Name, move);
        }

        #endregion
    }
}
