using UnityEngine;

namespace ButtonMenuUI {
    public interface IMenuItem {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sprite Sprite { get; set; }
    }
}
