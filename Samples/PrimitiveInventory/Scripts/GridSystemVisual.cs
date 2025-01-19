using Grid;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Samples.PrimitiveInventory.Scripts {
    public class GridSystemVisual : MonoBehaviour {

        private GridSystem<Cell> _inventory;
        [SerializeField] private Transform cellPrefab;
        [SerializeField] private Transform hollowCell;

        [SerializeField] public Cell _currentCell;
        [SerializeField] private int3 gridSize = new int3(8, 1, 8);
        [SerializeField] private float cellSize = 1.0f;
        [SerializeField] private bool snappingToGrid = false;

        private Camera _camera;

        private void Awake() {
            _camera = Camera.main;
        }

        void Start() {
            _inventory = new GridSystem<Cell>(gridSize, cellSize, transform.position, () => { return null; });
#if UNITY_EDITOR
            _inventory.SetDebug(false);
#endif // UNITY_EDITOR
            _inventory.OnGridValueChanged += OnCellChanged;

            for (int i = 0; i < _inventory.GetLength(0); ++i) {
                Transform cellObject = Instantiate(cellPrefab, transform, false);
                cellObject.localScale = new Vector3(cellSize, cellSize, cellSize);
                cellObject.localPosition = new Vector3(i, 0, 0) * cellSize;

                int3 position = new int3(i, 0, 0);
                Cell cell = new Cell(position, cellSize, true);
                cell.SetObject(cellObject);

                _inventory.SetObject(position, cell);
            }
        }

        void Update() {
            if (Input.GetMouseButtonDown(0) && _currentCell == null) {
                Vector3 mouse = UtilsClass.GetMouseWorldPositionOverPlane(_camera, GetGridPlane(0.0f));

                _currentCell = _inventory.GetObject(mouse);
                if (_currentCell != null) {
                    ((Transform) _currentCell.GetObject()).gameObject.SetActive(false);
                    _inventory.SetObject(new int3(_currentCell.GetPosition().x, 0, _currentCell.GetPosition().z), null);
                    hollowCell.gameObject.SetActive(true);
                }

            }

            if (Input.GetMouseButtonUp(0) && _currentCell != null) {
                Vector3 mouse = UtilsClass.GetMouseWorldPositionOverPlane(_camera, GetGridPlane(0.0f));

                if (!_inventory.SetObject(mouse, _currentCell)) {
                    _inventory.SetObject(new int3(_currentCell.GetPosition().x, 0, _currentCell.GetPosition().z), _currentCell);
                }

                ((Transform) _currentCell.GetObject()).gameObject.SetActive(true);
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

        private void OnCellChanged(int3 position, Cell cell, bool activeAction) {
            cell.SetPosition(position);
            Transform cellObject = (Transform) cell.GetObject();
            cellObject.localPosition = new Vector3(position.x, 0, position.z) * cellSize;
        }
    }
}
