using System;
using System.Collections.Generic;
using OptionSelectorUI;
using OptionSelectorUI.SelectorList;
using UnityEngine;
using UnityEngine.UIElements;

namespace Samples.OptionSelectorUI.Scripts {
    public class ExampleScript : MonoBehaviour {

        [SerializeField] private Transform _selectorUIPrefab;
        [SerializeField] private Transform _canvas;

        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private Vector2 _selectorSize = new(160f, 120f);

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
            OptionSelector<ItemSelectorList> result = Instantiate(_selectorUIPrefab).GetComponentInChildren<OptionSelectorList>();

            List<ItemSelectorList> items = new List<ItemSelectorList> {
                new ItemSelectorList { id = "1", name = "", sprite = _sprites[0] },
                new ItemSelectorList { id = "2", name = "", sprite = _sprites[1] },
                new ItemSelectorList { id = "3", name = "", sprite = _sprites[2] },
                new ItemSelectorList { id = "4", name = "", sprite = _sprites[3] }
            };

            result.Initialize("Piece Promotion Selector", items, _canvas, _selectorSize, new Vector2(1f, -1f));
            result.SetDestroyOnButtonPressed(false);
            result.OnItemSelected += OnPieceSelected;
        }

        private void OnPieceSelected(object sender, OptionSelectorUtils.OnItemSelectedArgs e) {
            Debug.Log("Selected piece: " + e.id);
        }

        public void PrintDebug(string message) {
            Debug.Log(message);
        }
    }
}
