using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace OptionSelectorUI {

    public abstract class OptionSelector<T> : MonoBehaviour {

        protected List<T> _items;
        protected Vector2 _selectorSize;
        protected Vector2 _direction;
        protected bool _destroyOnButtonPressed = true;
        protected bool _destroyOnMouseClick = true;

        [SerializeField] protected Transform _itemsCollection;
        [SerializeField] protected Transform _itemPrefab;

#region Callback function

        public event EventHandler<OptionSelectorUtils.OnItemSelectedArgs> OnItemSelected;

        public void ButtonPressed(string id) {
            OnItemSelected?.Invoke(this, new OptionSelectorUtils.OnItemSelectedArgs {
                id = id
            });

            if (_destroyOnButtonPressed) {
                Destroy(gameObject);
            }
        }

#endregion

        public void Initialize(string selectorId, List<T> items, Transform canvasParent, Vector2 selectorSize, Vector2 direction) {
            // Handle multiple instances of selector with the same Name
            Transform prevSelector = canvasParent.Find(selectorId);

            if (prevSelector != null) {
                Destroy(prevSelector.gameObject);
            }

            // Saving variables
            _items = items;
            _selectorSize = selectorSize;
            _direction = direction;

            // Config gameObject
            gameObject.name = selectorId;
            transform.SetParent(canvasParent, true);

            // Initialize selector
            InitializeButtons();
            _itemsCollection.position = Input.mousePosition;

            gameObject.SetActive(true);
        }

        public void SetDestroyOnButtonPressed(bool value) {
            _destroyOnButtonPressed = value;
        }

        public void SetDestroyOnMouseClick(bool value) {
            _destroyOnMouseClick = value;
        }

#region Monobehaviour methods

        private void Start() {
            gameObject.SetActive(false);
        }

        private void Update() {
            if (_destroyOnMouseClick && Input.GetMouseButtonUp((int) MouseButton.LeftMouse)) {
                Destroy(gameObject);
            }
        }

#endregion

#region Private Methods

        protected abstract void InitializeButtons();

#endregion
    }
}
