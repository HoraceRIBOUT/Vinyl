using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool playable = true;

    public float movementSpeedAtMax;
    public AnimationCurve speedUp;
    public AnimationCurve speedDown;

    private float acceleration = 0;
    private float currentSpeed = 0;






    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!playable)
            return;

        float xStick = Input.GetAxis("Horizontal");
        //float yStick = Input.GetAxis("Vertical");

        Deplacement(xStick);
        
    }

    void Deplacement(float xStick)
    {
        if (xStick == 0 && currentSpeed == 0)
            return;

        Vector3 movementFinal = Vector3.right;

        /*if(acceleration < 1 && xStick != 0)
        {
            acceleration += xStick * ;
        }

        movementSpeedAtMax
            speedUp*/

        movementFinal *= movementSpeedAtMax * xStick;

        this.transform.Translate(movementFinal);
    }

}
