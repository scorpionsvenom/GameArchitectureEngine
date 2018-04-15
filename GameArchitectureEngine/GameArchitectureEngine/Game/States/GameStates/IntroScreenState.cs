using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class IntroScreenState : State
    {
        public IntroScreenState()
        {
            Name = "Intro";
        }

        public override void Enter(object owner)
        {
            ActionRPG game = owner as ActionRPG;

            if (game != null)
            {
                game.gameState = GameState.IntroState;
                game.InitialiseIntroState();
                game.LoadIntroScreenContent();
                game.SpacePressed = false;
            }
        }

        public override void Exit(object owner)
        {
            ActionRPG game = owner as ActionRPG;

            if (game != null)
            {
                game.UnloadIntroContent();
            }
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            ActionRPG game = owner as ActionRPG;

            if (game == null) return;

            game.gameState = GameState.IntroState;
            //game.Player.IsAlive = true;
        }
    }
}
