using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TunaszUtils
{
    [System.Serializable]
    public class ItemPin
    {
        public string pinName;

        public List<string> pinContent;

        public ItemPin(string pinName)
        {
            this.pinName = pinName;
            this.pinContent = new List<string>();
        }
    }
    public class PinData : ScriptableObject
    {
        public List<ItemPin> itemPins;
        public int selectedPin;

        public PinData(List<ItemPin> itemPins)
        {
            this.itemPins = itemPins;
            this.selectedPin = 0;
        }
    }
}