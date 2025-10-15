//------------------------------------
//          Asset Validator
//     Copyright© 2025 OmniShade     
//------------------------------------

using UnityEngine;

/**
 * Demo AudioClip validator.
 * Declaring this class overrides the parent AudioClipValidator in the package.
 **/
namespace AssetValidatorDemo {

    public class AudioClipValidator : AssetValidator.AudioClipValidator {
        const float StreamingThresholdSeconds = 30.0f;

        protected override bool Validate(Object obj) {
            bool isValid = base.Validate(obj);

            // Custom check that long clips use load type Streaming
            var audio = obj as AudioClip;
            if (audio.length > StreamingThresholdSeconds && 
                audio.loadType != AudioClipLoadType.Streaming) {
                LogError(audio, "of length " + (int)audio.length + 
                    " secs exceeds threshold for streaming (" + StreamingThresholdSeconds + " secs) but does not use Streaming load type.");
                isValid = false;
            }

            return isValid;
        }
    }
}
