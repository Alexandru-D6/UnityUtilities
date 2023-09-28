using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;

namespace OptionSelectorUI.SelectorList {

    public class OptionSelectorList : OptionSelector<ItemSelectorList> {

        [SerializeField] private Transform _itemPrefab_OnlyImage;
        [SerializeField] private Transform _itemPrefab_OnlyName;

        protected override void InitializeButtons() {
            float incrememtsPosY = Mathf.Floor(_selectorSize.y / _items.Count);
            Vector2 currentPos;
            Vector2 signs = new Vector2(
                ((Input.mousePosition.x + _selectorSize.x > Camera.main.pixelWidth) ? -1f : 1f),
                ((Input.mousePosition.y - (_items.Count * (incrememtsPosY + 0.5f))  > 0f) ? -1f : 1f)
            );

            currentPos = new Vector2(
                (signs.x < 0f) ? -1f * _selectorSize.x : 0f,
                (signs.y < 0f) ? 0f : incrememtsPosY
            );

            _itemsCollection.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, signs.y > 0f ? 0f : 1f);

            foreach (var item in _items) {
                Assert.IsFalse(item.name == "" && item.sprite == null);

                Transform buttonObject;

                if (item.name != "" && item.sprite != null) {
                    buttonObject = Instantiate(_itemPrefab, _itemsCollection);
                } else if (item.name == "") {
                    buttonObject = Instantiate(_itemPrefab_OnlyImage, _itemsCollection);
                } else {
                    buttonObject = Instantiate(_itemPrefab_OnlyName, _itemsCollection);
                }

                buttonObject.gameObject.name = (item.name == "" ? item.name : item.sprite.name) + "Button";

                // Click action
                Button button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(() => {
                    ButtonPressed(item.id);
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
                textObject.text = item.name;

                // GameObject position
                Vector3 backupPos = buttonObject.localPosition;
                buttonObject.localPosition = new Vector3(currentPos.x, currentPos.y, backupPos.z);

                // Button size
                RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();

                if (item.name == "") {
                    Debug.Log(item.sprite.bounds);
                    rectTransform.sizeDelta = new Vector2(_selectorSize.x, incrememtsPosY - 0.5f);
                }
                rectTransform.sizeDelta = new Vector2(_selectorSize.x, incrememtsPosY - 0.5f);

                currentPos += new Vector2(0f, signs.y * (incrememtsPosY));
            }
        }
    }
}
