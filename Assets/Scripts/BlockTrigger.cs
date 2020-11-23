using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// If player enters, block falls
    /// </summary>
    /// 
    public class BlockTrigger : MonoBehaviour
    {
        public GameObject block;
        private BlockScript blockScript;

        void OnTriggerEnter2D(Collider2D collider)
        {
            blockScript = block.GetComponent<BlockScript>();
            blockScript.startBlock();

        }
    }
}