using Core;
using Model;
using Mechanics;

namespace Gameplay
{
    public class PlayerSpawn : Event<PlayerSpawn>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            player.Collider2d.enabled = true;
            player.ControlEnabled = false;
            if (player.AudioSource && player.RespawnAudio)
                player.AudioSource.PlayOneShot(player.RespawnAudio);
            player.Health.Increment();
            player.Teleport(model.spawnPoint.transform.position);
            player.PlayerJumpState = PlayerController.JumpState.Grounded;
            player.Animator.SetBool("dead", false);
            model.virtualCamera.Follow = player.transform;
            model.virtualCamera.LookAt = player.transform;
            Simulation.Schedule<EnablePlayerInput>(2f);
        }
    }
}