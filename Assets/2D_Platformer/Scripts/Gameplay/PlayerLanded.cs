using Core;
using Mechanics;

namespace Gameplay
{
    public class PlayerLanded : Event<PlayerLanded>
    {
        public PlayerController player;
        public override void Execute()
        {
            // звуки опускания на земную твердь
        }
    }
}