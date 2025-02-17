using Core;

namespace Gameplay
{
    public class HealthIsZero : Event<HealthIsZero>
    {
        public Health health;

        public override void Execute()
        {
            Schedule<PlayerDeath>();
        }
    }
}