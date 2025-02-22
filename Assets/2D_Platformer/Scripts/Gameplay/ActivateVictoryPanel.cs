using Core;
using Model;

namespace Gameplay
{
    public class ActivateVictoryPanel : Event<ActivateVictoryPanel>
    {
        private readonly PlatformerModel _model = Simulation.GetModel<PlatformerModel>();
        public override void Execute()
        {
            _model.UIController.SetActivePanel(3);
        }
    }
}