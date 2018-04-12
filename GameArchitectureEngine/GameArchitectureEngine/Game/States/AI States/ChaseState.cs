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
                enemy.Speed = 64;
            }
        }

        public override void Exit(object owner)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy != null)
            {
                enemy.Speed = 64;
            }
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy == null) return;

            enemy.MoveToward(enemy.Player, gameTime);        
        }

        
    }
}
