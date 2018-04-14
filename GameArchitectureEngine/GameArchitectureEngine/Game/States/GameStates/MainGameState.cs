using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    class MainGameState : State
    {
        private const double SceneChangeTime = 1.25;
        private double currentTime = 0.0;

        public MainGameState()
        {
            Name = "MainGame";
        }

        public override void Enter(object owner)
        {
            ActionRPG game = owner as ActionRPG;

            if (game != null)
            {
                //game.ResetScene();
                game.gameState = GameState.MainGameState;
                game.InitialiseMainGameState();
                game.LoadMainGameContent();

                //Load main game content. load correct map
                
                
                               
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
            ActionRPG game = owner as ActionRPG;

            if (game == null) return;

            
        }
    }
}