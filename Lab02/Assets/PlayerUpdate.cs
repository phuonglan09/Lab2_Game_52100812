using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUpdate : MonoBehaviour
{
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI velocityText;

    private Vector3 lastPosition;
    public static PlayerUpdate instance;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;

        lastPosition = transform.position;

        positionText.text = "Position: (" + transform.position.x.ToString("0.00") + ", " +
                            transform.position.y.ToString("0.00") + ", " +
                            transform.position.z.ToString("0.00") + ")";

        velocityText.text = "Velocity: (" + velocity.x.ToString("0.00") + ", " +
                            velocity.y.ToString("0.00") + ", " +
                            velocity.z.ToString("0.00") + ")";
    }
}