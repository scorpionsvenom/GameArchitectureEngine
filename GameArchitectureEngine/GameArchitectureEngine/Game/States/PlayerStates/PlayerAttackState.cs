using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class PlayerAttackState : State
    {
        private const double attackCooldownTime = 0.75;
        private double currentTime = 0.0;

        public PlayerAttackState()
        {
            Name = "PlayerAttack";
        }

        public override void Enter(object owner)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player != null)
            {
                player.Speed = 0.0f;
                currentTime = 0.0;
            }
        }

        public override void Exit(object owner)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player != null)
            {
                player.Speed = 0.0f;
                currentTime = 0.0;
            }
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player == null) return;            

            if (currentTime >= attackCooldownTime)
            {
                player.Speed = 0.0f;
                currentTime = 0.0f;
                player.Attack();
            }
            else
            {
                player.Speed = 0.0f;
                currentTime += gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
