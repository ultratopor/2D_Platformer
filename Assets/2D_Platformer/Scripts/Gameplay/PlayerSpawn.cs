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
            var player = model.Player;
            player.Collider2d.enabled = true;
            player.ControlEnabled = false;
            if (player.AudioSource && player.RespawnAudio)
                player.AudioSource.PlayOneShot(player.RespawnAudio);
            player.PlayerHealth.Increment();
            player.Teleport(model.SpawnPoint.transform.position);
            player.PlayerJumpState = PlayerController.JumpState.Grounded;
            player.PlayerAnimator.SetBool("dead", false);
            model.VirtualCamera.Follow = player.transform;
            model.VirtualCamera.LookAt = player.transform;
            Simulation.Schedule<EnablePlayerInput>(2f);
        }
    }
}