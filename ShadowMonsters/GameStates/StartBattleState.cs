using Microsoft.Xna.Framework;
using ShadowMonsters.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMonsters.GameStates
{
    public interface IStartBattleState
    {
        void SetCombatants(Player player, Character character);
    }

    public class StartBattleState : BaseGameState, IStartBattleState
    {
        Player player;
        Character character;

        public StartBattleState(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (character.BattleMonster.CurrentHealth <= 0)
            {
                if (!character.NextMonster())
                {
                    manager.ChangeState(GameRef.GamePlayState);
                }
                else
                {
                    GameRef.BattleState.SetShadowMonsters(player.Selected, character.BattleMonster);
                    GameRef.ActionSelectionState.SetShadowMonsters(player.Selected, character.BattleMonster);
                    manager.PushState((BattleState)GameRef.BattleState);
                    manager.PushState((ActionSelectionState)GameRef.ActionSelectionState);
                }
            }
            else if (player.Selected.CurrentHealth <= 0)
            {
                if (!player.Alive())
                {
                    manager.ChangeState(GameRef.GamePlayState);
                }
                else
                {
                    manager.PushState((ShadowMonsterSelectionState)GameRef.ShadowMonsterSelectionState);
                }
            }
            else
            {
                GameRef.BattleState.SetShadowMonsters(player.Selected, character.BattleMonster);
                GameRef.ActionSelectionState.SetShadowMonsters(player.Selected, character.BattleMonster);
                manager.PushState((BattleState)GameRef.BattleState);
                manager.PushState((ActionSelectionState)GameRef.ActionSelectionState);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void SetCombatants(Player player, Character character)
        {
            this.player = player;
            this.character = character;

            if (!player.Alive())
            {
                manager.PopState();
                return;
            }

            if (!character.Alive())
            {
                manager.PopState();
                return;
            }

            GameRef.BattleState.SetShadowMonsters(player.Selected, character.BattleMonster);
            GameRef.ActionSelectionState.SetShadowMonsters(player.Selected, character.BattleMonster);
            manager.PushState((BattleState)GameRef.BattleState);
            manager.PushState((ActionSelectionState)GameRef.ActionSelectionState);
        }
    }
}
