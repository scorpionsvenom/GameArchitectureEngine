using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    class MainGameState : State
    {        
        public MainGameState()
        {
            Name = "MainGame";
        }

        public override void Enter(object owner)
        {
            ActionRPG game = owner as ActionRPG;

            if (game != null)
            {
                game.gameState = GameState.MainGameState;
                game.InitialiseMainGameState();
                game.LoadMainGameContent(true);
            }
        }

        public override void Exit(object owner)
        {
            ActionRPG game = owner as ActionRPG;

            if (game != null)
            {
                game.UnloadMainGameContent();
            }
        }

        public override void Execute(object owner, GameTime gameTime)
        {          
        }
    }
}