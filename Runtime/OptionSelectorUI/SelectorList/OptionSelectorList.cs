using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;

namespace OptionSelectorUI {

    public class OptionSelectorList : OptionSelector<ItemSelectorList> {

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
                Transform buttonObject = Instantiate(_itemPrefab, _itemsCollection);
                buttonObject.gameObject.name = (item.name + "Button");

                // Click action
                Button button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(() => {
                    ButtonPressed(item.name);
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
                rectTransform.sizeDelta = new Vector2(_selectorSize.x, incrememtsPosY - 0.5f);

                currentPos += new Vector2(0f, signs.y * (incrememtsPosY));
            }
        }
    }
}