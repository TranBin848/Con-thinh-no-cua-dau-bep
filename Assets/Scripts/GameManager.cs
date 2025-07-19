﻿using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float anger = 0f;
    private float reputation = 100f;
    public float maxAnger = 100f;
    public float maxReputation = 100f;
    public float angerIncreaseRate = 5f;
    public float reputationDecreaseOnTimeout = 10f;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseAnger(float amount)
    {
        anger = Mathf.Clamp(anger + amount, 0f, maxAnger);
        Debug.Log($"Anger: {anger}/{maxAnger}");
        if (anger >= maxAnger)
        {
            Debug.Log("Chef is furious! Game Over!");
            // TODO: Thêm logic game over (như reload Scene hoặc hiển thị màn hình thua)
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void DecreaseReputation(float amount)
    {
        reputation = Mathf.Clamp(reputation - amount, 0f, maxReputation);
        Debug.Log($"Reputation: {reputation}/{maxReputation}");
        if (reputation <= 0)
        {
            Debug.Log("Restaurant reputation is zero! Game Over!");
            // TODO: Thêm logic game over
        }
    }

    public float GetAnger() => anger;
    public float GetReputation() => reputation;
}
