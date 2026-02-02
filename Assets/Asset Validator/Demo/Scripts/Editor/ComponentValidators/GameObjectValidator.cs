//------------------------------------
//          Asset Validator
//     Copyright© 2025 OmniShade     
//------------------------------------

using UnityEngine;
using AssetValidator;

/**
 * Demo of an overridden GameObjectValidator to exclude certain types from duplicate components heck.
 **/
namespace AssetValidatorDemo {
    
    [ComponentValidator(typeof(GameObject))]
    public class GameObjectValidator : AssetValidator.GameObjectValidator {

        public GameObjectValidator() {
            // Override scripts that are excluded from the duplicate components check 
            ExcludedComponentsFromDuplicateCheck = new() {
                typeof(Accessory)
            };
        }
    }
}
