using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using Button = UnityEngine.UI.Button;

namespace OptionSelectorUI {

    public class OptionSelector : MonoBehaviour {

        private List<string> _namesToDisplay;
        private Vector2 _boxSize;

        [SerializeField] private Transform _itemsCollection;
        [SerializeField] private Transform _itemPrefab;

#region Callback function

        public event EventHandler<OnItemSelectedArgs> OnItemSelected;

        public class OnItemSelectedArgs : EventArgs {
            public String ButtonName;
        }

#endregion

        public void Initialize(string name, List<string> namesToDisplay, Transform canvasParent, Vector2 boxSize) {
            // Handle multiple instances of selector with the same Name
            Transform prevSelector = canvasParent.Find(name);

            if (prevSelector != null) {
                Destroy(prevSelector.gameObject);
            }

            // Saving variables
            _namesToDisplay = namesToDisplay;
            _boxSize = boxSize;

            // Config gameObject
            gameObject.name = name;
            transform.SetParent(canvasParent, true);

            // Initialize selector
            InitializeButtonsVerticalList();
            _itemsCollection.position = Input.mousePosition;
        }

        public void ButtonPressed(string name) {
            OnItemSelected?.Invoke(this, new OnItemSelectedArgs {
                ButtonName = name
            });

            Destroy(gameObject);
        }

        private void Update() {
            if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse)) {
                Destroy(gameObject);
            }
        }

#region Private Methods

        private void InitializeButtonsVerticalList() {
            float currentPosY = 0f;
            float incrememtsPosY = Mathf.Floor(_boxSize.y / _namesToDisplay.Count);

            foreach (var name in _namesToDisplay) {
                Transform buttonObject = Instantiate(_itemPrefab, _itemsCollection);
                buttonObject.gameObject.name = (name + "Button");

                // Click action
                Button button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(() => {
                    ButtonPressed(name);
                });

                // TMP_Text text
                TMP_Text textObject = buttonObject.GetComponentInChildren<TMP_Text>();
                textObject.text = name;

                // GameObject position
                Vector3 backupPos = buttonObject.localPosition;
                buttonObject.localPosition = new Vector3(backupPos.x, -1f * currentPosY, backupPos.z);

                // Button size
                RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(_boxSize.x, incrememtsPosY - 0.5f);

                currentPosY += incrememtsPosY;
            }
        }

#endregion
    }
}
