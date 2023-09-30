using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace OptionSelectorUI {

    public abstract class OptionSelector<TItemObject, TItemType> : MonoBehaviour {

        protected List<TItemObject> _items;
        protected Vector2 _selectorSize;
        protected Vector2 _direction;
        protected Camera _camera;
        protected bool _destroyOnButtonPressed = true;
        protected bool _destroyOnMouseClick = true;

        [SerializeField] protected Transform _itemsCollection;
        [SerializeField] protected Transform _itemPrefab;

        private bool _initialized = false;

#region Callback function

        public event EventHandler<OptionSelectorUtils.OnItemSelectedArgs<TItemType>> OnItemSelected;

        public void ButtonPressed(TItemType id) {
            OnItemSelected?.Invoke(this, new OptionSelectorUtils.OnItemSelectedArgs<TItemType> {
                Id = id
            });

            if (_destroyOnButtonPressed) {
                Destroy(gameObject);
            }
        }

#endregion

        public void Initialize(string selectorId, List<TItemObject> items, Transform canvasParent, Camera cam, Vector2 selectorSize, Vector2 direction) {
            // Handle multiple instances of selector with the same Name
            Transform prevSelector = canvasParent.Find(selectorId);

            if (prevSelector != null) {
                Destroy(prevSelector.gameObject);
            }

            // Saving variables
            _items = items;
            _selectorSize = selectorSize;
            _direction = direction;
            _camera = cam;

            // Config gameObject
            gameObject.name = selectorId;
            transform.SetParent(canvasParent, true);

            // Initialize selector
            InitializeButtons();
            _itemsCollection.position = Input.mousePosition;

            _initialized = true;
        }

        public void SetDestroyOnButtonPressed(bool value) {
            _destroyOnButtonPressed = value;
        }

        public void SetDestroyOnMouseClick(bool value) {
            _destroyOnMouseClick = value;
        }

#region Monobehaviour methods

        private void Update() {
            if (_initialized && _destroyOnMouseClick && Input.GetMouseButtonUp((int) MouseButton.LeftMouse)) {
                Destroy(gameObject);
            }
        }

#endregion

#region Private Methods

        protected abstract void InitializeButtons();

#endregion
    }
}
