using Unity.Mathematics;
using UnityEngine;

namespace Grid {
    public class Cell {

        private Object _object;

        private int3 _position;
        private bool _value;

        public Cell() {
            _position = new int3(-1, -1, -1);
            _value = false;
        }

        public Cell(int3 position, float gridSize) {
            _position = position;
            _value = false;
        }

        public Cell(int3 position, float gridSize, bool value) {
            _position = position;
            this._value = value;
        }

        public void SetPosition(int3 position) {
            this._position = position;
        }

        public int3 GetPosition() {
            return _position;
        }

        public void SetObject(Object obj) {
            this._object = obj;
        }

        public Object GetObject() {
            return _object;
        }

        public override string ToString() {
            return _position.ToString() + " -> " + _value;
        }
    }
}
