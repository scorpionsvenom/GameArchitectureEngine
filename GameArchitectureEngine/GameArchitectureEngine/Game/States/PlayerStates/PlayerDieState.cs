using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class PlayerDieState : State
    {
        private const double dyingTime = 1.0;
        private double currentTime = 0.0;

        public PlayerDieState()
        {
            Name = "PlayerDie"; 
        }

        public override void Enter(object owner)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player != null)
            {
                player.state = PlayerGameObject.PlayerState.Die;
                player.Speed = 0.0f;
                currentTime = 0.0;
            }
        }

        public override void Exit(object owner)
        {
            PlayerGameObject player = owner as PlayerGameObject;
            currentTime = 0.0;
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player == null) return;

            if (currentTime >= dyingTime)
            {
                player.Die();
                currentTime = 0.0;
            }
            else
            {
                currentTime += gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
