using Core;
using Model;

namespace Gameplay
{
    public class PlayerDeath : Event<PlayerDeath>
    {
        private PlatformerModel _model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = _model.Player;
            if (player.PlayerHealth.IsAlive)
            {
                player.PlayerHealth.Die();
                _model.VirtualCamera.Follow = null;
                _model.VirtualCamera.LookAt = null;
                // player.collider.enabled = false;
                player.ControlEnabled = false;

                if (player.AudioSource && player.OuchAudio)
                    player.AudioSource.PlayOneShot(player.OuchAudio);
                player.PlayerAnimator.SetTrigger("hurt");
                player.PlayerAnimator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(2);
            }
        }
    }
}