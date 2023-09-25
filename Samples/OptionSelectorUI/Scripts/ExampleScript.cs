using System;
using System.Collections.Generic;
using OptionSelectorUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Samples.OptionSelectorUI.Scripts {
    public class ExampleScript : MonoBehaviour {

        [SerializeField] private OptionSelector _selectorUIPrefab;
        [SerializeField] private Transform _canvas;

        private Camera _camera;
        void Start() {
            _camera = Camera.main;
        }

        void Update() {
            if (Input.GetMouseButtonDown((int) MouseButton.RightMouse)) {
                InstantiateSelectorUI();
            }
        }

        private OptionSelector InstantiateSelectorUI() {
            OptionSelector result = Instantiate(_selectorUIPrefab).GetComponentInChildren<OptionSelector>();
            result.Initialize("Piece Promotion Selector", new List<string>(){"Queen", "Rook", "Bishop"}, _canvas);
            result.OnItemSelected += OnPieceSelected;

            return result;
        }

        private void OnPieceSelected(object sender, OptionSelector.OnItemSelectedArgs e) {
            Debug.Log("Selected piece: " + e.ButtonName);
        }
    }
}
