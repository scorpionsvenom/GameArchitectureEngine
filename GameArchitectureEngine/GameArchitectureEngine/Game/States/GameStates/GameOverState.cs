using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    class GameOverState : State
    {
        public GameOverState()
        {
            Name = "GameOver";
        }

        public override void Enter(object owner)
        {
            ActionRPG game = owner as ActionRPG;

            if (game != null)
            {
                game.gameState = GameState.GameOverState;
                game.InitialiseGameOverState();
                game.LoadGameOverContent();
            }
        }

        public override void Exit(object owner)
        {
            ActionRPG game = owner as ActionRPG;

            if (game != null)
            {

            }
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            ActionRPG game = owner as ActionRPG;

            if (game == null) return;

            game.gameState = GameState.GameOverState;
        }
    }
}