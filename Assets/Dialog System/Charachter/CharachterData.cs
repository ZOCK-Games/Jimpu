using System;
using UnityEngine;
namespace DialogSystem
{
    public class CharacterData : ScriptableObject
    {
        public string Name;
        public string Description;
        public string ID;
        public Sprite Image;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(ID))
                ID = Guid.NewGuid().ToString();
        }
    }
}