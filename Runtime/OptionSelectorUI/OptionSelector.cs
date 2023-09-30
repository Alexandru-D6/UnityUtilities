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

        [SerializeField] protected Transform _itemPrefab;

        private bool _initialized = false;

#region Builder
        public class Builder {
            private Transform _itemPrefab;
            private string _name = "";
            private List<TItemObject> _items;
            private Transform _parent;
            private Camera _camera;
            private Vector2 _direction = new(1f, 1f);
            private EventHandler<OptionSelectorUtils.OnItemSelectedArgs> _callback;
            private bool _destroyOnButtonPressed;
            private bool _destroyOnMouseClick = true;

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
                GameObject gameObject = new GameObject().AddComponent<RectTransform>().gameObject;
                OptionSelectorList selectorList = gameObject.AddComponent<OptionSelectorList>();

                selectorList._itemPrefab = _itemPrefab;
                selectorList._selectorId = _name;
                selectorList._parent = _parent;
                selectorList._items = _items as List<ItemSelectorList>;
                selectorList._camera = _camera;
                selectorList._direction = _direction;
                selectorList.OnItemSelected += _callback;

                selectorList.Initialize();
                selectorList._destroyOnButtonPressed = _destroyOnButtonPressed;
                selectorList._destroyOnMouseClick = _destroyOnMouseClick;
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

                Rect parentRect = _parent.GetComponent<RectTransform>().rect;
                _selectorSize = new Vector2(parentRect.width, parentRect.height);
            }

            // Config gameObject
            gameObject.name = _selectorId;
            transform.SetParent(_parent, true);

            // Initialize selector
            InitializeButtons();
            transform.position = Input.mousePosition;

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
