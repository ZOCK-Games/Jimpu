//------------------------------------
//          Asset Validator
//     Copyright© 2025 OmniShade     
//------------------------------------

using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

/**
 * Demo SpriteAtlas validator.
 * Declaring this class overrides the parent SpriteAtlasValidator in the package.
 **/
namespace AssetValidatorDemo {
    
    public class SpriteAtlasValidator : AssetValidator.SpriteAtlasValidator {
        protected override bool Validate(Object obj) {
            bool isValid = base.Validate(obj);

            // Custom check that Allow Rotation is disabled
            var atlas = obj as SpriteAtlas;
            var packingSettings = atlas.GetPackingSettings();
            if (packingSettings.enableRotation) {
                LogError(atlas, "must have AllowRotation disabled.");
                isValid = false;
            }

            return isValid;
        }
    }
}
