using UnityEngine;

namespace OptionSelectorUI.SelectorList {
    public struct ItemSelectorList {
        public readonly int Id;
        public readonly string Name;
        public readonly Sprite Sprite;

        public ItemSelectorList(int id, string name, Sprite sprite) {
            if (id < 0) {
                throw new System.ArgumentException("Id must be greater than 0");
            }

            Id = id;
            Name = name;
            Sprite = sprite;
        }
    }
}
