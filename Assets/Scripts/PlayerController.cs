using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveValue;
    public float speed;
    private int count;
    private int numPickups = 4;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI playerPositionText;
    public TextMeshProUGUI playerVelocityText;
    public TextMeshProUGUI distanceToClosestPickUpText;
    private Vector3 previousPosition;
    private GameObject[] allPickups;
    private GameObject closestPickup;
    float closestPickUpDistance = 0.0f;
    private LineRenderer lineRenderer;

    private void Start()
    {
        count = 0;
        winText.text = "";
        playerVelocityText.text = "Player Velocity: N/A";
        distanceToClosestPickUpText.text = "Closest PickUp: N/A";
        SetCountText();
        allPickups = GameObject.FindGameObjectsWithTag("PickUp");
        closestPickup = allPickups[0];
        lineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
        previousPosition = this.transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);

        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            count++;
            other.gameObject.SetActive(false);
            Debug.Log(count);
            SetCountText();
        }
    }

    private void SetCountText()
    {
        scoreText.text = "Score: " + count.ToString();
        if(count >= numPickups)
        {
            winText.text = "You Win!";
            distanceToClosestPickUpText.text = " ";
        }
    }

    private void Update()
    {
        playerPositionText.text = "Player Position: " + this.transform.position.ToString();
        playerVelocityText.text = "Player Velocity: " + ((this.transform.position - previousPosition) / Time.deltaTime);
        distanceToClosestPickUpText.text = "Closest PickUp: " + closestPickUpDistance;
        UpdateClosesetPickUp();
    }

    private void UpdateClosesetPickUp()
    {
        closestPickUpDistance = Vector3.Distance(closestPickup.transform.position, this.transform.position);
        foreach (var PickUp in allPickups)
        {
            float pickUpDistance = Vector3.Distance(PickUp.transform.position, this.transform.position);    
            PickUp.GetComponent<Renderer>().material.color = Color.white;
            if (pickUpDistance < closestPickUpDistance)
            {
                closestPickup = PickUp; 
            }
        }
        closestPickup.GetComponent<Renderer>().material.color = Color.blue;
        lineRenderer.SetPosition (0, this.transform.position);
        lineRenderer.SetPosition(1, closestPickup.transform.position);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

}
