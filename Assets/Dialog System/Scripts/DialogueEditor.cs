#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogSystem
{
    [InitializeOnLoad]
    public class DialogueEditor : EditorWindow
    {
        public static DialogueEditor instance { get; private set; }
        public DialogueGraphView dialogueGraphView;

        static DialogueEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode || state == PlayModeStateChange.EnteredPlayMode)
            {
                instance = null;
            }
        }

        [MenuItem("Window/Dialogue Editor")]
        public static void OpenWindow()
        {
            instance = GetWindow<DialogueEditor>("Dialogue Editor");
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void OnEnable()
        {

            var dialogueGraph = new DialogueGraphView();


            dialogueGraphView = dialogueGraph;


            var toolbar = new Toolbar();

            var FileLabel = new Label()
            {
                text = "No file Loaded"
            };
            FileLabel.style.flexGrow = 1;
            FileLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            DialogSaver.loadedNodePath += filePath => FileLabel.text = Path.GetFileName(filePath);

            #region BlackboardLoad
            var BlackboardLoad = new Blackboard(dialogueGraph);

            BlackboardLoad.style.display = DisplayStyle.None;
            BlackboardLoad.Q("addButton").style.display = DisplayStyle.None;

            var CloseButtonLoad = new Button(() =>
            {
                BlackboardLoad.style.display = DisplayStyle.None;
            })
            {
                text = "X",
            };
            CloseButtonLoad.style.width = 25;
            CloseButtonLoad.style.height = 20;
            CloseButtonLoad.style.marginLeft = 5;
            BlackboardLoad.Q("header").Add(CloseButtonLoad);
            #endregion


            #region  BlackboardCharacter
            var BlackboardCharacter = new Blackboard(dialogueGraph);

            BlackboardCharacter.style.display = DisplayStyle.None;
            BlackboardCharacter.Q("addButton").style.display = DisplayStyle.None;

            var CloseButtonCharacter = new Button(() =>
            {
                BlackboardCharacter.style.display = DisplayStyle.None;
            })
            {
                text = "X",
            };
            CloseButtonCharacter.style.width = 25;
            CloseButtonCharacter.style.height = 20;
            CloseButtonCharacter.style.marginLeft = 5;
            BlackboardCharacter.Q("header").Add(CloseButtonCharacter);
            #endregion


            var saveButton = new ToolbarButton(() =>
            {
                DialogSaver.instance.SaveGraph();
            })
            {
                text = "Save"
            };


            var loadButton = new ToolbarButton(() =>
            {
                EnableBlackboardLoad(BlackboardLoad);
            })
            {
                text = "Load"
            };

            var newCharacterSetting = new ToolbarButton(() =>
            {
                EnableBlackboardCharacter(BlackboardCharacter);
            })
            {
                text = "Character"
            };


            DialogSaver.instance.dialogueGraphView = dialogueGraph;
            DialogSaver.instance.mainFunction = dialogueGraph.mainFunction;
            DialogSaver.instance.visualFunctions = dialogueGraph.visualFunctions;

            toolbar.Add(loadButton);
            toolbar.Add(saveButton);
            toolbar.Add(newCharacterSetting);
            toolbar.Add(FileLabel);
            rootVisualElement.Add(toolbar);

            dialogueGraph.style.flexGrow = 1;
            rootVisualElement.Add(dialogueGraph);
            rootVisualElement.Add(BlackboardLoad);
            rootVisualElement.Add(BlackboardCharacter);
        }


        public void EnableBlackboardLoad(Blackboard blackboard)
        {
            blackboard.Clear();
            blackboard.style.display = DisplayStyle.Flex;


            var textAssetGUIDs = AssetDatabase.FindAssets("t:DialogFile");
            List<string> paths = new List<string>();
            foreach (var guid in textAssetGUIDs)
            {
                paths.Add(AssetDatabase.GUIDToAssetPath(guid));
            }
            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i];
                var data = DialogSaver.instance.GetData(path);
                if (data != null)
                {
                    var propertyName = System.IO.Path.GetFileNameWithoutExtension(path);
                    var typeText = data.desciption ?? "";

                    var newProperty = new BlackboardField { text = propertyName, typeText = typeText };
                    var textField = newProperty.Q<TextField>();
                    if (textField != null) textField.SetEnabled(false);
                    newProperty.RegisterCallback<MouseDownEvent>(evt =>
                    {
                        if (evt.button == 0 && evt.clickCount == 1)
                        {
                            var CurrentPropertyBlackBoard = new Blackboard();

                            CurrentPropertyBlackBoard.Q("addButton").style.display = DisplayStyle.None;

                            var CloseButtonCharacter = new Button(() =>
                            {
                                CurrentPropertyBlackBoard.RemoveFromHierarchy();
                            })
                            {
                                text = "X",
                            };
                            CloseButtonCharacter.style.width = 25;
                            CloseButtonCharacter.style.height = 20;
                            CloseButtonCharacter.style.marginLeft = 5;
                            CurrentPropertyBlackBoard.Q("header").Add(CloseButtonCharacter);

                            var LoadButton = new Button() { text = "Load Dialog" };

                            LoadButton.clicked += () =>
                            {
                                DialogSaver.instance.LoadData(path);
                            };

                            var DeleteButton = new Button() { text = "Delete Dialog" };

                            DeleteButton.clicked += () =>
                            {
                                if (!string.IsNullOrEmpty(path))
                                {
                                    AssetDatabase.DeleteAsset(path);
                                    AssetDatabase.SaveAssets();
                                    AssetDatabase.Refresh();

                                    CurrentPropertyBlackBoard.RemoveFromHierarchy();
                                }
                            };

                            var DuplicateButton = new Button() { text = "Duplicate Dialog" };

                            DuplicateButton.clicked += () =>
                            {
                                if (!string.IsNullOrEmpty(path))
                                {


                                    string destinationPath = AssetDatabase.GenerateUniqueAssetPath(path);

                                    bool Success = AssetDatabase.CopyAsset(path, destinationPath);

                                    if (Success)
                                    {
                                        AssetDatabase.Refresh();

                                        CurrentPropertyBlackBoard.RemoveFromHierarchy();

                                        DialogSaver.instance.LoadData(destinationPath);
                                    }
                                }
                            };

                            CurrentPropertyBlackBoard.Add(LoadButton);
                            CurrentPropertyBlackBoard.Add(DeleteButton);
                            CurrentPropertyBlackBoard.Add(DuplicateButton);

                            rootVisualElement.Add(CurrentPropertyBlackBoard);
                            blackboard.style.display = DisplayStyle.None;
                        }
                    });
                    blackboard.Add(newProperty);
                }
            }
        }

        public void EnableBlackboardCharacter(Blackboard blackboard)
        {
            blackboard.Clear();

            blackboard.style.display = DisplayStyle.Flex;

            var textFieldName = new TextField
            {
                label = "Character Name"
            };

            var textFieldDescription = new TextField
            {
                label = "Char. Description",
            };

            var imageField = new ObjectField
            {
                label = "Character Image",
                objectType = typeof(Sprite)
            };

            var CreateButton = new Button()
            {
                text = "Create"
            };

            CreateButton.clicked += () =>
                    {
                        if (imageField.value is Sprite sprite)
                        {
                            var data = ScriptableObject.CreateInstance<CharacterData>();

                            data.Name = textFieldName.value;
                            data.Description = textFieldDescription.value;
                            data.Image = sprite;

                            Directory.CreateDirectory("Assets/Resources/Characters");

                            AssetDatabase.CreateAsset(
                                data,
                                $"Assets/Resources/Characters/{data.Name}.asset"
                            );

                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    };
            var ViewCharactersButton = new Button()
            {
                text = "All Characters"
            };

            ViewCharactersButton.clicked += () =>
            {
                var CharachtersBlackboard = new Blackboard();

                CharachtersBlackboard.Q("addButton").style.display = DisplayStyle.None;

                var CloseButtonCharacter = new Button(() =>
                {
                    CharachtersBlackboard.RemoveFromHierarchy();
                })
                {
                    text = "X",
                };
                CloseButtonCharacter.style.width = 25;
                CloseButtonCharacter.style.height = 20;
                CloseButtonCharacter.style.marginLeft = 5;
                CharachtersBlackboard.Q("header").Add(CloseButtonCharacter);

                var CharacterList = new VisualElement();

                var characters = Resources.LoadAll<CharacterData>("");

                foreach (var data in characters)
                {
                    var CharButton = new Button(() => ShowCharacterInfo(data, CharachtersBlackboard))
                    {
                        text = data.name,
                    };
                    CharacterList.Add(CharButton);
                }

                CharachtersBlackboard.Add(CharacterList);

                rootVisualElement.Add(CharachtersBlackboard);


            };





            blackboard.Add(textFieldName);
            blackboard.Add(textFieldDescription);
            blackboard.Add(imageField);
            blackboard.Add(CreateButton);
            blackboard.Add(ViewCharactersButton);

        }

        public void ShowCharacterInfo(CharacterData characterData, Blackboard CharachtersBlackboard = null)
        {
            if (CharachtersBlackboard != null)
            {
                CharachtersBlackboard.RemoveFromHierarchy();
            }

            var showCharacterInfoBlackBoard = new Blackboard();

            showCharacterInfoBlackBoard.Q("addButton").style.display = DisplayStyle.None;

            var CloseButtonCharacter = new Button(() =>
            {
                showCharacterInfoBlackBoard.RemoveFromHierarchy();
            })
            {
                text = "X",
            };
            CloseButtonCharacter.style.width = 25;
            CloseButtonCharacter.style.height = 20;
            CloseButtonCharacter.style.marginLeft = 5;
            showCharacterInfoBlackBoard.Q("header").Add(CloseButtonCharacter);





            var textFieldName = new TextField
            {
                label = "Character Name",
                value = characterData.name
            };

            var textFieldDescription = new TextField
            {
                label = "Char. Description",
                value = characterData.Description,
                multiline = true
            };

            textFieldDescription.style.height = 80;

            var imageField = new ObjectField
            {
                label = "Character Image",
                objectType = typeof(Sprite),
                value = characterData.Image
            };

            var SaveButton = new Button()
            {
                text = "Save"
            };

            var DeleteButton = new Button()
            {
                text = "Delete"
            };

            SaveButton.clicked += () =>
            {
                characterData.Name = textFieldName.value;
                characterData.Description = textFieldDescription.value;
                if (imageField.value is Sprite x)
                {
                    characterData.Image = x;
                }
            };

            DeleteButton.clicked += () =>
            {
                string path = AssetDatabase.GetAssetPath(characterData);

                if (!string.IsNullOrEmpty(path))
                {
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            };

            showCharacterInfoBlackBoard.Add(textFieldName);
            showCharacterInfoBlackBoard.Add(textFieldDescription);
            showCharacterInfoBlackBoard.Add(imageField);
            showCharacterInfoBlackBoard.Add(SaveButton);
            showCharacterInfoBlackBoard.Add(DeleteButton);
            showCharacterInfoBlackBoard.style.width = 250;

            rootVisualElement.Add(showCharacterInfoBlackBoard);
        }
    }
}
#endif