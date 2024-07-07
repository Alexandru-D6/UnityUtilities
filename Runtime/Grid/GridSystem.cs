using System;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Grid {
    public class GridSystem<TGridObject> {

        public enum Dimension { X, Y, Z};
#if UNITY_EDITOR
        private bool _debug = false;
#endif // UNITY_EDITOR

        public Action<int3, TGridObject, bool> OnGridValueChanged;

        private int3 _gridSize;
        private float _cellSize;
        private float3 _originPosition;
        private TGridObject[,,] _gridArray;

#if UNITY_EDITOR
        private TextMesh[,,] _debugTextArray;
        private GameObject _debugContainer;
#endif // UNITY_EDITOR

        public GridSystem(int3 gridSize, float cellSize, float3 originPosition, System.Func<TGridObject> createGridObject) {
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

#if UNITY_EDITOR
            _debugContainer = new GameObject("Debug - (DO NOT DELETE)");

            if (_debug) {
                _debugTextArray = new TextMesh[gridSize.x, gridSize.y, gridSize.z];

                for (int i = 0; i < _gridArray.GetLength(0); ++i) {
                    for (int j = 0; j < _gridArray.GetLength(1); ++j) {
                        for (int k = 0; k < _gridArray.GetLength(2); ++k) {
                            _debugTextArray[i,j,k] = UtilsClass.CreateWorldText(_gridArray[i,j,k]?.ToString(),
                                GetWorldPosition(new int3(i, j, k)) + new float3(cellSize,cellSize,cellSize) * .5f,
                                new float3((gridSize.y == 1) ? 1 : 0, (gridSize.x == 1 && gridSize.z != 1) ? 1 : 0,0),
                                Color.black,
                                fontSize: 60,
                                parent: _debugContainer.transform);
                        }
                    }
                }

                for (int i = 0; i <= _gridArray.GetLength(0); ++i) {
                    for (int j = 0; j <= _gridArray.GetLength(1); ++j) {
                        for (int k = 0; k <= _gridArray.GetLength(2); ++k) {
                            if (i != _gridArray.GetLength(0)) Debug.DrawLine(GetWorldPosition(new int3(i, j, k)), GetWorldPosition(new int3(i+1, j, k)), Color.white, 100f);
                            if (j != _gridArray.GetLength(1)) Debug.DrawLine(GetWorldPosition(new int3(i, j, k)), GetWorldPosition(new int3(i, j+1, k)), Color.white, 100f);
                            if (k != _gridArray.GetLength(2)) Debug.DrawLine(GetWorldPosition(new int3(i, j, k)), GetWorldPosition(new int3(i, j, k+1)), Color.white, 100f);
                        }
                    }
                }
            }
#endif // UNITY_EDITOR

        }

        public float3 GetWorldPosition(int3 position) {
            return new float3(position.x, position.y, position.z) * _cellSize + _originPosition;
        }

        public int3 GetGridPosition(float3 worldPosition) {
            worldPosition -= _originPosition;

            return new int3(  Mathf.FloorToInt((worldPosition.x + 0.05f) / _cellSize),
                Mathf.FloorToInt((worldPosition.y + 0.05f) / _cellSize),
                Mathf.FloorToInt((worldPosition.z + 0.05f) / _cellSize));
        }

        public int3 GetSize() {
            return _gridSize;
        }

        public int GetLength(Dimension dimension) {
            return _gridSize[(int)dimension];
        }

        public bool SetObject(int3 position, TGridObject value, bool activeAction = true) {
            // If it's outside the grid
            if (!InsideGrid(position)) return false;

            // If there's already a piece in the destination cell and you aren't trying to null that position
            if (value != null && _gridArray[position.x, position.y, position.z] != null) return false;

            _gridArray[position.x, position.y, position.z] = value;
            // The trigger is only send if object is not null, that's for the cases when you wanna free that position
            if (OnGridValueChanged != null && value != null) OnGridValueChanged(position, value, activeAction);

#if UNITY_EDITOR
            if (_debug && value != null) {
                _debugTextArray[position.x, position.y, position.z].text = value.ToString();
            }
#endif // UNITY_EDITOR

            return true;
        }

        public bool SetObject(float3 worldPosition, TGridObject value, bool activeAction = true) {
            return SetObject(GetGridPosition(worldPosition), value, activeAction);
        }

        public TGridObject GetObject(int3 position) {
            if (InsideGrid(position)) return _gridArray[position.x, position.y, position.z];

            return default(TGridObject);
        }

        public TGridObject GetObject(float3 worldPosition) {
            return GetObject(GetGridPosition(worldPosition));
        }

        public bool InsideGrid(int3 position) {
            return (position.x >= 0 && position.y >= 0 && position.z >= 0 && position.x < _gridSize.x && position.y < _gridSize.y && position.z < _gridSize.z);
        }

        public bool InsideGrid(float3 worldPosition) {
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
                                GetWorldPosition(new int3(i, j, k)) + new float3(_cellSize,_cellSize,_cellSize) * .5f,
                                new float3((_gridSize.y == 1) ? 1 : 0, (_gridSize.x == 1 && _gridSize.z != 1) ? 1 : 0,0),
                                Color.black,
                                fontSize: 60,
                                parent: _debugContainer.transform);
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
#endif // UNITY_EDITOR
    }
}
