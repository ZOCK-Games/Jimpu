//------------------------------------
//          Asset Validator
//     Copyright© 2025 OmniShade     
//------------------------------------

using UnityEngine;

/**
 * Demo scriptable object using [Required] and [ResourcePath] attributes.
 **/
namespace AssetValidatorDemo {

    [CreateAssetMenu(menuName = "Scriptable Objects/CreatureData")]
    public class CreatureData : ScriptableObject {
        // This field must be assigned to a non-null value
        [Required] public Material SkinMat;

        // This field must be an asset in a Resources folder
        [ResourcePath] public string TerrainTex;

        // MinLife should not be greater than MaxLife, this will be validated
        // by CreatureDataValidator.
        [Range(1, 100)] public int MinLife = 1;
        [Range(1, 100)] public int MaxLife = 100;
    }
}
