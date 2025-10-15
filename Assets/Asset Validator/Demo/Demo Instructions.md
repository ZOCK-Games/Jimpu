-----------------
DEMO INSTRUCTIONS
-----------------
This is a demo project containing various materials, textures, models, prefabs, audio clips, etc., designed to simulate a real project. There are multiple *intentional* problems with the configuration of the assets and scenes such as textures that are too large, missing references, long music clips that don't use Streaming load type, models marked Read/Write consuming extra memory, or scriptable objects with invalid configurations.

Run the Asset Validator to detect all the issues.
  Menu->Window->Asset Validator
  or use hotkey Ctrl/Cmd-Shift + V

Alternatively, you can also run it as a unit test from:
  Menu->General->Test Runner->AssetValidatorTests
  This is a text interface intended for use with a Continuous Integration build process.

There are several sample validators in the demo project, designed to show how you can customize your own validators to suit your project.

After you have finished reviewing the Demo project, I recommend removing it so as to avoid unnecessary errors from being displayed by the Asset Validator in the project.
