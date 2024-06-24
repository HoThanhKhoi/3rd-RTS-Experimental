using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    [SerializeField] private InputReader inputReader;
    [SerializeField] private RectTransform canvasRectTransform;

    private TextMeshProUGUI textMeshPro;
    private RectTransform bgRectTransform;
    private RectTransform rectTransform;
    private Vector2 tooltipOffset;
    private Vector2 anchoredPosition;
    private TooltipTimer tooltipTimer;


    #region Unity Methods
    private void Awake()
    {
        Instance = this;

        rectTransform = GetComponent<RectTransform>();
        textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        bgRectTransform = transform.Find("background").GetComponent<RectTransform>();

        tooltipOffset = new Vector2(10, 5);

        Hide();
    }

    private void Update()
    {
        rectTransform.anchoredPosition = (inputReader.MousePosition / canvasRectTransform.localScale.x) + tooltipOffset;

        SetTooltipPosition();

        if (tooltipTimer != null)
        {
            tooltipTimer.timer -= Time.deltaTime;
            if (tooltipTimer.timer <= 0)
            {
                Hide();
            }
        }
    }
    #endregion

    private void SetTooltipPosition()
    {
        anchoredPosition = (inputReader.MousePosition / canvasRectTransform.localScale.x) + tooltipOffset;

        if (anchoredPosition.x + bgRectTransform.rect.width > Screen.width)
        {
            anchoredPosition.x = Screen.width - bgRectTransform.rect.width;
        }

        if (anchoredPosition.y + bgRectTransform.rect.height > Screen.height)
        {
            anchoredPosition.y = Screen.height - bgRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string toolTipText)
    {
        textMeshPro.text = toolTipText;
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(10, 10);
        bgRectTransform.sizeDelta = textSize + padding;
    }

    public void Show(string toolTipText, TooltipTimer tooltipTimer = null)
    {
        this.tooltipTimer = tooltipTimer;
        gameObject.SetActive(true);
        SetText(toolTipText);
        SetTooltipPosition();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public class TooltipTimer
    {
        public float timer;
    }
}
