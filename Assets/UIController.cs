using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour {//, IPointerEnterHandler, IPointerExitHandler {
    // Ref to spatial mapping
    public GameObject spatialMapping;
    private SpatialMappingManager spatialMappingManager;

    // Ref to spatial understanding
    public GameObject spatialUnderstanding;
    private SpatialUnderstandingCustomMesh spatialUnderstandingCustomMesh;

    // Ref to scan manager (management of spatial understanding)
    public GameObject mappingOrchestrator;
    private ScanManager scanManager;

    // UI-Elements with values (exept buttons)
    public Dropdown dropDownSpatialUnderstandingScanState;
    public Dropdown dropDownSliceGeometry;

    public Toggle toggleSlice;
    public bool isSlicingEnabled = false;

    public bool isAnyUIObjectInFocus = false;

    public enum SelectedSliceGeometry { Triangle, Rectangle, Circle, Polygon};
    public SelectedSliceGeometry selectedSliceGeometry; 

    // Use this for initialization
    void Start() {
        // Init ref to spatial mapping
        spatialMappingManager = spatialMapping.GetComponent<SpatialMappingManager>();

        // Init ref to spatial understanding
        spatialUnderstandingCustomMesh = spatialUnderstanding.GetComponent<SpatialUnderstandingCustomMesh>();

        // Init ref to scan manager (management of spatial understanding)
        scanManager = mappingOrchestrator.GetComponent<ScanManager>();

        selectedSliceGeometry = SelectedSliceGeometry.Triangle;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(isAnyUIObjectInFocus);
	}

    bool drawSpatialMapping = true; 

    public void HandleSpatialMapping () {
        //Debug.Log(eventData.selectedObject);
        Debug.Log("FOOO");
        if (drawSpatialMapping) {
            spatialMappingManager.DrawVisualMeshes = true;
            drawSpatialMapping = false;
        } else {
            spatialMappingManager.DrawVisualMeshes = false;
            drawSpatialMapping = true;
        }
    }

    bool drawSpatialUnderstanding = true;

    public void HandleSpatialUnderstanding () {

        if (drawSpatialUnderstanding) {
            spatialUnderstandingCustomMesh.DrawProcessedMesh = true;
            drawSpatialUnderstanding = false;
        } else {
            spatialUnderstandingCustomMesh.DrawProcessedMesh = false;
            drawSpatialUnderstanding = true;
        }
    }

    public void HandleSpartialUnderstandingScanState()
    {
        // Index:
        // 0 = Start Spatial Understanding
        // 1 = Stop  Spatial Understanding
        // 2 = Stop  Spatial Understanding and save mesh as obj file on the device.
        int currentDropDownIndex = dropDownSpatialUnderstandingScanState.value;

        switch (currentDropDownIndex)
        {
            case 0: // NONE
                Debug.Log("ReadyToScan");
                Debug.Log(currentDropDownIndex);
                break;

            case 1: // Start Spatial Understanding
                Debug.Log("Start Scan");
                Debug.Log(currentDropDownIndex);
                scanManager.StartScan();
                break;

            case 2: // Finish Spatial Understanding
                Debug.Log("Finish Scan");
                Debug.Log(currentDropDownIndex);
                scanManager.FinishScan();
                break;

            case 3: // Export spatial understanding mesh to obj
                scanManager.SaveScanSU();
                Debug.Log("Exported Spatial Understanding mesh");
                Debug.Log(currentDropDownIndex);
                break;


            case 4: // Export spatial mapping mesh to obj
                scanManager.SaveScanSM();
                Debug.Log("Exported Spatial Mapping mesh");
                Debug.Log(currentDropDownIndex);
                break;


            case 5: // Export spatial understanding and spatial mapping mesh to obj
                scanManager.SaveScanBoth();
                Debug.Log("Exported complete mesh");
                Debug.Log(currentDropDownIndex);
                break;

            default:
                currentDropDownIndex = 0; 
                break;
        }
    }

    public void HandleSliceGeometry()
    {
        // Index:
        // 0 = Triangle
        // 1 = Rectangle
        // 2 = Circle
        // 3 = Polygon
        int currentDropDownIndex = dropDownSliceGeometry.value;

        switch (currentDropDownIndex)
        {
            case 0: // NONE
             //   Debug.Log("Triangle");
             //   Debug.Log(currentDropDownIndex);
                selectedSliceGeometry = SelectedSliceGeometry.Triangle;
                break;

            case 1: // Triangle
              //  Debug.Log("Rectangle");
          //      Debug.Log(currentDropDownIndex);
                selectedSliceGeometry = SelectedSliceGeometry.Rectangle;
                break;

            case 2: // Circle
              //  Debug.Log("Circle");
            //    Debug.Log(currentDropDownIndex);
                selectedSliceGeometry = SelectedSliceGeometry.Circle;
                break;

            case 3: // Polygon
             //   Debug.Log("Polygon");
              //  Debug.Log(currentDropDownIndex);
                selectedSliceGeometry = SelectedSliceGeometry.Polygon;
                break;

            default:
                currentDropDownIndex = 0;
                break;
        }
    }

    public void HandleToggleSlice()
    {
        if (toggleSlice.isOn)
            isSlicingEnabled = true;
        else
            isSlicingEnabled = false;
    }
}
