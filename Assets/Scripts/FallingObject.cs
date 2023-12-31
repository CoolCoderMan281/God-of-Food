using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public enum FoodType
    {
        NORMAL, MOLDY
    }

    public enum FoodCategory
    {
        Veggies, Fruit, Moldy_Veggies, Moldy_Fruit, Dairy, Moldy_Dairy, Protein, UNSET
    }

    public bool isExample = false;
    public float FallSpeedMultiplyer = 1.0f;
    public float MaxSpeed = 2.0f;
    public float SpawnBuffer;
    public bool SpawnBuffered = false;
    public Vector3 PrePausedVelo = Vector3.zero;
    public int Worth = 1;
    public int Punishment = -127;
    public Material material;
    public GameManager manager;
    private Rigidbody rb;
    public FoodType foodtype = FoodType.NORMAL;
    public FoodCategory category = FoodCategory.UNSET;

    public AudioHandler audioHandler;
    // Start is called before the first frame update
    void Start()
    {
        if (Punishment == -127)
        {
            Punishment = Worth;
        }
        //gameObject.GetComponent<Renderer>().material = material;
        rb = gameObject.GetComponent<Rigidbody>();
        if (isExample)
        {
            rb.useGravity = false;
            
        } else
        {
            rb.useGravity = false;
            Invoke(nameof(DisableSpawnBuffer), SpawnBuffer);
        }
    }

    void DisableSpawnBuffer()
    {
        rb.useGravity = true;
        SpawnBuffered = true;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!isExample) // Don't waste resources! Don't process if it's the template
        {
            if (rb.useGravity)
            {
                if (manager.Style == GameManager.SectionStyle.IDLE)
                {
                    Destroy(gameObject);
                }
                // Apply fall speed per frame
                if (rb.useGravity)
                {
                    rb.AddForce(new Vector3(0, -(FallSpeedMultiplyer) / 4, 0), ForceMode.Force);
                }
                if (rb.velocity.y < -MaxSpeed)
                {
                    rb.velocity = new Vector3(0, -MaxSpeed, 0);
                }
                if (transform.position.y <= -2) // Don't waste resources! Only raycast when within 1 unit from basket
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, Vector3.down, out hit))
                    {
                        if (hit.distance <= 0.5f)
                        {
                            //Caught();
                        }
                    }
                }
                if (transform.position.y <= -4) // Don't waste resources! Cull!
                {
                    if (foodtype == FoodType.NORMAL)
                    {
                        audioHandler.PlayAudio(audioHandler.foodMiss);
                    } else
                    {
                        audioHandler.PlayAudio(audioHandler.moldyMiss);
                    }
                    manager.UpdateScore(-Punishment,gameObject,caught:false);
                    Destroy(gameObject);
                }
            }
        }
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Basket")
    //    {
    //        Caught();
    //    }
    //}

    public void Caught()
    {
        manager.UpdateScore(Worth, gameObject,caught:true);
        Destroy(gameObject);
    }
}
