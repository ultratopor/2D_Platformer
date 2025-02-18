using Core;
using Mechanics;

namespace Gameplay
{
    public class PlayerJumped : Event<PlayerJumped>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (player.AudioSource && player.JumpAudio)
                player.AudioSource.PlayOneShot(player.JumpAudio);
        }
    }
}