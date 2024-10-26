using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using static DataLoader;

public class EnergyShield : MonoBehaviour
{
    public TextMeshProUGUI scoreGT;
    public TextMeshProUGUI levelGT;
    public AudioSource audioSource;

    public int sceneIndex;
    private bool dataApplied = false;

    float eggsNumber;
    int score = 0;


    private void Start () {
        GameObject scoreGO = GameObject.Find("Score");
        if (scoreGO != null) {
            scoreGT = scoreGO.GetComponent<TextMeshProUGUI>();
            scoreGT.text = "Score: 0";
        }

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        GameObject levelGO = GameObject.Find("Level");
        if (levelGO != null) {
            levelGT = levelGO.GetComponent<TextMeshProUGUI>();
            levelGT.text = $"Level: {sceneIndex}";
        }
    }

    private void Update ()
    {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;

        if (!dataApplied && DataLoader.dataSet.ContainsKey(sceneIndex)) {
            DataLoader.DataEntry dataEntry = DataLoader.dataSet[sceneIndex];
            eggsNumber = dataEntry.EggsNumber;
            dataApplied = true;
        }

        if (dataApplied && score == eggsNumber) {
            LoadNextScene();
        }
    }

    private void OnCollisionEnter (Collision coll) {
        GameObject Collided = coll.gameObject;
        if (Collided.tag == "Dragon Egg") {
            Destroy(Collided);
        }

        // Извлекаем числовое значение из строки
        score += 1;
        scoreGT.text = $"Score: {score}";

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    private void LoadNextScene () {
        int nextSceneIndex = sceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else {
            Debug.Log("No more scenes to load. Closing the game.");
            Application.Quit();
        }
    }
}
