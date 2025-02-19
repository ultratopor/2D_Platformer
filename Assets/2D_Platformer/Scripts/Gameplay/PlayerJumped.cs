using Core;
using Mechanics;

namespace Gameplay
{
    public class PlayerJumped : Event<PlayerJumped>
    {
        public PlayerController Player;

        public override void Execute()
        {
            if (Player.AudioSource && Player.JumpAudio)
                Player.AudioSource.PlayOneShot(Player.JumpAudio);
        }
    }
}