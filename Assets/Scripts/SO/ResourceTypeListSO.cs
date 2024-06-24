using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceTypeListSO", menuName = "Scriptable Objects/Resources/ResourceTypeListSO")]
public class ResourceTypeListSO : ScriptableObject
{
    public List<ResourceTypeSO> ResourceTypeList;
}
