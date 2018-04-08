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
            throw new NotImplementedException();
        }

        public override void Exit(object owner)
        {
            throw new NotImplementedException();
        }

        public override void Execute(object owner, GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
