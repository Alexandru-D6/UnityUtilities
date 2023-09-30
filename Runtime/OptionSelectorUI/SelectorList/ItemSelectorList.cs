using UnityEngine;

namespace OptionSelectorUI.SelectorList {
    public struct ItemSelectorList<T> {
        public readonly T Id;
        public readonly string Name;
        public readonly Sprite Sprite;

        public ItemSelectorList(T id, string name, Sprite sprite) {
            if (id == null) {
                throw new System.ArgumentException("Id must be greater than 0");
            }

            Id = id;
            Name = name;
            Sprite = sprite;
        }
    }
}
