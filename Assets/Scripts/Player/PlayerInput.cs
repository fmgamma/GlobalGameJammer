using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

  
    void Start()
    {
        //TO BE DONE
    }

    public bool CanProcessInput()
    {
        //TO BE DONE
        return (true);
    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // always returns a magnitude of 1
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }
    
    public Vector3 GetMoveInputP2()
    {
        if (CanProcessInput())
        {
            Vector2 move = new Vector2(Input.GetAxisRaw("HorizontalP2"), Input.GetAxisRaw("VerticalP2"));

            // always returns a 1
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }
    
    public float GetHorizontalInput()
    {
        if (CanProcessInput())
        {
            float move = Input.GetAxisRaw("Horizontal");

            return move;
        }
        return 0;
    }
    
    public float GetHorizontalInputP2()
    {
        if (CanProcessInput())
        {
            float move = Input.GetAxisRaw("HorizontalP2");

            return move;
        }

        return 0;
    }    
    public float GetVerticalInput()
    {
        if (CanProcessInput())
        {
            float move = Input.GetAxisRaw("Vertical");

            return move;
        }
        return 0;
    }
    
    public float GetVerticalInputP2()
    {
        if (CanProcessInput())
        {
            float move = Input.GetAxisRaw("VerticalP2");

            return move;
        }

        return 0;
    }
}