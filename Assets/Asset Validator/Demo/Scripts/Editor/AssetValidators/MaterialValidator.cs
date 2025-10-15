//------------------------------------
//          Asset Validator
//     Copyright© 2025 OmniShade     
//------------------------------------

using UnityEngine;

/**
 * Demo Material validator.
 * Declaring this class overrides the parent MaterialValidator in the package.
 **/
namespace AssetValidatorDemo {

    public class MaterialValidator : AssetValidator.MaterialValidator {
        protected override bool Validate(Object obj) {
            bool isValid = base.Validate(obj);

            var mat = obj as Material;
            if (mat == null)
                return isValid;

            // Custom check that alpha value is set to 1 for opaque objects
            if (isValid && mat.renderQueue < 2450 && mat.color.a < 1) {
                LogError(mat, "is a solid material but has alpha set below 1.0");
                isValid = false;
            }

            return isValid;
        }
    }
}
