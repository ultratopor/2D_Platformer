using Core;
using Model;
using Mechanics;

namespace Gameplay
{
    public class PlayerEnteredDeathZone : Event<PlayerEnteredDeathZone>
    {
        public DeathZone Deathzone;

        //PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            Simulation.Schedule<PlayerDeath>(0);
        }
    }
}