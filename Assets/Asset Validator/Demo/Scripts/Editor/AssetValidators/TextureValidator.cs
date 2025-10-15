//------------------------------------
//          Asset Validator
//     Copyright© 2025 OmniShade     
//------------------------------------

using UnityEditor;
using UnityEngine;

/**
 * Demo texture validator. 
 * Declaring this class of the same name overrides the parent validator.
 **/
namespace AssetValidatorDemo {

    public class TextureValidator : AssetValidator.TextureValidator {
        public TextureValidator() {
            // Over-ride large texture validation dimensions
            CompressedMaxSize = 1024;
            UncompressedMaxSize = 512;
            MinDimensionChecked = 256;            
        }

        protected override bool Validate(Object obj) {
            bool isValid =  base.Validate(obj);

            string path = AssetDatabase.GetAssetPath(obj);
            var ti = AssetImporter.GetAtPath(path) as TextureImporter;

            // Custom check that MipMaps are enabled on non-sprite objects
            if (ti.textureType != TextureImporterType.Sprite && !ti.mipmapEnabled) {
                LogError(obj, "should have MipMaps enabled.");
                isValid = false;
            }

            return isValid;
        }
    }
}
