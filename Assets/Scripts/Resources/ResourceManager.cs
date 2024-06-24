using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;


    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;


    private ResourceTypeListSO resourceTypeList;

    #region Unity Methods
    private void Awake()
    {
        Instance = this;
        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSO resourceType in resourceTypeList.ResourceTypeList)
        {
            resourceAmountDictionary[resourceType] = 0;
        }

        resourceAmountDictionary[resourceTypeList.ResourceTypeList[0]] = 10000;
    }

    private void Update()
    {
        
    }

    #endregion

    private void TestLogResourceAmountDictionary()
    {
        foreach (ResourceTypeSO resourceType in resourceAmountDictionary.Keys)
        {
            //Debug.Log(resourceType.nameString + ": " + resourceAmountDictionary[resourceType]);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        if(resourceType == null)
        {
            return;
        }
            
        resourceAmountDictionary[resourceType] += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ConvertResource(ResourceAmount inputResourceAmount, ResourceAmount outputResourceAmount, int differentialRate)
    {
        if(inputResourceAmount == null)
        {
            return;
        }

        inputResourceAmount.amount = outputResourceAmount.amount * differentialRate;

        

        if (CanAfford(inputResourceAmount))
        {
            AddResource(outputResourceAmount.resourceType, outputResourceAmount.amount);
            SpendResources(new ResourceAmount[] { inputResourceAmount });
        }
            

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] resourceCostArray)
    {
        foreach (ResourceAmount resourceCost in resourceCostArray)
        {
            if (GetResourceAmount(resourceCost.resourceType) < resourceCost.amount)
            {
                return false;
            }
        }
        return true;
    }

    public void SpendResources(ResourceAmount[] resourceCostArray)
    {
        foreach (ResourceAmount resourceCost in resourceCostArray)
        {
            resourceAmountDictionary[resourceCost.resourceType] -= resourceCost.amount;
        }
    }

    
}
