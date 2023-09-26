using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;

namespace OptionSelectorUI {

    public class OptionSelector : MonoBehaviour {

        private List<string> _namesToDisplay;
        private Vector2 _boxSize;
        private bool _destroyOnButtonPressed = true;
        private bool _destroyOnMouseClick = true;

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

            if (_destroyOnButtonPressed) {
                Destroy(gameObject);
            }
        }

        public void SetDestroyOnButtonPressed(bool value) {
            _destroyOnButtonPressed = value;
        }

        public void SetDestroyOnMouseClick(bool value) {
            _destroyOnMouseClick = value;
        }

        private void Update() {
            if (_destroyOnMouseClick && Input.GetMouseButtonUp((int) MouseButton.LeftMouse)) {
                Destroy(gameObject);
            }
        }

#region Private Methods

        private void InitializeButtonsVerticalList() {
            float incrememtsPosY = Mathf.Floor(_boxSize.y / _namesToDisplay.Count);
            Vector2 currentPos;
            Vector2 signs = new Vector2(
                ((Input.mousePosition.x + _boxSize.x > Camera.main.pixelWidth) ? -1f : 1f),
                ((Input.mousePosition.y - (_namesToDisplay.Count * (incrememtsPosY + 0.5f))  > 0f) ? -1f : 1f)
            );

            currentPos = new Vector2(
                (signs.x < 0f) ? -1f * _boxSize.x : 0f,
                (signs.y < 0f) ? 0f : incrememtsPosY
            );

            _itemsCollection.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, signs.y > 0f ? 0f : 1f);

            foreach (var name in _namesToDisplay) {
                Transform buttonObject = Instantiate(_itemPrefab, _itemsCollection);
                buttonObject.gameObject.name = (name + "Button");

                // Click action
                Button button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(() => {
                    ButtonPressed(name);
                });

                EventTrigger trigger = buttonObject.GetComponent<EventTrigger>();
                // Make a trigger that on mouse enter call the function SetDestroyOnClick
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((data) => { SetDestroyOnMouseClick(false); });
                trigger.triggers.Add(entry);

                // Make a trigger that on mouse exit call the function SetDestroyOnClick
                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener((data) => { SetDestroyOnMouseClick(true); });
                trigger.triggers.Add(entry);

                // TMP_Text text
                TMP_Text textObject = buttonObject.GetComponentInChildren<TMP_Text>();
                textObject.text = name;

                // GameObject position
                Vector3 backupPos = buttonObject.localPosition;
                buttonObject.localPosition = new Vector3(currentPos.x, currentPos.y, backupPos.z);

                // Button size
                RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(_boxSize.x, incrememtsPosY - 0.5f);

                currentPos += new Vector2(0f, signs.y * (incrememtsPosY));
            }
        }

#endregion
    }
}
