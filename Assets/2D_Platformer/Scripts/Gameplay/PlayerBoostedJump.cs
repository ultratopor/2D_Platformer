using Core;
using Mechanics;
using Model;

namespace Gameplay
{
    public class PlayerBoostedJump : Event<PlayerBoostedJump>
    {
        public MushroomBooster Booster;
        private readonly PlatformerModel _model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var willBoost = _model.Player.Bounds.center.y >= Booster.Bounds.max.y;
            if (willBoost)
            {
                _model.Player.Bounce(Booster.Boost);
                Booster.SetBoostAnimation();
            }
        }
    }
}