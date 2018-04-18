using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class PlayerChaseState : State
    {
        public PlayerChaseState()
        {
            Name = "PlayerChase";
        }

        public override void Enter(object owner)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player != null)
            {
                player.state = PlayerGameObject.PlayerState.Chase;
            }
        }

        public override void Exit(object owner)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player != null)
            {
            }
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player != null)
            {
                player.MoveTowardEntity(player.target, gameTime);
            }
        }

        
    }
}
