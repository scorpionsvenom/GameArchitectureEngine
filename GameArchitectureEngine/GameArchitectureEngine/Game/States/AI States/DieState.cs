using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class DieState : State
    {
        public DieState()
        {
            Name = "Die"; 
        }

        public override void Enter(object owner)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy != null)
            {
                enemy.state = EnemyGameObject.EnemyState.Die;
                enemy.Speed = 0.0f;
            }
        }

        public override void Exit(object owner)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            EnemyGameObject enemy = owner as EnemyGameObject;

            if (enemy == null) return;
            
            enemy.Die();

            enemy.Speed = 0.0f;
        }
    }
}
