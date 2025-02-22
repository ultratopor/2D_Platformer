using UnityEngine;
using Gameplay;
using Core;

namespace Mechanics
{
    public class VictoryZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p == null) return;
            var ev = Simulation.Schedule<PlayerEnteredVictoryZone>();
            ev.VictoryZone = this;
        }
    }
}