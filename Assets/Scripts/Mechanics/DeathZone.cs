using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// DeathZone components mark a collider which will schedule a
    /// PlayerEnteredDeathZone event when the player enters the trigger.
    /// </summary>
    public class DeathZone : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log("Death Zone Entered");
            var p = collider.gameObject.GetComponent<CarController>();
            Debug.Log(p.ToString());
            if (p != null)
            {
                Debug.Log("p is not null");
                var ev = Schedule<PlayerEnteredDeathZone>();
                ev.deathzone = this;
            }
        }
    }
}