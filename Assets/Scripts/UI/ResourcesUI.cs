using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class ResourceUI : MonoBehaviour
{
    private ResourceTypeListSO resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;

    #region Unity Methods

    private void Awake()
    {
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();


        Transform resourceTemplate = transform.Find("ResourceTemplate");
        resourceTemplate.gameObject.SetActive(false);

        int index = 1;
        float offsetAmount = -100f;

        foreach (ResourceTypeSO resourceType in resourceTypeList.ResourceTypeList)
        {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);

            resourceTransform.gameObject.SetActive(true);

            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, offsetAmount * index);

            resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.resourceImage;

            resourceTypeTransformDictionary[resourceType] = resourceTransform;

            index++;
        }
    }

    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
        UpdateResourceAmount();
    }

    #endregion

    private void ResourceManager_OnResourceAmountChanged(object sender, EventArgs e)
    {
        UpdateResourceAmount();
    }

    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeList.ResourceTypeList)
        {
            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);

            Transform resourceTransform = resourceTypeTransformDictionary[resourceType];

            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }
    }
}
