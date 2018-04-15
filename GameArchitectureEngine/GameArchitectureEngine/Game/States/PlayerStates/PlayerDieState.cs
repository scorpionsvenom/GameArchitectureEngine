using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class PlayerDieState : State
    {
        public PlayerDieState()
        {
            Name = "PlayerDie"; 
        }

        public override void Enter(object owner)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player != null)
            {
                player.Speed = 0.0f;
            }
        }

        public override void Exit(object owner)
        {
            PlayerGameObject player = owner as PlayerGameObject;
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player == null) return;

            player.Die();
        }
    }
}
