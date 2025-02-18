using Core;
using Model;

namespace Gameplay
{
    public class PlayerDeath : Event<PlayerDeath>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            if (player.Health.IsAlive)
            {
                player.Health.Die();
                model.virtualCamera.Follow = null;
                model.virtualCamera.LookAt = null;
                // player.collider.enabled = false;
                player.ControlEnabled = false;

                if (player.AudioSource && player.OuchAudio)
                    player.AudioSource.PlayOneShot(player.OuchAudio);
                player.Animator.SetTrigger("hurt");
                player.Animator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(2);
            }
        }
    }
}