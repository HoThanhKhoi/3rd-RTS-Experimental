using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    BuildingTypeSO buildingType;

    private float timerGenerator;
    private float timerConverter;
    private float resourceGeneratorTimerMax;
    private float resourceConverterTimerMax;

    #region Unity Methods
    private void Awake()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        resourceGeneratorTimerMax = buildingType.resourceGeneratorData.timerMax;
        resourceConverterTimerMax = buildingType.resourceConverterData.timerMax;
    }

    private void Start()
    {

    }

    private void Update()
    {
        timerGenerator -= Time.deltaTime;
        if (timerGenerator <= 0f)
        {
            timerGenerator = resourceGeneratorTimerMax;
            ResourceManager.Instance.AddResource(buildingType.resourceGeneratorData.resourceType, 1);
        }

        timerConverter -= Time.deltaTime;
        if (timerConverter <= 0f)
        {
            timerConverter = resourceConverterTimerMax;
            ResourceManager.Instance.ConvertResource(buildingType.resourceConverterData.inputResourceAmount,
                buildingType.resourceConverterData.outputResourceAmount,
                buildingType.resourceConverterData.differentialRate);
        }


    }
    #endregion
}
