using UnityEngine;

namespace Grid {
    public class Cell {

        [SerializeField] private Transform _cellGameObject;

        private Vector2Int _position;
        private float _gridSize;
        private bool _value;

        public Cell() {
            _position.x = -1;
            _position.y = -1;
            _gridSize = 0.0f;
            _value = false;
        }

        public Cell(int x, int y, float gridSize) {
            _position.x = x;
            _position.y = y;
            this._gridSize = gridSize;
            _value = false;
        }

        public Cell(int x, int y, float gridSize, bool value) {
            _position.x = x;
            _position.y = y;
            this._gridSize = gridSize;
            this._value = value;
        }

        public void SetValue(bool value) {
            this._value = value;
            _cellGameObject.gameObject.SetActive(value);
        }

        public bool GetValue() {
            return _value;
        }

        public void SetPosition(Vector2Int position) {
            this._position = position;

            _cellGameObject.localPosition = new Vector3(position.x, 0.0f, position.y) * _gridSize;
        }

        public Vector2Int GetPosition() {
            return _position;
        }

        public void SetGameObject(Transform cell) {
            _cellGameObject = cell;
            _cellGameObject.localScale = new Vector3(_gridSize,_gridSize,_gridSize);
        }

        public void SetActiveObject(bool value) {
            _cellGameObject.gameObject.SetActive(value);
        }

        public override string ToString() {
            if (_cellGameObject != null) return "{ " + _position.x + " - " + _position.y + " } -> " + _value;
            else return "null";
        }
    }
}
