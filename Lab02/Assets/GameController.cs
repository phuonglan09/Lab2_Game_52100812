using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private DebugMode currentDebugMode = DebugMode.Normal;
    public TextMeshProUGUI positionText;



    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject[] pickups;
    public TextMeshProUGUI playerVelocityText;
    public TextMeshProUGUI closestPickupDistanceText;

    private LineRenderer lineRenderer;
    public GameObject player; // Tham chiếu đến đối tượng người chơi
    void Start()
    {
        pickups = GameObject.FindGameObjectsWithTag("PickUp");

        // Thêm thành phần LineRenderer vào đối tượng GameController
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Thiết lập các thuộc tính của LineRenderer
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }
    public void SetCountText(int count)
    {
        winTextObject.SetActive(false);
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }

    void Update()
    {
        // Tìm khoảng cách đến pickup gần nhất
        float closestDistance = float.MaxValue;
        GameObject closestPickup = null;

        foreach (GameObject pickup in pickups)
        {
            if (pickup.activeSelf) // Chỉ xem xét các pickup còn hoạt động
            {
                float distance = Vector3.Distance(PlayerUpdate.instance.transform.position, pickup.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPickup = pickup;
                }
            }
        }

        // Hiển thị khoảng cách đến pickup gần nhất
        closestPickupDistanceText.text = "Distance to Closest Pickup: " + closestDistance.ToString("0.00") + " units";

        // Thay đổi màu sắc của pickup gần nhất sang màu xanh
        if (closestPickup != null)
        {
            // Thiết lập vị trí cho LineRenderer (từ người chơi đến pickup gần nhất)
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, closestPickup.transform.position);
            foreach (GameObject pickup in pickups)
            {
                if (pickup == closestPickup)
                {
                    pickup.GetComponent<Renderer>().material.color = Color.blue;
                }
                else
                {
                    pickup.GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Chuyển sang chế độ gỡ lỗi tiếp theo (lặp lại)
            currentDebugMode = (DebugMode)(((int)currentDebugMode + 1) % Enum.GetValues(typeof(DebugMode)).Length);
        }

        // Dựa trên chế độ gỡ lỗi hiện tại, cập nhật thông tin gỡ lỗi
        UpdateDebugInformation();
    }
    void UpdateDebugInformation()
    {
        switch (currentDebugMode)
        {
            case DebugMode.Normal:
                // Ẩn tất cả thông tin gỡ lỗi
                HideDebugInformation();
                break;
            case DebugMode.Distance:
                // Hiển thị thông tin gỡ lỗi khoảng cách
                ShowDistanceDebugInformation();
                break;
            case DebugMode.Vision:
                // Hiển thị thông tin gỡ lỗi tầm nhìn
                ShowVisionDebugInformation();
                break;
        }
    }
    void HideDebugInformation()
    {
        positionText.gameObject.SetActive(false);
        playerVelocityText.gameObject.SetActive(false);
        closestPickupDistanceText.gameObject.SetActive(false);

        
    }

    void ShowDistanceDebugInformation()
    {
        positionText.gameObject.SetActive(true);
        playerVelocityText.gameObject.SetActive(true);
        closestPickupDistanceText.gameObject.SetActive(true);
        // Xác định pickup tiềm năng và thay đổi màu sắc
        GameObject potentialPickup = DeterminePotentialPickup();

        if (potentialPickup != null)
        {
            // Hiển thị khoảng cách đến pickup tiềm năng
            float distanceToPotentialPickup = Vector3.Distance(PlayerUpdate.instance.transform.position, potentialPickup.transform.position);
            closestPickupDistanceText.text = "Distance to Potential Pickup: " + distanceToPotentialPickup.ToString("0.00") + " units";

            // Thiết lập vị trí cho LineRenderer (từ người chơi đến pickup tiềm năng)
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, potentialPickup.transform.position);

            // Thay đổi màu sắc của các pickup
            foreach (GameObject pickup in pickups)
            {
                if (pickup == potentialPickup)
                {
                    pickup.GetComponent<Renderer>().material.color = Color.green;
                    // Hướng pickup tiềm năng về phía người chơi
                    pickup.transform.LookAt(player.transform.position);
                }
                else
                {
                    pickup.GetComponent<Renderer>().material.color = Color.white;
                    // Pickup không được tiềm năng, nên không cần hướng về phía người chơi
                }
            }
        }
        else
        {
            // Nếu không có pickup tiềm năng, ẩn thông tin về khoảng cách đến pickup
            closestPickupDistanceText.gameObject.SetActive(false);
        }
    }

    void ShowVisionDebugInformation()
    {
        positionText.gameObject.SetActive(true);
        playerVelocityText.gameObject.SetActive(true);
        closestPickupDistanceText.gameObject.SetActive(true);

        // Xác định pickup tiềm năng và thay đổi màu sắc
        GameObject potentialPickup = DeterminePotentialPickup();

        if (potentialPickup != null)
        {
            // Hiển thị khoảng cách đến pickup tiềm năng
            float distanceToPotentialPickup = Vector3.Distance(PlayerUpdate.instance.transform.position, potentialPickup.transform.position);
            closestPickupDistanceText.text = "Distance to Potential Pickup: " + distanceToPotentialPickup.ToString("0.00") + " units";

            // Thiết lập vị trí cho LineRenderer (từ người chơi đến pickup tiềm năng)
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, potentialPickup.transform.position);

            // Thay đổi màu sắc của các pickup
            foreach (GameObject pickup in pickups)
            {
                if (pickup == potentialPickup)
                {
                    pickup.GetComponent<Renderer>().material.color = Color.green;
                    // Hướng pickup tiềm năng về phía người chơi
                    pickup.transform.LookAt(player.transform.position);
                }
                else
                {
                    pickup.GetComponent<Renderer>().material.color = Color.white;
                    // Pickup không được tiềm năng, nên không cần hướng về phía người chơi
                }
            }
        }
        else
        {
            // Nếu không có pickup tiềm năng, ẩn thông tin về khoảng cách đến pickup
            closestPickupDistanceText.gameObject.SetActive(false);
        }
    }

    GameObject DeterminePotentialPickup()
    {
        GameObject potentialPickup = null;
        float closestDot = -1f; // Giá trị dot product gần nhất tìm thấy

        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity.normalized; // Lấy vận tốc của người chơi

        foreach (GameObject pickup in pickups)
        {
            if (pickup.activeSelf) // Chỉ xem xét các pickup còn hoạt động
            {
                // Tính toán vector từ người chơi đến pickup
                Vector3 directionToPickup = (pickup.transform.position - player.transform.position).normalized;

                // Tính toán dot product của vector vận tốc của người chơi và vector hướng đến pickup
                float dotProduct = Vector3.Dot(playerVelocity, directionToPickup);

                // Nếu dot product lớn hơn dot product gần nhất đã tìm thấy trước đó, cập nhật pickup tiềm năng
                if (dotProduct > closestDot)
                {
                    closestDot = dotProduct;
                    potentialPickup = pickup;
                }
            }
        }

        return potentialPickup;
    }

    public enum DebugMode
    {
        Normal,
        Distance,
        Vision
    }
}
