using UnityEngine;
using Core;
using Gameplay;

namespace Mechanics
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                var ev = Simulation.Schedule<PlayerEnteredDeathZone>();
                ev.Deathzone = this;
            }
        }
    }
}