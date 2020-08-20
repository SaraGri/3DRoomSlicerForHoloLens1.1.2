using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity;

public class CamUIController : MonoBehaviour {

    public Text scanStateText;
    public Text distanceText;

    public GameObject spatialUnderstandingGO;
    private SpatialUnderstanding spatialUnderstanding;

    public GameObject hololensCamera;
    public GameObject defaultCursor;

    // Use this for initialization
    void Start () {
        spatialUnderstanding = spatialUnderstandingGO.GetComponent<SpatialUnderstanding>();
	}
	
	// Update is called once per frame
	void Update () {
        scanStateText.text = "ScanState: " + spatialUnderstanding.ScanState;

        float dist = Vector3.Magnitude(defaultCursor.transform.position - hololensCamera.transform.position);
        distanceText.text = "Dist: " + string.Format("{0:0.####}", dist);

    }
}
