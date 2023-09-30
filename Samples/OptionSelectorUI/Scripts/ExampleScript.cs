#region Using Directives

using System.Collections.Generic;
using OptionSelectorUI;
using UnityEngine;
using UnityEngine.UIElements;

// Pseudo defines (Do after any using directive to be able to use others structs)
using ItemSelectorList = OptionSelectorUI.SelectorList.ItemSelectorList<Samples.OptionSelectorUI.Scripts.Animals>;

#endregion

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
            OptionSelector<ItemSelectorList, Animals> result = Instantiate(_selectorUIPrefab).GetComponentInChildren<OptionSelectorListWrapper>();

            List<ItemSelectorList> items = new List<ItemSelectorList> {
                new(Animals.Elephant , Animals.Elephant.ToString(), _sprites[0]),
                new(Animals.Giraffe  , Animals.Giraffe.ToString() , _sprites[1]),
                new(Animals.Pig      , Animals.Pig.ToString()     , _sprites[2]),
                new(Animals.Monkey   , Animals.Monkey.ToString()  , _sprites[3])
            };

            result.Initialize("Piece Promotion Selector", items, _canvas, _camera, _selectorSize, new Vector2(1f, -1f));
            result.SetDestroyOnButtonPressed(false);
            result.OnItemSelected += OnPieceSelected;
        }

        private void OnPieceSelected(object sender, OptionSelectorUtils.OnItemSelectedArgs<Animals> e) {
            Debug.Log("Selected piece: " + e.Id);
        }
    }
}
