using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public delegate void GameAction(eButtonState buttonState, Vector2 amount);
    public delegate void CollisionHandler(object sender, CollisionEventArgs e);
    public delegate void CollisionWithSpecificObjectHandler(object sender, object other, CollisionEventArgs e);
}
