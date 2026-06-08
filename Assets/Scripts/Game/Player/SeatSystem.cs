using System.Collections;
using System.Linq;
using UnityEngine;

public class SeatSystem : MonoBehaviour
{
    private bool Seated;
    private Transform EntitySeatPosition;
    public virtual void Sit(Transform entityToBeSeated, Transform ObjectSeat, EntityManager entity = null, EntityManager seatObject = null)
    {
        if (entity == null)
        {
            entity = entityToBeSeated.GetComponent<EntityManager>();
        }


        if (seatObject == null)
        {
            seatObject = GetComponent<EntityManager>();
        }

        if (entity != null && entity.canBeSeated)
        {
            entity.SetIsSeated(true);

            if (seatObject != null)
            {
                seatObject.SetIsSeated(true);
            }
            entity.MoveAnimator.SetBool("Seated", true);
            
            Seated = true;
            StartCoroutine(SetPosition(entityToBeSeated, ObjectSeat));
        }
    }

    private IEnumerator SetPosition(Transform entityToBeSeated, Transform ObjectSeat, float TimeToSet = -1)
    {
        Debug.Log("Set Position");
        float TimePased = 0;
        if (TimeToSet < 0)
        {
            while (Seated && ObjectSeat != null)
            {
                entityToBeSeated.position = ObjectSeat.position;
                TimePased += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (Seated && TimePased < TimeToSet && ObjectSeat != null)
            {
                entityToBeSeated.position = ObjectSeat.position;
                TimePased += Time.deltaTime;
                yield return null;
            }
        }
    }
    public virtual void StandUp(Transform ObjectToStandUp, Transform ObjectSeat)
    {
        EntityManager entity = ObjectToStandUp.gameObject.GetComponent<EntityManager>();
        Seated = false;
        entity.SetMovement(true);

        entity.MoveAnimator.SetBool("Seated", false);

    }
}
