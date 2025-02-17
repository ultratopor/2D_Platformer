using Model;
using Core;

namespace Gameplay
{
    public class EnablePlayerInput : Event<EnablePlayerInput>
    {
        private PlatformerModel _model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = _model.player;
            player.controlEnabled = true;
        }
    }
}