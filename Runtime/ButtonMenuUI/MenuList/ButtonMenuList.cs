using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace ButtonMenuUI.MenuList {

    public class SelectionMenuList : ButtonMenuBase {

        private enum ButtonType {
            ImageAndText,
            OnlyImage,
            OnlyText,
            Null
        }

        private float _textScaleFactor = 0.65f;

        private ButtonType _buttonType = ButtonType.Null;

        void InitializeLayout() {
            VerticalLayoutGroup verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            Canvas canvas = GetComponentInParent<Canvas>();

            verticalLayoutGroup.spacing = 2.5f;

            // The position (0,0,0) is the bottom left corner of the screen
            Vector2 signs = new Vector2(
                (_direction.x < 0f) ?
                    (transform.position.x - (DEFAULT_SELECTOR_SIZE.x * _selectorSize.x * canvas.scaleFactor) <= 0f ? 1f : _direction.x) :
                    (transform.position.x + (DEFAULT_SELECTOR_SIZE.x * _selectorSize.x * canvas.scaleFactor) >= _camera.pixelWidth ? -1f : _direction.x),
                (_direction.y > 0f) ?
                    (transform.position.y + (DEFAULT_SELECTOR_SIZE.y * _selectorSize.y * canvas.scaleFactor) >= _camera.pixelHeight ? -1f : _direction.y) :
                    (transform.position.y - (DEFAULT_SELECTOR_SIZE.y * _selectorSize.y * canvas.scaleFactor) <= 0f ? 1f : _direction.y)
            );

            transform.localPosition += new Vector3(
                (int)_direction.x != (int)signs.x ?
                    signs.x * DEFAULT_SELECTOR_SIZE.x * _selectorSize.x :
                    0f,
                (int)_direction.y != (int)signs.y ?
                    signs.y * DEFAULT_SELECTOR_SIZE.y * _selectorSize.y :
                    0f,
                0f);
        }

        protected override void InitializeButtons() {
            InitializeLayout();

            float incrementsPosY = Mathf.Floor((DEFAULT_SELECTOR_SIZE.y * _selectorSize.y) / _items.Count);

            // Define button type
            if (_items[0].Name != "" && _items[0].Sprite != null) {
                _buttonType = ButtonType.ImageAndText;
            } else if (_items[0].Name == "") {
                _buttonType = ButtonType.OnlyImage;
            } else {
                _buttonType = ButtonType.OnlyText;
            }

            // Check prefab to have all game objects
            Assert.IsNotNull(_itemPrefab);
            Assert.IsNotNull(_itemPrefab.GetComponent<Button>());
            if (_buttonType != ButtonType.OnlyText) {
                Assert.IsTrue(_itemPrefab.GetComponentsInChildren<Image>().Length > 1);
            }
            if (_buttonType != ButtonType.OnlyImage) {
                Assert.IsNotNull(_itemPrefab.GetComponentInChildren<TMP_Text>());
            }

            foreach (var item in _items) {
                Assert.IsFalse(item == null || (item.Name == "" && item.Sprite == null));

                Transform buttonObject = Instantiate(_itemPrefab, transform);

                if (item.Name != "" && item.Sprite != null) {
                    Assert.IsFalse(_buttonType != ButtonType.ImageAndText, "All items must have the same type.");
                } else if (item.Name == "") {
                    Assert.IsFalse(_buttonType != ButtonType.OnlyImage, "All items must have the same type.");
                } else {
                    Assert.IsFalse(_buttonType != ButtonType.OnlyText, "All items must have the same type.");
                }

                buttonObject.gameObject.name = (item.Name != "" ? item.Name : item.Sprite.name) + "Button";

                // Click action
                Button button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(() => {
                    ButtonPressed(item);
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
                if (_buttonType != ButtonType.OnlyImage) {
                    TMP_Text textObject = buttonObject.GetComponentInChildren<TMP_Text>();
                    textObject.text = item.Name;

                    Vector2 anchorMax = textObject.GetComponent<RectTransform>().anchorMax;
                    Vector2 anchorMin = textObject.GetComponent<RectTransform>().anchorMin;
                    // The value 0.85f is a magic number that works for the default font size
                    float textScaleFactor = (anchorMax.y - anchorMin.y) * 0.85f * _textScaleFactor;

                    textObject.fontSize = incrementsPosY * textScaleFactor;
                }

                // Image sprite
                if (_buttonType != ButtonType.OnlyText) {
                    Image imageObject = null;

                    foreach (var image in buttonObject.GetComponentsInChildren<Image>()) {
                        if (image.gameObject != buttonObject.gameObject) {
                            imageObject = image;
                            break;
                        }
                    }

                    Assert.IsNotNull(imageObject);

                    imageObject.sprite = item.Sprite;

                    // Image scale factor
                    Vector2 spriteScaleFactor = new Vector2(item.Sprite.bounds.extents.y / item.Sprite.bounds.extents.x,
                                                            item.Sprite.bounds.extents.x / item.Sprite.bounds.extents.y);
                    Vector2 anchorMax = imageObject.GetComponent<RectTransform>().anchorMax;
                    Vector2 anchorMin = imageObject.GetComponent<RectTransform>().anchorMin;
                    Vector2 imageScaleFactor = new Vector2(incrementsPosY / (DEFAULT_SELECTOR_SIZE.x * _selectorSize.x * (anchorMax.x - anchorMin.x)),
                                                            (DEFAULT_SELECTOR_SIZE.x * _selectorSize.x * (anchorMax.x - anchorMin.x)) / incrementsPosY);

                    imageObject.GetComponent<RectTransform>().localScale = new Vector3( 1f * Mathf.Clamp(imageScaleFactor.x * spriteScaleFactor.y , 0f, 1f),
                                                                                        1f * Mathf.Clamp(imageScaleFactor.y * spriteScaleFactor.x, 0f, 1f),
                                                                                        1f);
                }

                // Button size
                RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(DEFAULT_SELECTOR_SIZE.x * _selectorSize.x, incrementsPosY);

            }
        }
    }
}
