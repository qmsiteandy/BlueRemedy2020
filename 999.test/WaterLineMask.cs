using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct WaterLine_Part
{
    public float height;
    public float velocity;
    public GameObject gameObject;
    public float xboundsMin;
    public float xboundsMax;
}

public class WaterLineMask : MonoBehaviour {

    public float velocityDamping = 0.999999f; // Proportional velocity damping, must be less than or equal to 1.
    public float timeScale = 25f;

    public float Width = 11;
    private float widthPerBar = 0.1f;
    public GameObject maskBar;

    [SerializeField] public WaterLine_Part[] parts;

    private int size;
    private float currentHeight;

    // Use this for initialization
    void Start ()
    {
        maskBar = transform.GetChild(0).gameObject;

        Initialize();   //複製排列maskBar
    }

    private void Initialize()
    {
        size = (int)Mathf.Ceil(Width / widthPerBar);
        parts = new WaterLine_Part[size];


        maskBar.transform.position -= new Vector3(Width / 2f, 0f, 0f);

        parts[0].gameObject = maskBar;
       
        for (int i = 0; i < size; i++)
        {
            if (i > 0)
            {
                GameObject go = Instantiate(maskBar);
                go.transform.parent = this.transform;
                go.transform.localPosition = new Vector3(parts[i - 1].gameObject.transform.localPosition.x + widthPerBar, parts[i - 1].gameObject.transform.localPosition.y, 1f);

                parts[i].gameObject = go;
            }
            parts[i].height = parts[i].gameObject.transform.position.y;

            parts[i].xboundsMin = parts[i].gameObject.transform.position.x - widthPerBar;
            parts[i].xboundsMax = parts[i].gameObject.transform.position.x + widthPerBar;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        for (int i = 1; i < size - 1; i++)
        {
            int j = i - 1;
            int k = i + 1;
            parts[i].height = (parts[i].gameObject.transform.localPosition.y + parts[j].gameObject.transform.localPosition.y + parts[k].gameObject.transform.localPosition.y) / 3.0f;
        }

        // Velocity and height are updated... 
        for (int i = 1; i < size - 1; i++)
        {
            // update velocity and height
            parts[i].velocity = (parts[i].velocity + (parts[i].height - parts[i].gameObject.transform.localPosition.y)) * velocityDamping;

            float timeFactor = Time.deltaTime * timeScale;
            if (timeFactor > 1f) timeFactor = 1f;

            parts[i].height += parts[i].velocity * timeFactor;

            // Update the dot position
            Vector3 newPosition = new Vector3(
                parts[i].gameObject.transform.localPosition.x,
                parts[i].height,
                parts[i].gameObject.transform.localPosition.z);
            parts[i].gameObject.transform.localPosition = newPosition;
        }
    }

    #region Interaction

    /// <summary>
    /// Make waves from a point
    /// </summary>
    /// <param name="location"></param>
    /// <param name="force"></param>
    public void Splash(Vector3 location, float force)
    {
        // Find the touched part
        for (int i = 0; i < (size - 1); i++)
        {
            if (location.x >= parts[i].xboundsMin
              && location.x < parts[i].xboundsMax)
            {
                Splash(i, force);

                return;
            }
        }
    }

    private void Splash(int i, float heightModifier)
    {
        parts[i].gameObject.transform.localPosition = new Vector3(
       parts[i].gameObject.transform.localPosition.x,
       parts[i].gameObject.transform.localPosition.y + heightModifier,
       parts[i].gameObject.transform.localPosition.z
       );
    }

    #endregion
}
