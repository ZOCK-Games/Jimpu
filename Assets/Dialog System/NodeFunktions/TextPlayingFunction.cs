using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace DialogSystem
{
    public class TextPlayingFunction
    {
        /// <param name="Speed">The Speed per Char 0 is nothing and 100 is fast</param>
        public async Task PlayText(DialogElement dialogElement, string Text, int Speed, TextAnimations textAnimation, Color? color = null)
        {
            if (!dialogElement.gameObject.activeSelf)
            {
                dialogElement.gameObject.SetActive(true);
            }

            Color TextColor = Color.white;
            if (color.HasValue)
            {
                TextColor = color.Value;
            }
            dialogElement.narratorText.color = TextColor;
            string invText = $"<color=#ffffff00>{Text}</color>";
            dialogElement.narratorText.text = invText;

            var allChars = Text.ToCharArray();
            for (int i = 0; i < allChars.Count(); i++)
            {
                string visiblePart = new string(allChars, 0, i);
                string invisiblePart = new string(allChars, i, allChars.Length - i);
                if (invisiblePart.Length > 0)
                {
                    dialogElement.narratorText.text = $"{visiblePart}<color=#ffffff00>{invisiblePart}</color>";
                }
                else
                {
                    dialogElement.narratorText.text = Text;
                }
                await Task.Delay(Speed);
            }
            dialogElement.narratorText.text = Text;
        }
    }
}