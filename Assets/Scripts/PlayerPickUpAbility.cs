using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpAbility : MonoBehaviour
{
    public GameObject CarryObject;
    public float PlayerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CarryObject = null;
            }

            Vector3 movement = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                movement += new Vector3(0, PlayerSpeed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                movement += new Vector3(0, -PlayerSpeed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                movement += new Vector3(-PlayerSpeed, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement += new Vector3(PlayerSpeed, 0);
            }

            if (movement != Vector3.zero)
            {
                transform.position += movement;
                if (CarryObject != null)
                {
                    CarryObject.transform.position += movement;
                }
            }

           
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (CarryObject == null)
        {
            CarryObject = col.gameObject;
        }
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
    
    }
}
