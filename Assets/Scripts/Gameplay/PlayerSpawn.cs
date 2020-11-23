using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            model.DebugLog("player spawn");
            //var player = model.player;
            var outPlayer = model.ourPlayer;
            //player.collider2d.enabled = true;
            //player.controlEnabled = false;
            //if (player.audioSource && player.respawnAudio)
            //    player.audioSource.PlayOneShot(player.respawnAudio);
            //player.health.Increment();
            //player.Teleport(model.spawnPoint.transform.position);
            outPlayer.Teleport(model.spawnPoint.transform.position);
            //player.jumpState = PlayerController.JumpState.Grounded;
            //player.animator.SetBool("dead", false);
            model.virtualCamera.m_Follow = outPlayer.transform;
            model.virtualCamera.m_LookAt = outPlayer.transform;
            Simulation.Schedule<EnablePlayerInput>(2f);
        }
    }
}