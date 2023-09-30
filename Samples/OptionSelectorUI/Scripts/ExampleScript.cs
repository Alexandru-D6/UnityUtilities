#region Using Directives

using System.Collections.Generic;
using OptionSelectorUI;
using OptionSelectorUI.SelectorList;
using UnityEngine;
using UnityEngine.UIElements;

#endregion

namespace Samples.OptionSelectorUI.Scripts {
    public class ExampleScript : MonoBehaviour {

        [SerializeField] private Transform _animalSelectorList;
        [SerializeField] private Transform _buttonUIPrefab;

        [SerializeField] private List<Sprite> _sprites;

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
                .WithParent(_animalSelectorList)
                .WithItemPrefab(_buttonUIPrefab)
                .WithName("Animals Selector")
                .WithItems(items)
                .WithCamera(_camera)
                .WithEvent(OnPieceSelected)
                .BuildSelectorList();
        }

        private void OnPieceSelected(object sender, OptionSelectorUtils.OnItemSelectedArgs e) {
            Debug.Log("Selected piece: " + e.Id);
        }
    }
}
