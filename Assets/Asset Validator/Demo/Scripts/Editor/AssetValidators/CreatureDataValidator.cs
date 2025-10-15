//------------------------------------
//          Asset Validator
//     Copyright© 2025 OmniShade     
//------------------------------------

using UnityEngine;
using AssetValidator;

/**
 * Demo of an asset validator for CreatureData scriptable objects.
 **/
namespace AssetValidatorDemo {

    public class CreatureDataValidator : AssetValidatorBase {
        public CreatureDataValidator() {
            filter = "t:creaturedata";
        }

        protected override bool Validate(Object obj) {
            var cd = obj as CreatureData;
            if (cd.MinLife >= cd.MaxLife) {
                LogError(cd, "cannot have MinLife " + cd.MinLife + " greater than MaxLife " + cd.MaxLife + ".");
                return false;
            }

            return true;
        }
    }
}
