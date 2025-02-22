using Core;
using Model;
using Mechanics;

namespace Gameplay
{
    public class PlayerEnteredVictoryZone : Event<PlayerEnteredVictoryZone>
    {
        public VictoryZone VictoryZone;
        private readonly PlatformerModel _model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            // анимация победы
            _model.Player.ControlEnabled = false;
            _model.MetaGameController.ToggleMainMenu();
            Simulation.Schedule<ActivateVictoryPanel>();
        }
    }
}