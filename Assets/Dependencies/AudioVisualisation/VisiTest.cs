using UnityEngine;
using System.Collections;

public class VisiTest : MonoBehaviour {

    public SoundCapture capture;


    GameObject[] bars;

    // Use this for initialization
    void Start () {
        MakeBars();
    }

    void MakeBars()
    {
        bars = new GameObject[capture.numBars];
        for (int i = 0; i < bars.Length; i++)
        {
            GameObject visiBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visiBar.transform.position = new Vector3(i, 0, 0);
            visiBar.transform.parent = transform;
            visiBar.name = "VisiBar " + i;
            bars[i] = visiBar;
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if (bars.Length != capture.numBars)
        {
            foreach (GameObject bar in bars)
            {
                Destroy(bar);
            }

            MakeBars();
        }

        // Since this is being changed on a seperate thread we do this to be safe
        lock(capture.barData)
        {
            for (int i = 0; i < capture.barData.Length; i++)
            {
                // Don't make the bars too short
                float curData = Mathf.Max(0.01f, capture.barData[i]);

                // Set offset so they stretch off the ground instead of expand in the air
                bars[i].transform.position = new Vector3(i, curData / 2.0f*10.0f, 0);
                bars[i].transform.localScale = new Vector3(1, curData*10.0f, 1);
            }
        }
	}
}
