using Core;
using Model;
using Mechanics;

namespace Gameplay
{
    public class PlayerEnteredVictoryZone : Event<PlayerEnteredVictoryZone>
    {
        public VictoryZone victoryZone;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            model.player.Animator.SetTrigger("victory");
            model.player.ControlEnabled = false;
        }
    }
}