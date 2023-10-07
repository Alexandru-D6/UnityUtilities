using System;
using System.Collections.Generic;
using ButtonMenuUI.MenuList;
using UnityEngine;
using UnityEngine.UIElements;

namespace ButtonMenuUI {

    public abstract class ButtonMenuBase : MonoBehaviour {

        [SerializeField] protected Transform _itemPrefab;
        protected Transform _parent;
        protected Camera _camera;

        protected string _selectorId;
        protected List<IMenuItem> _items;

        protected bool _constantScale = true;
        protected bool _destroyOnButtonPressed = true;
        protected bool _destroyOnMouseClick = true;

        protected Vector3 _position;
        protected Vector2 _direction;
        protected Vector2 _selectorSize;

        private bool _initialized = false;

        protected Vector2 DEFAULT_SELECTOR_SIZE = new(100f, 100f);

#region Builder
        public class Builder {
            private Transform _itemPrefab;
            private Transform _parent;
            private Camera _camera;

            private string _name = "";
            private List<IMenuItem> _items;

            private bool _constantScale = true;
            private bool _destroyOnButtonPressed;
            private bool _destroyOnMouseClick = true;

            private Vector3 _position = Vector3.zero;
            private Vector2 _direction = new(1f, -1f);
            private Vector2 _selectorSize = Vector2.zero;

            private EventHandler<ButtonMenuUtils.OnItemPressedArgs> _callback;

            public Builder WithItemPrefab(Transform prefab) {
                _itemPrefab = prefab;
                return this;
            }

            public Builder WithName(string name) {
                _name = name;
                return this;
            }

            public Builder WithItems(List<IMenuItem> items) {
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

            public Builder WithPosition(Vector2 position) {
                _position = position;
                return this;
            }

            public Builder WithDirection(Vector2 direction) {
                _direction = direction;
                return this;
            }

            public Builder WithSelectorSize(Vector2 size) {
                _selectorSize = size;
                return this;
            }

            public Builder WithEvent(EventHandler<ButtonMenuUtils.OnItemPressedArgs> callback) {
                _callback = callback;
                return this;
            }

            public Builder WithConstantScale(bool constantScale) {
                _constantScale = constantScale;
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
                GameObject gameObject = new GameObject(_name, typeof(RectTransform));
                ButtonMenuBase selectorList = gameObject.AddComponent<SelectionMenuList>();

                selectorList._itemPrefab = _itemPrefab;
                selectorList._selectorId = _name;
                selectorList._parent = _parent;
                selectorList._items = _items;
                selectorList._camera = _camera;
                selectorList._direction = _direction;
                selectorList._position = _position;
                selectorList._selectorSize = _selectorSize;
                selectorList.OnItemSelected += _callback;
                selectorList._constantScale = _constantScale;
                selectorList._destroyOnButtonPressed = _destroyOnButtonPressed;
                selectorList._destroyOnMouseClick = _destroyOnMouseClick;

                selectorList.Initialize();
            }
        }

#endregion

#region Callback function

        public event EventHandler<ButtonMenuUtils.OnItemPressedArgs> OnItemSelected;

        public void ButtonPressed(IMenuItem item) {
            OnItemSelected?.Invoke(this, new ButtonMenuUtils.OnItemPressedArgs {
                Item = item
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

                if (_selectorSize == Vector2.zero) {
                    _selectorSize = _parent.GetComponent<RectTransform>().sizeDelta;
                }
            }

            // Config gameObject
            gameObject.name = _selectorId;
            transform.SetParent(_parent, !_constantScale);
            RectTransform rectTransform = transform.GetComponent<RectTransform>();
            rectTransform.sizeDelta  = DEFAULT_SELECTOR_SIZE * _selectorSize;
            rectTransform.pivot      = new Vector2(0, 1);
            rectTransform.anchorMin  = new Vector2(0, 1);
            rectTransform.anchorMax  = new Vector2(0, 1);

            if (_position != Vector3.zero) {
                rectTransform.position = _position;
            } else {
                rectTransform.localPosition = Vector3.zero;
            }

            // Initialize selector
            InitializeButtons();

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
                OnItemSelected?.Invoke(this, new ButtonMenuUtils.OnItemPressedArgs {
                    Item = null
                });

                Destroy(gameObject);
            }
        }

#endregion

#region Private Methods

        protected abstract void InitializeButtons();

#endregion
    }
}
