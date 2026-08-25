using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


namespace DialogSystem
{
    public class GraphSettingsPanel : UnityEngine.UIElements.VisualElement

    {
        public GraphSettingsPanel()
        {
            style.position = Position.Absolute;
            style.top = 40;
            style.right = 10;
            style.width = 250;
            style.backgroundColor = new Color(0.18f, 0.18f, 0.18f, 0.95f);
            style.borderBottomLeftRadius = 5;
            style.borderBottomWidth = 2.5f;
            style.borderLeftWidth = 2.5f;
            style.borderTopWidth = 2.5f;
            style.borderRightWidth = 2.5f;
            var title = new Label("Graph Settings");
            title.style.unityFontStyleAndWeight = FontStyle.Bold;

            var CloseButton = new Button();

            CloseButton.style.position = Position.Absolute;
            CloseButton.style.top = 0;
            CloseButton.style.right = 10;
            CloseButton.style.width = 20;
            CloseButton.style.height = 20;
            CloseButton.text = "X";
            CloseButton.RegisterCallback<ClickEvent>(evt =>
            {
                style.display = DisplayStyle.None;
            });

            Add(title);
            Add(CloseButton);

            // Main Settings is for all standard stuff 
            VisualElement MainSettings = new VisualElement();
            Add(MainSettings);

            MainSettings.style.backgroundColor = new Color(0.28f, 0.28f, 0.28f, 0.95f);
            MainSettings.style.borderTopLeftRadius = 5;
            MainSettings.style.top = 8;
            MainSettings.style.height = 60;

            #region  Max Trigger Setting
            IntegerField MaxCountText = new IntegerField("Max Triggers:");
            MaxCountText.tooltip = "The Max Amount the dialog can be played. Use -1 for infinity";
            MaxCountText.name = "AmountText";

            MainSettings.Add(MaxCountText);
            #endregion

            #region Animation Setting

            List<string> Choices = Enum.GetNames(typeof(TextAnimations)).ToList();
            var DropDownFieldAnimation = new DropdownField
            {
                choices = Choices
            };

            MainSettings.Add(DropDownFieldAnimation);
            #endregion
        }
    }
}
namespace DialogSystem
{
    public enum TextAnimations
    {
        None,
        TypewriterClean,
        Typewriter
    }
}