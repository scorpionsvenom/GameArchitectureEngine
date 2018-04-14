using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameArchitectureEngine
{
    public delegate void DamagePlayerHandler(object sender, CollisionEventArgs e);
    public delegate void HealPlayerHandler(object sender, CollisionEventArgs e);
    public delegate void CollisionHandler(object sender, CollisionEventArgs e);
    public delegate void DamageEnemyHandler(PlayerGameObject player, EnemyGameObject victim, CollisionEventArgs e);
    public delegate void CollisionWithPotionHandler(object sender, HealthPotionGameObject potion, CollisionEventArgs e);
}
