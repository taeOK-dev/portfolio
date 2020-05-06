using UnityEngine;
using System;
using System.Collections;
using Celestials;
using Service.WarFront;

public class WarFrontUI : PanelBase
{
    public bool debugOpen = true;

    public WarFront_CellIcon cellIconPrefab = null;
    public WarFront_CellEffect cellEffect = null;

    public virtual void OnSelectHexCell(HexCell cell) { }
    public virtual void BackStep() { }

   // [HideInInspector]
    public HexCell select_HexCell = null;
    
    [HideInInspector]
    public HexCell prev_HexCell = null;

    public int select_HexCell_index
    {
        get
        {
            return (select_HexCell == null) ? -1 : select_HexCell.index;
        }
    }
    public int prev_HexCell_index
    {
        get
        {
            return (prev_HexCell == null) ? -1 : prev_HexCell.index;
        }
    }


    protected GameObject cellIconRoot = null;
    private Camera uiCamera = null;

    //private string warFrontPrefabName = string.Empty;

    Panel_WarFront.Data data_;

    protected override void OnOpenUI(object data, System.Action<bool> actionCallback = null)
    {
        data_ = data as Panel_WarFront.Data;

        //if(warFrontPrefabName != _data.warFrontPrefabName)
        {
            if (cellIconRoot != null)
                DestroyImmediate(cellIconRoot);
        }

        CreateWarFront_CellIcon_Root();
    }

    void CreateWarFront_CellIcon_Root()
    {
        cellIconRoot = new GameObject("WarFront_CellIcon_Root");
        cellIconRoot.transform.parent = this.transform;
        cellIconRoot.gameObject.SetActive(HexGrid.IsToolMode);
    }

    public void SelectHexCell(HexCell select_HexCell)
    {

        switch (select_HexCell._type)
        {
            case HexCell.CellType.Free:
            case HexCell.CellType.None:
                if (UIManager.Instance.GetTopPopUP() != UI_PANEL.KeyPointPopup ||
                  UIManager.Instance.GetTopPopUP() != UI_PANEL.EntrancePopUp)
                {
                    this.select_HexCell = null;// ????
                    this.BackStep();
                }
                break;

            case HexCell.CellType.QuestPoint:
            case HexCell.CellType.FreeQuestPoint:
                if(select_HexCell.isShowQuest == true)
                {
                    this.select_HexCell = select_HexCell;
                    OnSelectHexCell(select_HexCell);
                }
                else
                {
                    this.select_HexCell = null;// ????
                    this.BackStep();
                }
                break;

            case HexCell.CellType.DungeonPoint:
                {
                    this.select_HexCell = select_HexCell;
                    OnSelectHexCell(select_HexCell);
                }
                break;


            default:
                if(this.prev_HexCell != null)
                {
                    this.prev_HexCell = this.select_HexCell;
                }
                this.select_HexCell = select_HexCell;
                OnSelectHexCell(select_HexCell);
                break;
        }
    }

    public WarFront_CellIcon InstantiateCellIcon(HexCell cell)
    {
        if (cellIconPrefab == null)
            return null;

        if(uiCamera == null)
        {
            GameObject camera = GameObject.Find("UICam");
            if (camera != null)
                uiCamera = camera.GetComponent<Camera>();
        }

        if (cellIconRoot == null)
        {
            CreateWarFront_CellIcon_Root();
        }

        WarFront_CellIcon icon = Instantiate<WarFront_CellIcon>(cellIconPrefab);

        icon.Initialize(cell, uiCamera);
        icon.transform.SetParent(this.transform);
        icon.transform.localScale = new Vector3(1f, 1f, 1f);
        icon.transform.parent = cellIconRoot.transform;

        return icon;
    }

    public void InstantiateCellEffect(HexCell cell)
    {
        if (cellEffect == null)
        {
            return;
        }

        WarFront_CellEffect tempEffect = Instantiate<WarFront_CellEffect>(cellEffect);
        tempEffect.Initialize(cell);
        tempEffect.transform.SetParent(this.transform);
        tempEffect.transform.localScale = new Vector3(1f, 1f, 1f);
    }

}



