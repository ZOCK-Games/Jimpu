using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DialogSystem
{
    public static class DialogObjects
    {

        [MenuItem("GameObject/Dialog/Dialog Window", false, 10)]
        private static void CreateDialogWindow(MenuCommand menuCommand)
        {
            var Canvas = Object.FindAnyObjectByType<Canvas>();
            if (Canvas == null)
            {
                Canvas = new GameObject("Canvas").AddComponent<Canvas>();
                Canvas.gameObject.AddComponent<CanvasScaler>();
                Canvas.gameObject.AddComponent<GraphicRaycaster>();
                Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            GameObject dialog = new GameObject("Dialog Window");

            dialog.transform.SetParent(Canvas.transform);

            dialog.AddComponent<DialogElement>();

            GameObjectUtility.SetParentAndAlign(
                dialog,
                menuCommand.context as GameObject
            );

            Undo.RegisterCreatedObjectUndo(
                dialog,
                "Create Dialog Window"
            );

            Selection.activeGameObject = dialog;
        }

        [MenuItem("GameObject/Dialog/Dialog Trigger", false, 10)]
        private static void CreateDialogTrigger(MenuCommand menuCommand)
        {
            GameObject trigger = new GameObject("Dialog Trigger");

            var dialogTrigger = trigger.AddComponent<DialogTrigger>();

            dialogTrigger.dialogElement = Object.FindAnyObjectByType<DialogElement>();

            GameObjectUtility.SetParentAndAlign(
                trigger,
                menuCommand.context as GameObject
            );

            Undo.RegisterCreatedObjectUndo(
                trigger,
                "Create Dialog Trigger"
            );

            Selection.activeGameObject = trigger;
        }
    }
}