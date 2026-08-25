# Dialog System Z - Documentation
by - ZOCK Games - 

# Table of Contents
1. Introduction
2. Requirements
3. Installation
4. Creating Your First Dialogue
5. Playing Your Dialogue
6. Dialogue Editor
7. Node Types
8. Characters
9. Audio
10. Dialogue Elements
11. Triggers
12. Saving and Loading
13. Troubleshooting
14. Support

## 1. Introduction

Dialog System Z is a lightweight, visual node-based system to create, manage, and trigger interactive dialogues effortlessly. Perfect for branching narratives with audio support and custom character profiles.

## 2. Requirements

- Unity 2021.3+
- Unity UI
- TextMeshPro
- Optional InputSystem

# 3. Installation

1. Import the Dialog System package into your Unity project.
2. Open the Dialog System folder in the Project window.
3. Add the required Dialogue Window to your scene:
4. Dialog → Dialog Window
5. Make sure the Dialogue Window exists in the scene before starting a dialogue.

# 4. Creating Your First Dialogue

1. Open the Dialogue Editor

Window → Dialog Editor
The Dialogue Editor is used to create and edit dialogue graphs / Create Characters.

2. Create a Dialogue

Create a new dialogue node with right click then select Start (without a start node the dialog doesn't work)
Give the node a description.

Then create another node like a Normal Dialog Node and connect the output port from the Start node with the
input port node of the other node

To Finish add a end node 3. Save the Dialog

Click on the Save button on the top right and select where to save the Dialog you need to save it in Assets/Resources otherwise it wont work at runtime.

# 5. Playing your Dialogue

1. After you successfully created your Dialog File you now want to trigger it for that if you don't have a Dialogue Window in your hierarchy create one (Dialogue -> dialogue window )

2. Now that you have your dialogue window create an Dialogue Trigger (Dialogue -> Dialogue trigger)

3. After Selecting the Dialogue trigger Select your dialogue file and dialogue element in the inspector

4. next select a trigger option and if needed select the new option

# 6. Dialogue Editor

The Dialogue Editor provides a visual graph interface for creating dialogue flows.

Creating Nodes

- Right-click inside the graph to open the node menu.

Moving Nodes

- Click and drag a node to reposition it.

Connecting Nodes

- Drag from an output port to an input port to create a connection.

Deleting Nodes

- Select a node and right click it then delete it using the available delete option.

Duplicating Nodes

- Select a node and right click it then use the duplicate option to create a copy.

Saving Nodes

- Look at the top right and left click the Save Button and the select your Folder (needs to be in Assets/Resources)

Loading Nodes

- Look at the top left and left click the Load Button after that a window opens where you can Load/Delete/Duplicate
  the Dialogue

Create Characters

- Look at the top left and left click the Character Button now a window opens where you can fill out the infos and the create
  a new Character or you can click on "All Characters" to view all characters and also delete characters

# 7. Node Types

Start Node

- The Start Node defines the beginning of a dialogue.
- A dialogue needs to have one Start Node.

Dialog Node

- The Dialog Node displays dialogue text and character information.
- Use it to create normal lines of dialogue.

Multiple Choice Node

- The Multiple Choice Node allows the player to select between multiple dialogue options.
- Each choice can lead to a different node.

Audio Node

- The Audio Node plays an audio clip during the dialogue.
- Audio files must be stored inside a Resources folder.

Action Node

- The Action Node can be used to perform configured actions during a dialogue.
- Characters can also be configured for use with dialogue actions.

Wait Node

- The Wait Node pauses the dialogue for the configured amount of time before continuing.

End Node

- The End Node marks the end of a dialogue path and when played will Clear the dialogue UI window.
- Every possible dialogue path should eventually reach an End Node.

# 8. Characters

- Characters can be created and configured through the Dialog System.
- Characters can be used to display character information and images during dialogue.
- Create your characters before assigning them to dialogue actions or dialogue elements.

# 9. Audio

Audio clips used by the Dialog System must currently be stored inside a Unity Resources folder.
For example: "Assets/Resources/Audio/MyDialogueAudio.wav"

# 10. Dialogue Elements

The visual appearance of the dialogue can be configured through the Dialogue Elements.
The default elements are located in:
DialogSystem/Elements
You can modify these elements to customize the appearance of the dialogue UI.
For example, you can customize:

- Character image
- Character name
- Dialogue text
- Choice buttons
- Colors
- Layout

# 11. Triggers

After creating a dialogue, create a Trigger to Play the file in hierarchy (Dialogue -> Dialogue Trigger)

In the corresponding trigger component:

Select the dialogue file.
Choose the desired trigger.
Enter Play Mode.
Activate the trigger to start the dialogue.

# 12. Saving and Loading

Save your dialogue after making changes in the Dialogue Editor.

Important: Dialogue files currently need to be located inside a Resources folder for runtime loading.

Dialogue files stored outside a Resources folder cannot currently be loaded by the runtime system.

# 13. Troubleshooting
My dialogue does not load
- Make sure the dialogue file is located inside a Resources folder.

My audio does not play
- Make sure the audio clip is also located inside a Resources folder.

The dialogue window does not appear
- Make sure a Dialogue Window has been added to the scene.
- Dialog → Dialog Window

A dialogue path does not continue
- Check that the nodes are connected correctly and that the output port is connected to the expected input port.

A dialogue never ends
- Make sure every possible dialogue path eventually reaches an End Node.


## 14. Support

- https://zock-games.de/
- Contact: zock868@gmail.com
