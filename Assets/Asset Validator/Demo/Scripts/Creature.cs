//------------------------------------
//          Asset Validator
//     Copyright© 2025 OmniShade     
//------------------------------------

using System;
using UnityEngine;

/**
 * Demo component using [Required] and [ResourcePath] attributes, 
 * including within a nested serialized class.
 **/
namespace AssetValidatorDemo {

    public class Creature : MonoBehaviour {
        // This field must be assigned to a non-null value
        [Required] public GameObject[] Weapons;

        // This field must be an asset in a Resources folder
        [ResourcePath] public string AttackSFX;

        // Attributes work with serialized classes as well.
        [Serializable]
        public class Terrain {
            [Required] public GameObject TerrainPrefab;
            [ResourcePath] public string TerrainTex;
        }
        public Terrain Ground;

        // Life should never be negative, this will be validated by CreatureValidator.
        public int Life;
    }
}
