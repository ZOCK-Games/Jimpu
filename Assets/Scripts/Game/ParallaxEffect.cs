using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParallaxObject
{
    public GameObject Objects;
    public float parallaxFaktor;
}
public class ParallaxEffect : MonoBehaviour
{
    public List<ParallaxObject> ParallaxObjects;
    public List<Vector3> startPositions;
    public Camera camera;
    void Start()
    {
        startPositions.Clear();
        for (int i = 0; i < ParallaxObjects.Count; i++)
        {
            startPositions.Add(ParallaxObjects[i].Objects.transform.position);
        }
        }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ParallaxObjects.Count; i++)
        {
            float distanz = (camera.transform.position.x * ParallaxObjects[i].parallaxFaktor);

            ParallaxObjects[i].Objects.transform.position = new Vector3(startPositions[i].x + distanz, startPositions[i].y, startPositions[i].z);
        }
    }
}
