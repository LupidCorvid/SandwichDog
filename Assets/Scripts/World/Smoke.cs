using UnityEngine;

public class Smoke : MonoBehaviour
{
    private const float OFFSET_HEIGHT = 0.05f;
    private Food attachedFood;

    private Vector3 offsetToObj;

    public void FixedUpdate()
    {
        // only update position
        this.transform.position = attachedFood.transform.position + offsetToObj;
    }

    public void InitializeSmoke(Food inFood)
    {
        attachedFood = inFood;

        // set smoke to play on highest pt of object
        if (inFood.transform.position.y > inFood.topPoint.transform.position.y)
        {
            this.transform.position = attachedFood.topPoint.transform.position;
            // include height when updating pos
            offsetToObj = new Vector3(0.0f, attachedFood.topPoint.localPosition.y + OFFSET_HEIGHT, 0.0f);
        }
        else
        {
            this.transform.position = attachedFood.transform.position;
            offsetToObj = new Vector3(0.0f, OFFSET_HEIGHT, 0.0f);
        }
    }
}
