using UnityEngine;

public class SeatSystem : MonoBehaviour
{
    private bool Seated;
    void Start()
    {
    }

    // Update is called once per frame
    public virtual void Sit(Transform transformObj, Transform ObjectToSeat)
    {
        EntityManager entity = ObjectToSeat.gameObject.GetComponent<EntityManager>();
        entity.SetMovement(false);
        while (Seated)
        {
            ObjectToSeat.position = transformObj.position;
        }
    }
    public virtual void StandUp(Transform ObjectToStandUp)
    {
        EntityManager entity = ObjectToStandUp.gameObject.GetComponent<EntityManager>();
        Seated = false;
        entity.SetMovement(false);
    }
}
