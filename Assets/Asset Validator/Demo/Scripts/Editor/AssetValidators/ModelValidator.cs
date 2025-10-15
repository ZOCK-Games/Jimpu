//------------------------------------
//          Asset Validator
//     Copyright© 2025 OmniShade     
//------------------------------------

using UnityEditor;
using UnityEngine;

/**
 * Demo Model validator.
 * Declaring this class overrides the parent ModelValidator in the package.
 **/
namespace AssetValidatorDemo {

    public class ModelValidator : AssetValidator.ModelValidator {
        protected override bool Validate(Object obj) {
            bool isValid = base.Validate(obj);

            string path = AssetDatabase.GetAssetPath(obj);
            var mi = AssetImporter.GetAtPath(path) as ModelImporter;

            // Custom check that mesh compression is enabled
            if (mi.meshCompression == ModelImporterMeshCompression.Off) {
                LogError(obj, "must have MeshCompression set to On.");
                isValid = false;
            }

            return isValid;
        }
    }
}
