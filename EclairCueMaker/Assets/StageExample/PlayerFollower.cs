using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerFollower : MonoBehaviour
{
    public GameObject lookAt;

    public Transform cameraTransform;   // 操作するカメラ
    public float mouseSensitivity = 300.0f;  // マウス感度
    public float defaultDistance = 3.0f;
    public float defaultAngle = 30.0f;//degree
    public float defaultY = 0.0f;

    public float maxAngle = 80.0f;
    public float minAngle = 5.0f;

    private float y;    // カメラのY軸成分
    private float angle;
    private float distance;
    private Vector3 cameraOffset;

    private static bool cursorIsLocked = false;
    public static bool CursorIsLocked
    {
        get
        {
            return cursorIsLocked;
        }
        set
        {
            cursorIsLocked = value;
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
                SceneManager.UnloadScene("UIExample");
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadSceneAsync("UIExample",LoadSceneMode.Additive);
            }
        }
    }

    public void ResetCamera()
    {
        y = 0;
        angle = 0;
    }

    void Start()
    {
        y = defaultY;
        angle = defaultAngle;
        distance = defaultDistance;
    }

    void Awake()
    {
        CursorIsLocked = true;
    }

    void Update()
    {
        if (Application.isEditor && Input.GetButtonDown("Fire2"))
        {
            CursorIsLocked = true;
        }

        if (Input.GetKeyDown("escape"))
        {
            CursorIsLocked = !CursorIsLocked;
        }
    }

    // 全ての処理が終わったとにカメラの位置を調整するためにLateUpdateにする
    void LateUpdate()
    {


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance += scroll * 4;

        if (cursorIsLocked)
        {
            y += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
            angle -= Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

            if (angle > maxAngle) angle = maxAngle;
            if (angle < minAngle) angle = minAngle;

            cameraOffset = new Vector3(0, 0, -distance);
            cameraOffset = Quaternion.Euler(angle, y, 0) * cameraOffset;
        }

        Transform lookAtTransform = lookAt.GetComponent<Transform>();
        cameraTransform.position = lookAtTransform.position + cameraOffset;

        /*if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            distance += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 500;
            distance = Mathf.Clamp(distance, 3.0f, 20.0f);
        }*/
        
        // カメラをオブジェクトから角度(20.0f, angleY, 0.0f)にdistance分離れた位置に配置
        /*Vector3 center = transform.position + centerOffset;
        cameraTransform.position = center + (
            Quaternion.AngleAxis(angleY, Vector3.up) *
            Quaternion.AngleAxis(20.0f, Vector3.right) *
            new Vector3(0, 0, -distance)
        );*/
        cameraTransform.LookAt(lookAt.GetComponent<Transform>().position);
		cameraTransform.RotateAround (cameraTransform.position, cameraTransform.right, -6);
    }

}