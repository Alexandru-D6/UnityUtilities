#region Using Directives

using System.Collections.Generic;
using OptionSelectorUI;
using OptionSelectorUI.SelectorList;
using UnityEngine;
using UnityEngine.UIElements;

// Pseudo defines (Do after any using directive to be able to use others structs)
using ItemSelectorList = OptionSelectorUI.SelectorList.ItemSelectorList;

#endregion

namespace Samples.OptionSelectorUI.Scripts {
    public class ExampleScript : MonoBehaviour {


        [SerializeField] private Transform _selectorUIPrefab;
        [SerializeField] private Transform _buttonUIPrefab;
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
            List<ItemSelectorList> items = new List<ItemSelectorList> {
                new((int)Animals.Elephant , Animals.Elephant.ToString(), _sprites[0]),
                new((int)Animals.Giraffe  , Animals.Giraffe.ToString() , _sprites[1]),
                new((int)Animals.Pig      , Animals.Pig.ToString()     , _sprites[2]),
                new((int)Animals.Monkey   , Animals.Monkey.ToString()  , _sprites[3])
            };

            new OptionSelector<ItemSelectorList>.Builder()
                .WithSelectorPrefab(_selectorUIPrefab)
                .WithItemPrefab(_buttonUIPrefab)
                .WithName("Animals Selector")
                .WithItems(items)
                .WithParent(_canvas)
                .WithCamera(_camera)
                .WithSize(_selectorSize)
                .WithEvent(OnPieceSelected)
                .BuildSelectorList();
        }

        private void OnPieceSelected(object sender, OptionSelectorUtils.OnItemSelectedArgs e) {
            Debug.Log("Selected piece: " + e.Id);
        }
    }
}
