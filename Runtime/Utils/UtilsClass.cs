using UnityEngine;

namespace Utils {
    public class UtilsClass {

        public static TextMesh CreateWorldText(string text, Vector3 localPosition, Vector3 localDirection, Color color, Transform parent = null, int fontSize = 60, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAligment = TextAlignment.Center, int sortingOrder = 0) {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localEulerAngles = new Vector3(90 * localDirection.x, 90 * localDirection.y, 90 * localDirection.z);
            transform.localScale = new Vector3(0.1f,0.1f,0.1f);

            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAligment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMesh;
        }

        public static Vector3 Vector3ToVector3Floor(Vector3 value) {
            return new Vector3(Mathf.FloorToInt(value.x),Mathf.FloorToInt(value.y),Mathf.FloorToInt(value.z));
        }

        public static Vector3Int Vector3ToVector3Int(Vector3 value) {
            return new Vector3Int(Mathf.FloorToInt(value.x),Mathf.FloorToInt(value.y),Mathf.FloorToInt(value.z));
        }

        public static Vector3 GetMouseWorldPosition() {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.y = 0.0f;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ() {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
            return worldCamera.ScreenToWorldPoint(screenPosition);
        }

        public static Vector3 GetMouseWorldPositionOverPlane(Camera camera) {
            Plane plane = new(new Vector3(0.0f,1.0f,0.0f), new Vector3(0.0f,0.0f,0.0f));

            return GetMouseWorldPositionOverPlane(camera, plane);
        }

        public static Vector3 GetMouseWorldPositionOverPlane(Camera camera, Plane plane) {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePos);

            float enter;
            if (plane.Raycast(ray, out enter)) return ray.GetPoint(enter);

            return Vector3.zero;
        }
    }
}
