using System;
using System.Collections.Generic;
using OptionSelectorUI.SelectorList;
using UnityEngine;
using UnityEngine.UIElements;

namespace OptionSelectorUI {

    public abstract class OptionSelector<TItemObject> : MonoBehaviour {

        protected string _selectorId;
        protected List<TItemObject> _items;
        protected Transform _parent;
        protected Vector2 _selectorSize;
        protected Vector2 _direction;
        protected Camera _camera;
        protected bool _destroyOnButtonPressed = true;
        protected bool _destroyOnMouseClick = true;

        [SerializeField] protected Transform _itemsCollection;
        [SerializeField] protected Transform _itemPrefab;

        private bool _initialized = false;

#region Builder
        public class Builder {
            private Transform _selectorPrefab;
            private Transform _itemPrefab;
            private string _name = "";
            private List<TItemObject> _items;
            private Transform _parent;
            private Camera _camera;
            private Vector2 _size = new(175f, 160f);
            private Vector2 _direction = new(1f, 1f);
            private EventHandler<OptionSelectorUtils.OnItemSelectedArgs> _callback;
            private bool _destroyOnButtonPressed;
            private bool _destroyOnMouseClick = true;

            public Builder WithSelectorPrefab(Transform prefab) {
                _selectorPrefab = prefab;
                return this;
            }

            public Builder WithItemPrefab(Transform prefab) {
                _itemPrefab = prefab;
                return this;
            }

            public Builder WithName(string name) {
                _name = name;
                return this;
            }

            public Builder WithItems(List<TItemObject> items) {
                _items = items;
                return this;
            }

            public Builder WithParent(Transform parent) {
                _parent = parent;
                return this;
            }

            public Builder WithCamera(Camera camera) {
                _camera = camera;
                return this;
            }

            public Builder WithSize(Vector2 size) {
                _size = size;
                return this;
            }

            public Builder WithDirection(Vector2 direction) {
                _direction = direction;
                return this;
            }

            public Builder WithEvent(EventHandler<OptionSelectorUtils.OnItemSelectedArgs> callback) {
                _callback = callback;
                return this;
            }

            public Builder WithDestroyOnButtonPressed(bool destroy) {
                _destroyOnButtonPressed = destroy;
                return this;
            }

            public Builder WithDestroyOnMouseClick(bool destroy) {
                _destroyOnMouseClick = destroy;
                return this;
            }

            public void BuildSelectorList() {
                OptionSelectorList result = Instantiate(_selectorPrefab).gameObject.AddComponent<OptionSelectorList>();

                result._itemsCollection = result.transform.Find("ItemsCollection");
                result._itemPrefab = _itemPrefab;
                result._selectorId = _name;
                result._parent = _parent;
                result._items = _items as List<ItemSelectorList>;
                result._camera = _camera;
                result._selectorSize = _size;
                result._direction = _direction;
                result.OnItemSelected += _callback;

                result.Initialize();
                result._destroyOnButtonPressed = _destroyOnButtonPressed;
                result._destroyOnMouseClick = _destroyOnMouseClick;
            }
        }

#endregion

#region Callback function

        public event EventHandler<OptionSelectorUtils.OnItemSelectedArgs> OnItemSelected;

        public void ButtonPressed(int id) {
            OnItemSelected?.Invoke(this, new OptionSelectorUtils.OnItemSelectedArgs {
                Id = id
            });

            if (_destroyOnButtonPressed) {
                Destroy(gameObject);
            }
        }

#endregion

        internal void Initialize() {
            // Handle multiple instances of selector with the same Name
            if (_parent != null) {
                Transform prevSelector = _parent.Find(_selectorId);

                if (prevSelector != null) {
                    Destroy(prevSelector.gameObject);
                }
            }

            // Config gameObject
            gameObject.name = _selectorId;
            transform.SetParent(_parent, true);

            // Initialize selector
            InitializeButtons();
            _itemsCollection.position = Input.mousePosition;

            _initialized = true;
        }

        public void SetDestroyOnButtonPressed(bool destroy) {
            _destroyOnButtonPressed = destroy;
        }

        public void SetDestroyOnMouseClick(bool destroy) {
            _destroyOnMouseClick = destroy;
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
