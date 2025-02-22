using Core;
using Model;

namespace Gameplay
{
    public class SwitchMainMenu : Event<SwitchMainMenu>
    {
        private readonly PlatformerModel _model = Simulation.GetModel<PlatformerModel>();
        public override void Execute()
        {
            _model.MetaGameController.ToggleMainMenu();
        }
    }
}