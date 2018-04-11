using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class IdleState : State
    {
        private const double directionChangeTime = 0.5;
        private double currentTime = 0.0;

        public IdleState()
        {
            Name = "Idle";
        }

        public override void Enter(object owner)
        {
            //Define enemy and behaviour initialisation
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy != null) enemy.Speed = 48;

            currentTime = 0.0;
        }

        public override void Exit(object owner)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy != null) enemy.Speed = 64;

            currentTime = 0.0f;
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy == null) return;

            if (currentTime >= directionChangeTime)
            {
                currentTime = 0.0f;
                enemy.SetRandomDirection();
            }
            else
            {
                currentTime += gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
