using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    public GameManager manager;
    public bool isDisplay;
    public void Start()
    {
        Debug.Log("BasketController ready!");
    }
    public void OnDisable()
    {
        Debug.Log("BasketController stopped!");
    }
    // Update is called once per frame
    void Update()
    {
        if (!manager.paused)
        {
            if (!isDisplay)
            {
                gameObject.GetComponent<Renderer>().enabled = Input.GetKey(KeyCode.R);
            }
            // Get cursor position
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            // Fix position to y=-3 and z=0
            newPosition.y = -3; newPosition.z = 0;
            // Limit position to on screen
            //if (newPosition.x < -8.8)
            //{
            //    newPosition.x = -8.8f;
            //}
            //else if (newPosition.x > 8.8)
            //{
            //    newPosition.x = 8.8f;
            //}
            // Set the basket position to the modified cursor position
            if (isDisplay)
            {
                transform.position = Vector3.Lerp(transform.position, newPosition, 0.025f);
            } else
            {
                transform.position = newPosition;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<FallingObject>().Caught();
    }
}
