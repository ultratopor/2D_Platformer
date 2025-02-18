using Core;
using Mechanics;

namespace Gameplay
{
    public class HealthIsZero : Event<HealthIsZero>
    {
        public Health health;

        public override void Execute()
        {
            Simulation.Schedule<PlayerDeath>();
        }
    }
}