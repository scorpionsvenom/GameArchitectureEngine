using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class AttackState : State
    {
        private const double attackCooldownTime = 0.75;
        private double currentTime = 0.0;

        public override void Enter(object owner)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy != null)
            {
                enemy.Speed = 0.0f;
                currentTime = 0.0;
            }
        }

        public override void Exit(object owner)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy != null)
            {
                enemy.Speed = 0.0f;
                currentTime = 0.0;
            }
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy == null) return;

            if (currentTime >= attackCooldownTime)
            {
                enemy.Speed = 0.0f;
                currentTime = 0.0f;
                enemy.Attack();
            }
            else
            {
                enemy.Speed = 0.0f;
                currentTime += gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
