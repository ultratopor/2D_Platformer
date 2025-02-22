using Core;
using Mechanics;

namespace Gameplay
{
    public class HealthIsZero : Event<HealthIsZero>
    {
        public Health Health;

        public override void Execute()
        {
            Simulation.Schedule<PlayerDeath>();
        }
    }
}