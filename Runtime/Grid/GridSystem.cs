using System;
using UnityEngine;
using Utils;

namespace Grid {
    public class GridSystem<TGridObject> {

        public enum Dimension { X, Y, Z};
        private bool _debug = false;

        public Action<Vector3Int, TGridObject, bool> OnGridValueChanged;

        private Vector3Int _gridSize;
        private float _cellSize;
        private Vector3 _originPosition;
        private TGridObject[,,] _gridArray;

        private TextMesh[,,] _debugTextArray;

        public GridSystem(Vector3Int gridSize, float cellSize, Vector3 originPosition, System.Func<TGridObject> createGridObject) {
            this._gridSize = gridSize;
            this._cellSize = cellSize;
            this._originPosition = originPosition;

            _gridArray = new TGridObject[gridSize.x, gridSize.y, gridSize.z];

            for (int i = 0; i < _gridArray.GetLength(0); ++i) {
                for (int j = 0; j < _gridArray.GetLength(1); ++j) {
                    for (int k = 0; k < _gridArray.GetLength(2); ++k) {
                        _gridArray[i, j, k] = createGridObject();
                    }
                }
            }

            if (_debug) {
                _debugTextArray = new TextMesh[gridSize.x, gridSize.y, gridSize.z];

                for (int i = 0; i < _gridArray.GetLength(0); ++i) {
                    for (int j = 0; j < _gridArray.GetLength(1); ++j) {
                        for (int k = 0; k < _gridArray.GetLength(2); ++k) {
                            _debugTextArray[i,j,k] = UtilsClass.CreateWorldText(_gridArray[i,j,k]?.ToString(),
                                GetWorldPosition(new Vector3Int(i, j, k)) + new Vector3(cellSize,cellSize,cellSize) * .5f,
                                new Vector3((gridSize.y == 1) ? 1 : 0, (gridSize.x == 1 && gridSize.z != 1) ? 1 : 0,0),
                                Color.black,
                                fontSize: 60);
                        }
                    }
                }

                for (int i = 0; i <= _gridArray.GetLength(0); ++i) {
                    for (int j = 0; j <= _gridArray.GetLength(1); ++j) {
                        for (int k = 0; k <= _gridArray.GetLength(2); ++k) {
                            if (i != _gridArray.GetLength(0)) Debug.DrawLine(GetWorldPosition(new Vector3Int(i, j, k)), GetWorldPosition(new Vector3Int(i+1, j, k)), Color.white, 100f);
                            if (j != _gridArray.GetLength(1)) Debug.DrawLine(GetWorldPosition(new Vector3Int(i, j, k)), GetWorldPosition(new Vector3Int(i, j+1, k)), Color.white, 100f);
                            if (k != _gridArray.GetLength(2)) Debug.DrawLine(GetWorldPosition(new Vector3Int(i, j, k)), GetWorldPosition(new Vector3Int(i, j, k+1)), Color.white, 100f);
                        }
                    }
                }
            }

        }

        public Vector3 GetWorldPosition(Vector3Int position) {
            return new Vector3(position.x, position.y, position.z) * _cellSize + _originPosition;
        }

        public Vector3Int GetGridPosition(Vector3 worldPosition) {
            worldPosition -= _originPosition;

            return new Vector3Int(  Mathf.FloorToInt((worldPosition.x + 0.05f) / _cellSize),
                Mathf.FloorToInt((worldPosition.y + 0.05f) / _cellSize),
                Mathf.FloorToInt((worldPosition.z + 0.05f) / _cellSize));
        }

        public Vector3Int GetSize() {
            return _gridSize;
        }

        public int GetLength(Dimension dimension) {
            return _gridSize[(int)dimension];
        }

        public bool SetObject(Vector3Int position, TGridObject value, bool activeAction = true) {
            // If it's outside the grid
            if (!InsideGrid(position)) return false;

            // If there's already a piece in the destination cell and you aren't trying to null that position
            if (value != null && _gridArray[position.x, position.y, position.z] != null) return false;

            _gridArray[position.x, position.y, position.z] = value;
            // The trigger is only send if object is not null, that's for the cases when you wanna free that position
            if (OnGridValueChanged != null && value != null) OnGridValueChanged(position, value, activeAction);

            if (_debug && value != null) {
                _debugTextArray[position.x, position.y, position.z].text = value.ToString();
            }

            return true;
        }

        public bool SetObject(Vector3 worldPosition, TGridObject value, bool activeAction = true) {
            return SetObject(GetGridPosition(worldPosition), value, activeAction);
        }

        public TGridObject GetObject(Vector3Int position) {
            if (InsideGrid(position)) return _gridArray[position.x, position.y, position.z];

            return default(TGridObject);
        }

        public TGridObject GetObject(Vector3 worldPosition) {
            return GetObject(GetGridPosition(worldPosition));
        }

        public bool InsideGrid(Vector3Int position) {
            return (position.x >= 0 && position.y >= 0 && position.z >= 0 && position.x < _gridSize.x && position.y < _gridSize.y && position.z < _gridSize.z);
        }

        public bool InsideGrid(Vector3 worldPosition) {
            return InsideGrid(GetGridPosition(worldPosition));
        }

#if UNITY_EDITOR
        public void SetDebug(bool value) {
            _debug = value;

            if (value) {
                _debugTextArray = new TextMesh[_gridSize.x, _gridSize.y, _gridSize.z];

                for (int i = 0; i < _gridArray.GetLength(0); ++i) {
                    for (int j = 0; j < _gridArray.GetLength(1); ++j) {
                        for (int k = 0; k < _gridArray.GetLength(2); ++k) {
                            _debugTextArray[i,j,k] = UtilsClass.CreateWorldText(_gridArray[i,j,k]?.ToString(),
                                GetWorldPosition(new Vector3Int(i, j, k)) + new Vector3(_cellSize,_cellSize,_cellSize) * .5f,
                                new Vector3((_gridSize.y == 1) ? 1 : 0, (_gridSize.x == 1 && _gridSize.z != 1) ? 1 : 0,0),
                                Color.black,
                                fontSize: 60);
                        }
                    }
                }
            }
        }

        public void UpdateDebug() {
            if (!_debug) return;

            for (int i = 0; i < _gridArray.GetLength(0); ++i) {
                for (int j = 0; j < _gridArray.GetLength(1); ++j) {
                    for (int k = 0; k < _gridArray.GetLength(2); ++k) {
                        _debugTextArray[i,j,k].text = _gridArray[i,j,k]?.ToString();
                    }
                }
            }
        }
    }
#endif // UNITY_EDITOR
}
