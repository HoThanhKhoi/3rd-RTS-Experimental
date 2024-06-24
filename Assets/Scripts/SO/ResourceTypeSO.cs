using UnityEngine;

[CreateAssetMenu(fileName = "ResourceTypeSO", menuName = "Scriptable Objects/Resources/ResourceTypeSO")]
public class ResourceTypeSO : ScriptableObject
{
    public string nameString;
    public string shortNameString;
    public Sprite resourceImage;
    public string colorHex;
}
