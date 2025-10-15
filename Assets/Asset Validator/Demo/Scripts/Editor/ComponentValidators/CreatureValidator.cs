//------------------------------------
//          Asset Validator
//     Copyright© 2025 OmniShade     
//------------------------------------

using UnityEngine;
using AssetValidator;

/**
 * Demo of component validator for Creature component.
 **/
namespace AssetValidatorDemo {
    
    [ComponentValidator(typeof(Creature))]
    public class CreatureValidator : ComponentValidatorBase {

        public override bool Validate(Object obj) {
            // Custom check that life is non-negative
            var creature = obj as Creature;
            if (creature.Life < 0) {
                LogError(creature, "has negative value for Life " + creature.Life + ".");
                return false;
            }

            return true;
        }
    }
}
