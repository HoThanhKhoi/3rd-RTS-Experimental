using System;
using System.Resources;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs>
        OnActiveBuildingTypeChanged;

    public class OnActiveBuildingTypeChangedEventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }

    private Camera mainCamera;
    [SerializeField] private InputReader inputReader;

    private BuildingTypeSO activeBuildingType;
    private BuildingTypeListSO buildingTypeList;

    #region Unity Methods
    private void OnEnable()
    {
        inputReader.MouseRightClickEvent += OnRightMouseClick;
        inputReader.MouseLeftClickEvent += OnLeftMouseClick;
        
    }

    private void Awake()
    {
        Instance = this;

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        SetActiveBuildingType(null);
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (inputReader == null) return;
    }


    #endregion

    #region On Input Read
    private void OnLeftMouseClick()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && activeBuildingType != null)
        {
            if (CanSpawnBuilding(activeBuildingType, Utils.GetMouseWorldPosition(mainCamera, inputReader.MousePosition), out string errorMessage))
            {
                ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                Instantiate(activeBuildingType.prefab,
                            Utils.GetMouseWorldPosition(mainCamera, inputReader.MousePosition),
                            Quaternion.identity);
            }
            else
            {
                TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer { timer = 2f });
            }
        }
    }



    private void OnRightMouseClick()
    {
        SetActiveBuildingType(null);
    }

    #endregion


    #region Functions
    //change active building by index
    private void ChangeCurrentBuildingType(int index)
    {
        activeBuildingType = buildingTypeList.BuildingTypeList[index - 1];
        OnActiveBuildingTypeChanged?.Invoke
        (
            this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType }
        );
    }

    //change active building by building type
    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke
        (
            this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType }
        );
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;

        if (!isAreaClear)
        {
            errorMessage = "Area is not clear!";
            return false;
        }
        else if (!ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
        {
            errorMessage = "Not enough resources!";
            return false;
        }

        errorMessage = "";
        return true;
    }


    #endregion

}

