using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class FleeState : State
    {
        public FleeState()
        {
            Name = "Flee";
        }

        public override void Enter(object owner)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy != null)
            {
                enemy.state = EnemyGameObject.EnemyState.Flee;
                enemy.Speed = EnemyGameObject.FleeSpeed;
            }
        }

        public override void Exit(object owner)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;
            if (enemy != null) enemy.Speed = EnemyGameObject.FleeSpeed;
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy == null) return;

            if (enemy.Player != null)
            {
                if (enemy.PlayerSeen)
                {
                    Vector2 newDirection = enemy.Position - enemy.Player.Position;
                    newDirection.Normalize();
                    enemy.Direction = newDirection;
                    enemy.Velocity = enemy.Direction * enemy.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }
    }
}
