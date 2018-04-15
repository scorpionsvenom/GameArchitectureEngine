using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class WalkState : State
    {
        public WalkState()
        {
            Name = "Walk";
        }

        public override void Enter(object owner)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player != null) player.Speed = player.MaxSpeed;
        }

        public override void Exit(object owner)
        {
            PlayerGameObject player = owner as PlayerGameObject;

            if (player != null) player.Speed = player.MaxSpeed;
        }

        public override void Execute(object owner, GameTime gameTime)
        {         
            PlayerGameObject player = owner as PlayerGameObject;
        }
    }
}
