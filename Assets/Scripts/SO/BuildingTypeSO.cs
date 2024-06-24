using UnityEngine;

[CreateAssetMenu(fileName = "BuildingTypeSO", menuName = "Scriptable Objects/Buildings/BuildingTypeSO")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public Sprite buildingSprite;

    public ResourceGeneratorData resourceGeneratorData;
    public ResourceConverterData resourceConverterData;

    public ResourceAmount[] constructionResourceCostArray;

    public string GetConstructionResourceCostString()
    {
        string str = "";

        foreach (ResourceAmount resourceAmount in constructionResourceCostArray)
        {
            str += "<color=#" + resourceAmount.resourceType.colorHex
                    + ">"
                    + resourceAmount.amount
                    + " "
                    + resourceAmount.resourceType.nameString
                    + "\n"
                    + "</color>"
                ;
        }
        return str;
    }





}
