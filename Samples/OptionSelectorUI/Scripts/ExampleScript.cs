using System;
using System.Collections.Generic;
using OptionSelectorUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Samples.OptionSelectorUI.Scripts {
    public class ExampleScript : MonoBehaviour {

        [SerializeField] private Transform _selectorUIPrefab;
        [SerializeField] private Transform _canvas;

        private Camera _camera;
        void Start() {
            _camera = Camera.main;
        }

        void Update() {
            if (Input.GetMouseButtonDown((int) MouseButton.RightMouse)) {
                InstantiateSelectorList();
            }
        }

        private void InstantiateSelectorList() {
            OptionSelector<ListItemScriptableObject> result = Instantiate(_selectorUIPrefab).GetComponentInChildren<OptionSelector<ListItemScriptableObject>>();
            List<ListItemScriptableObject> items = new List<ListItemScriptableObject> {
                new ListItemScriptableObject { name = "Pawn",   sprite = null },
                new ListItemScriptableObject { name = "Knight", sprite = null },
                new ListItemScriptableObject { name = "Bishop", sprite = null },
                new ListItemScriptableObject { name = "Rook",   sprite = null }
            };

            result.Initialize("Piece Promotion Selector", items, _canvas, new Vector2(150f, 200f), new Vector2(1f, -1f));
            result.SetDestroyOnButtonPressed(false);
            result.OnItemSelected += OnPieceSelected;
        }

        private void OnPieceSelected(object sender, OptionSelectorUtils.OnItemSelectedArgs e) {
            Debug.Log("Selected piece: " + e.id);
        }
    }
}
