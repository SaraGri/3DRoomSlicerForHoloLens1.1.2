using System;
using System.Collections.Generic;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;
using UnityEditor;

public class ScanManager : MonoBehaviour, IInputClickHandler
{
    //public TextMesh InstructionTextMesh;
    public Transform FloorPrefab;
    public TestExporter exporter;

    public GameObject hololensCamera;
    public GameObject defaultCursor;
    private AnimatedCursor animatedCurser;

    public GameObject spatialMapping;
    private SpatialMappingManager spatialMappingManager;

    public GameObject ui;
    private UIController uiController;

    private bool spatialUnderstaingIsActive;
    private bool scanStateCallbackIsAttached;

    // Use this for initialization
    void Start()
    {
        //SpatialUnderstanding.Instance
        // Check if spatial understanding is active
        spatialUnderstaingIsActive = false;

        // Only add callback once
        scanStateCallbackIsAttached = false;

        // Add InputHandler
        InputManager.Instance.PushFallbackInputHandler(this.gameObject);

        // Init ref to spatial mapping
        spatialMappingManager = spatialMapping.GetComponent<SpatialMappingManager>();

        // Init ref to UIController
        uiController = ui.GetComponent<UIController>();

        // Init ref to curser (state)
        animatedCurser = defaultCursor.GetComponent<AnimatedCursor>();

        // Auto start scan
        if (SpatialUnderstanding.Instance.AutoBeginScanning)
        {
            //SpatialUnderstanding.Instance.RequestBeginScanning();
            //SpatialUnderstanding.Instance.ScanStateChanged += ScanStateChanged;
            StartScan();
        }
    }

