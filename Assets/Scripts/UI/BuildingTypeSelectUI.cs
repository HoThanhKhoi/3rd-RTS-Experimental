using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;
    [SerializeField] private float offsetAmount = -150f;

    private Dictionary<BuildingTypeSO, Transform> btnTransformDictionary;

    Transform btnTransform;

    #region Unity Methods
    private void Awake()
    {
        Transform btnTemplate = transform.Find("btnTemplate");

        btnTemplate.gameObject.SetActive(false);

        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        int index = 0;
        foreach (BuildingTypeSO buildingType in buildingTypeList.BuildingTypeList)
        {
            if (ignoreBuildingTypeList.Contains(buildingType))
            {
                continue;
            }

            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0f);

            btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.buildingSprite;


            btnTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });

            var eventTrigger = btnTransform.GetComponent<Button>().gameObject.AddComponent<EventTrigger>();


            var entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entryEnter.callback.AddListener((eventData) => {
                TooltipUI.Instance.Show(buildingType.nameString + "\n" + buildingType.GetConstructionResourceCostString());
            });
            eventTrigger.triggers.Add(entryEnter);


            var exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            exitEntry.callback.AddListener((eventData) => {
                TooltipUI.Instance.Hide();
            });
            eventTrigger.triggers.Add(exitEntry);

            btnTransformDictionary[buildingType] = btnTransform;

            index++;
        }
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        UpdateActiveBuildingTypeButton();
    }

    #endregion

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        UpdateActiveBuildingTypeButton();
    }

    private void UpdateActiveBuildingTypeButton()
    {
        foreach (BuildingTypeSO buildingType in btnTransformDictionary.Keys)
        {
            btnTransform = btnTransformDictionary[buildingType];
            btnTransform.Find("selected").gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();

        if (activeBuildingType != null)
        {
            btnTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
        }
    }
}
