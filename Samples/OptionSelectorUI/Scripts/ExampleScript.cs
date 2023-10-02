using System.Collections.Generic;
using ButtonMenuUI;
using ButtonMenuUI.MenuList;
using UnityEngine;
using UnityEngine.UIElements;

namespace Samples.OptionSelectorUI.Scripts {

    public class MovementInfo : IMenuItem {
        public readonly string MovementType;
        public readonly string TargetType;
        public readonly string TargetPiece;
        public readonly string TargetPosition;

        public int Id { get; set; }
        public string Name { get; set; }
        public Sprite Sprite { get; set; }

        public MovementInfo(string movementType, string targetType, string targetPiece, string targetPosition) {
            this.MovementType = movementType;
            this.TargetType = targetType;
            this.TargetPiece = targetPiece;
            this.TargetPosition = targetPosition;
        }

        public override string ToString() {
            return $"MovementType: {MovementType}, TargetType: {TargetType}, TargetPiece: {TargetPiece}, TargetPosition: {TargetPosition}";
        }
    }

    public struct MovementInfoStruct : IMenuItem {
        public readonly string MovementType;
        public readonly string TargetType;
        public readonly string TargetPiece;
        public readonly string TargetPosition;

        public int Id { get; set; }
        public string Name { get; set; }
        public Sprite Sprite { get; set; }

        public MovementInfoStruct(string movementType, string targetType, string targetPiece, string targetPosition) {
            MovementType = movementType;
            TargetType = targetType;
            TargetPiece = targetPiece;
            TargetPosition = targetPosition;
            Id = 0;
            Name = "";
            Sprite = null;
        }

        public override string ToString() {
            return $"MovementType: {MovementType}, TargetType: {TargetType}, TargetPiece: {TargetPiece}, TargetPosition: {TargetPosition}";
        }
    }

    public class ExampleScript : MonoBehaviour {

        [SerializeField] private Transform _animalSelectorList;
        [SerializeField] private Transform _buttonUIPrefab;

        [SerializeField] private List<Sprite> _sprites;

        private Camera _camera;
        void Start() {
            _camera = Camera.main;
        }

        void Update() {
            if (Input.GetMouseButtonDown((int) MouseButton.RightMouse)) {
                InstantiateSelectorList();
            }
        }

        private void InstantiateSelectorList() {
            List<MovementInfoStruct> movementInfos = new List<MovementInfoStruct> {
                new("Move", "Piece", "Elephant", "A1"),
                new("Attack", "Piece", "Giraffe", "A2"),
                new("Promotion", "Piece", "Pig", "A3"),
                new("Castling", "Piece", "Monkey", "A4")
            };

            List<IMenuItem> items = new List<IMenuItem>();

            foreach (MovementInfoStruct movementInfo in movementInfos) {
                MovementInfoStruct tmp = movementInfo;
                tmp.Id = movementInfos.IndexOf(movementInfo);
                tmp.Name = movementInfo.MovementType;
                tmp.Sprite = _sprites[Random.Range(0, _sprites.Count)];

                items.Add(tmp);
            }

            new ButtonMenuBase.Builder()
                .WithParent(_animalSelectorList)
                .WithItemPrefab(_buttonUIPrefab)
                .WithName("Animals Selector")
                .WithItems(items)
                .WithCamera(_camera)
                .WithDirection(new Vector2(1f, -1f))
                .WithPosition(Input.mousePosition)
                .WithSelectorSize(new Vector2(200f, 200f))
                .WithEvent(OnPieceSelected)
                .BuildSelectorList();
        }

        private void OnPieceSelected(object sender, ButtonMenuUtils.OnItemPressedArgs e) {
            if (e.Item is null) {
                Debug.Log("No item selected");
            }else {
                Debug.Log("Selected piece: " + ((MovementInfoStruct)e.Item));
            }
        }
    }
}