    // Callback which is executed when scan state has changed (EventHandling 
    // via event Action -> without event data)
    private void ScanStateChanged()
    {
        if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning)
        {
            //LogSurfaceState();
        }
        else if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Done)
        {
            //LogSurfaceState();
            //exporter.Export("test.obj"); 
            // InstanciateObjectOnFloor();
        }
    }

    private void OnDestroy()
    {
        // Remove the scan state changed callback
        if(scanStateCallbackIsAttached)
            SpatialUnderstanding.Instance.ScanStateChanged -= ScanStateChanged;
    }

    // Start scanning with spartial understanding
    public void StartScan()
    {
        //this.InstructionTextMesh.text = "Requested Start Scan";
        SpatialUnderstanding.Instance.RequestBeginScanning();
        spatialUnderstaingIsActive = true;

        // Since the state has changed from none to readyToScan (leads to state 
        // scanning in update  of SpatialUnderstanding.cs) we add an callback 
        // which handle the states within the scanManager (process only one time)
        if (!scanStateCallbackIsAttached)
        {
            SpatialUnderstanding.Instance.ScanStateChanged += ScanStateChanged;
            scanStateCallbackIsAttached = true;
        }
    }

    // Finish/Stop the scanning with spatial understanding
    public void FinishScan()
    {
        //this.InstructionTextMesh.text = "Requested Finish Scan";

        if (spatialUnderstaingIsActive)
        {
            SpatialUnderstanding.Instance.RequestFinishScan();
            spatialUnderstaingIsActive = false;
        }
    }

    // Export spatial understanding mesh only
    public void SaveScanSU()
    {
        //this.InstructionTextMesh.text = "Requested Finish Scan";

        string filename = "SU_d_" + DateTime.Now.ToString("d_M_yyyy") + 
                          "_t_" + DateTime.Now.ToString("HH_mm_ss") + ".obj";

        exporter.Export
        (
            // Formated filename 
            filename,
            // Spatial Understanding mesh
            SpatialUnderstanding.Instance.UnderstandingCustomMesh.GetMeshFilters().ToArray(),
            // Spatial Mapping mesh
            null
        );
    }

    // Export spatial mapping mesh only
    public void SaveScanSM()
    {
        //this.InstructionTextMesh.text = "Requested Finish Scan";
        string filename = "SM_d_" + DateTime.Now.ToString("d_M_yyyy") +
                  "_t_" + DateTime.Now.ToString("HH_mm_ss") + ".obj";

        exporter.Export
        (
            // Formated filename
            filename,
            // Spatial Understanding mesh
            null,
            // Spatial Mapping mesh
            spatialMappingManager.GetMeshFilters().ToArray()
        );
    }

    // Export spatial mapping and spatial understanding mesh
    public void SaveScanBoth()
    {
        //this.InstructionTextMesh.text = "Requested Finish Scan";

        string filename = "SMSU_d_" + DateTime.Now.ToString("d_M_yyyy") +
                  "_t_" + DateTime.Now.ToString("HH_mm_ss") + ".obj";

        exporter.Export
        (
            // Formated filename
            filename,
            // Spatial Understanding mesh
            SpatialUnderstanding.Instance.UnderstandingCustomMesh.GetMeshFilters().ToArray(),
            // Spatial Mapping mesh
            spatialMappingManager.GetMeshFilters().ToArray()
        );
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        //this.InstructionTextMesh.text = "Requested Finish Scan"; 
    }

    // Manage scan state update cycle
    void Update()
    {
        // Handle scan state
        switch (SpatialUnderstanding.Instance.ScanState)
        {
            case SpatialUnderstanding.ScanStates.None:
                //this.InstructionTextMesh.text = "State: None";
                break;
            case SpatialUnderstanding.ScanStates.ReadyToScan:
                //this.InstructionTextMesh.text = "State: Ready to Scan";
                break;
            case SpatialUnderstanding.ScanStates.Scanning:
                //this.LogSurfaceState();
                break;
            case SpatialUnderstanding.ScanStates.Finishing:
                //this.InstructionTextMesh.text = "State: Finishing Scan";
                break;
            case SpatialUnderstanding.ScanStates.Done:
                //this.InstructionTextMesh.text = "State: Scan DONE";
                break;
            default:
                break;
        }
    }

    private void LogSurfaceState()
    {
        IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
        if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) != 0)
        {
            var stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();
            //this.InstructionTextMesh.text = "Scanning " + string.Format("TotalSurfaceArea: {0:0.##} - WallSurfaceArea: {1:0.##} - HorizSurfaceArea: {2:0.##}", stats.TotalSurfaceArea, stats.WallSurfaceArea, stats.HorizSurfaceArea);
            //this.InstructionTextMesh.text = "Position " + string.Format("x: {0:0.##} - y: {1:0.##} - z: {2:0.##}", defaultCursor.transform.position.x, defaultCursor.transform.position.y, defaultCursor.transform.position.z);
            float dist = Vector3.Magnitude(defaultCursor.transform.position - hololensCamera.transform.position);
            //this.InstructionTextMesh.text = "Distance " + string.Format("{0:0.####}", dist);
        
            //var rayResult = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticRaycastResult();
            //this.InstructionTextMesh.text = rayResult.SurfaceType.ToString();
        }
    }

    private void InstanciateObjectOnFloor()
    {
        const int QueryResultMaxCount = 512;

        SpatialUnderstandingDllTopology.TopologyResult[] _resultsTopology = new SpatialUnderstandingDllTopology.TopologyResult[QueryResultMaxCount];

        var minLengthFloorSpace = 0.25f;
        var minWidthFloorSpace = 0.25f;

        var resultsTopologyPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(_resultsTopology);
        var locationCount = SpatialUnderstandingDllTopology.QueryTopology_FindPositionsOnFloor(minLengthFloorSpace, minWidthFloorSpace, _resultsTopology.Length, resultsTopologyPtr);

        if (locationCount > 0)
        {
            Instantiate(this.FloorPrefab, _resultsTopology[0].position, Quaternion.LookRotation(_resultsTopology[0].normal, Vector3.up));

            //this.InstructionTextMesh.text = "Placed the hologram";
        }
        else
        {
            //this.InstructionTextMesh.text = "I can't found enough space to place the hologram.";
        }
    }
}