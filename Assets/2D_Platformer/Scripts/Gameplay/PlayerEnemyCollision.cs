using Core;
using Model;
using Mechanics;
using UnityEngine;

namespace Gameplay
{
    public class PlayerEnemyCollision : Event<PlayerEnemyCollision>
    {
        public EnemyController Enemy;
        public PlayerController Player;

        //private PlatformerModel _model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            // механика уничтожения врагов как в Марио
            var willHurtEnemy = Player.Bounds.center.y >= Enemy.Bounds.max.y;

            if (willHurtEnemy)
            {
                var enemyHealth = Enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.Decrement();
                    Simulation.Schedule<EnemyTakeHit>().Enemy = Enemy;
                    
                    if (!enemyHealth.IsAlive)
                    {
                        Simulation.Schedule<EnemyDeath>().Enemy = Enemy;
                        Player.Bounce(4);
                    }
                    else
                    {
                        Player.Bounce(7);
                    }
                }
                else
                {
                    Simulation.Schedule<EnemyDeath>().Enemy = Enemy;
                    Player.Bounce(2);
                }
            }
            else
            {
                Simulation.Schedule<PlayerDeath>();
            }
        }
    }
}