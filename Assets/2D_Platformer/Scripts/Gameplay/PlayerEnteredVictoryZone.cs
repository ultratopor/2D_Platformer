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
            model.Player.PlayerAnimator.SetTrigger("victory");
            model.Player.ControlEnabled = false;
        }
    }
}