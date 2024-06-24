using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private Camera mainCamera;
    private GameObject spriteGameObject;

    #region Unity Methods
    private void Awake()
    {
        spriteGameObject = transform.Find("sprite").gameObject;
        Hide();
    }

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }
    private void Update()
    {
        transform.position = Utils.GetMouseWorldPosition(mainCamera, inputReader.MousePosition);
    }

    #endregion

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        if (e.activeBuildingType == null)
        {
            Hide();
        }
        else
        {
            Show(e.activeBuildingType.buildingSprite);
        }
    }


    public void Show(Sprite ghostSprite)
    {
        spriteGameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    public void Hide()
    {
        spriteGameObject.SetActive(false);
    }
}
