using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class ChaseState : State
    {
        public ChaseState()
        {
            Name = "Chase";
        }

        public override void Enter(object owner)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy != null)
            {
                enemy.state = EnemyGameObject.EnemyState.Chase;
                enemy.Speed = EnemyGameObject.ChaseSpeed;
            }
        }

        public override void Exit(object owner)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy != null)
            {
                enemy.Speed = EnemyGameObject.ChaseSpeed;
            }
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy == null) return;

            enemy.MoveToward(enemy.Target, gameTime);        
        }

        
    }
}
