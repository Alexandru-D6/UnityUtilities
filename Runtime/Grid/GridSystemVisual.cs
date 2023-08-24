using System;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Grid {
    public class GridSystemVisual : MonoBehaviour {

        private GridSystem<Cell> _inventory;
        [SerializeField] private Transform cellPrefab;
        [SerializeField] private Transform hollowCell;

        private Cell _currentCell;
        [SerializeField] private Vector3Int gridSize = new Vector3Int(8, 1, 8);
        [SerializeField] private float cellSize = 1.0f;
        [SerializeField] private bool snappingToGrid = false;

        private Camera _camera;

        private void Awake() {
            _camera = Camera.main;
        }

        void Start() {
            _inventory = new GridSystem<Cell>(gridSize, cellSize, transform.position, () => { return null; });
            _inventory.OnGridValueChanged += OnCellChanged;

            for (int i = 0; i < _inventory.GetLength(0); ++i) {
                Transform cellObject = Instantiate(cellPrefab, transform, false);

                Cell cell = new Cell(i, 0, cellSize, true);
                cell.SetGameObject(cellObject);
                cell.SetPosition(new Vector2Int(i,0));

                _inventory.SetObject(new Vector3Int(i,0,0), cell);
            }
        }

        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Vector3 mouse = UtilsClass.GetMouseWorldPositionOverPlane(_camera, GetGridPlane(0.0f));

                _currentCell = _inventory.GetObject(mouse);
                if (_currentCell != null) {
                    _currentCell.SetActiveObject(false);
                    _inventory.SetObject(new Vector3Int(_currentCell.GetPosition().x, 0, _currentCell.GetPosition().y), null);
                    hollowCell.gameObject.SetActive(true);
                }

            }else if (Input.GetMouseButtonUp(0) && _currentCell != null) {
                Vector3 mouse = UtilsClass.GetMouseWorldPositionOverPlane(_camera, GetGridPlane(0.0f));

                Vector2Int lastCellPosition = _currentCell.GetPosition();
                if (!_inventory.SetObject(mouse, _currentCell)) {
                    _inventory.SetObject(new Vector3Int(_currentCell.GetPosition().x, 0, _currentCell.GetPosition().y), _currentCell);
                }

                _currentCell.SetActiveObject(true);
                _currentCell = null;
                hollowCell.gameObject.SetActive(false);
            }

            if (_currentCell != null) {
                Vector3 mouse = UtilsClass.GetMouseWorldPositionOverPlane(_camera, GetGridPlane(0.0f));

                mouse = mouse - (new Vector3(cellSize,-0.02f,cellSize) / 2.0f);

                if (snappingToGrid) {
                    float prevY = mouse.y;
                    mouse = UtilsClass.Vector3ToVector3Floor((mouse / cellSize) + (new Vector3(cellSize,cellSize,cellSize)/4.0f)) * cellSize;
                    mouse.y = prevY;
                }

                hollowCell.position = mouse;
            }
        }

        private Plane GetGridPlane(float offset) {
            return new Plane(new Vector3(0.0f,1.0f,0.0f), new Vector3(0.0f,transform.position.y + offset,0.0f));
        }

        private void OnCellChanged(Vector3Int position, Cell cell, bool activeAction) {
            cell.SetPosition(new Vector2Int(position.x, position.z));
        }
    }
}
