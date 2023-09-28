using UnityEngine;

[CreateAssetMenu(fileName = "ListItem", menuName = "ScriptableObjects/SelectorUI", order = 1)]
public class ListItemScriptableObject : ScriptableObject {
    public new string name;
    public Sprite sprite;
}
