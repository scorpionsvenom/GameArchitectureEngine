using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameArchitectureEngine
{    
    public delegate void CollisionHandler(object sender, CollisionEventArgs e);
    public delegate void CollisionWithSpecificObjectHandler(object sender, object other, CollisionEventArgs e);
}
