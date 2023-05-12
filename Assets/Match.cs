using System.Collections;
using UnityEngine;
using TMPro;

public class Match: MonoBehaviour {
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject endCube;
    [SerializeField] private TMP_Text resultText;
    private const float marginOfError = 2f;
    private const float moveSpeed = 1f;
    private const float rotateSpeed = 0.1f;
    private const float scaleSpeed = 0.001f;
    private float startTime;
    private bool isMatched;

    private void Start() {
        startTime = Time.time;
    }

    private void Update() {
        MoveCube();
        RotateCube();
        ScaleCube();
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Quit();
        }

        if (!isMatched && CubesMatch()) {
            SetCubesColor(Color.green);
            DisplayCompletionMessage();
            isMatched = true;
            this.enabled = false;
            Invoke("Quit", 5f);
        }
    }

    private void MoveCube() {
        Vector3 movement = new Vector3(0f, Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private void RotateCube() {
        if (Input.GetKey(KeyCode.E)) {
            transform.Rotate(Vector3.right, rotateSpeed);
        }
        if (Input.GetKey(KeyCode.Q)) {
            transform.Rotate(Vector3.left, rotateSpeed);
        }
    }

    private void ScaleCube() {
        if (Input.GetKey(KeyCode.X)) {
            transform.localScale += new Vector3(0, scaleSpeed, scaleSpeed);
        }
        if (Input.GetKey(KeyCode.Z)) {
            transform.localScale -= new Vector3(0, scaleSpeed, scaleSpeed);
        }
    }

    private bool CubesMatch() {
        bool positionMatches = Vector3.Distance(cube.transform.position, endCube.transform.position) <= marginOfError;
        bool rotationMatches = Quaternion.Angle(cube.transform.rotation, endCube.transform.rotation) <= marginOfError;
        bool scaleMatches = Vector3.Distance(cube.transform.localScale, endCube.transform.localScale) <= marginOfError / 10;

        return positionMatches && rotationMatches && scaleMatches;
    }

    private void SetCubesColor(Color color) {
        cube.GetComponent <Renderer>().material.color = color;
        endCube.GetComponent <Renderer>().material.color = color;
    }

    private void DisplayCompletionMessage() {
        float completionTime = Time.time - startTime;
        resultText.text = string.Format("You took {0} seconds to get to match the squares", completionTime);
        resultText.color = Color.green;
    }

    public void Quit() {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}