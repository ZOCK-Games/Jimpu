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
    void Update()
    {
        for (int i = 0; i < ParallaxObjects.Count; i++)
        {
            float distX = camera.transform.position.x * ParallaxObjects[i].parallaxFaktor;
            float distY = camera.transform.position.y * (ParallaxObjects[i].parallaxFaktor * 0.5f);
            ParallaxObjects[i].Objects.transform.position = new Vector3(startPositions[i].x + distX, startPositions[i].y + distY / 2, startPositions[i].z);
        }
    }
}
