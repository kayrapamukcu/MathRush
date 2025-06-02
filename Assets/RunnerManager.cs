using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GateSide { Left, Right }

public class RunnerManager : MonoBehaviour
{
    public static RunnerManager Instance;

    [Header("Runner Setup")]
    public GameObject runnerPrefab;
    public const float speed = 5f;
    public const float horizontalSpeed = 10f;
    private const float spawnGridSpacing = 0.2f;
    private const float constY = -1.2f;
    private const float minDist = 0.25f;
    public AudioClip individualDeathClip;
    public bool canMove = true;

    public List<GameObject> runners = new List<GameObject>();

    void Awake() { 
        Instance = this;
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 0;
    }

    void Start()
    {
        runners.Add(GameObject.FindWithTag("Player"));
    }

    void Update()
    {
        if (!canMove) return;
        float h = Input.GetAxis("Horizontal");
        if (h < 0f && runners.Exists(r => r.transform.position.x <= -5f))
            h = 0f;
        if (h > 0f && runners.Exists(r => r.transform.position.x >= 5f))
            h = 0f;


        Vector3 forward = Vector3.forward * speed * Time.deltaTime;
        Vector3 sideways = Vector3.right * h * horizontalSpeed * Time.deltaTime;

        for (int i = 0; i < runners.Count; i++)
        {
            Transform t = runners[i].transform;
            Vector3 pos = t.position + forward + sideways;
            pos.x = Mathf.Clamp(pos.x, -5f, 5f);
            pos.y = constY;
            t.position = pos;
        }
        
        for (int i = 0; i < runners.Count; i++)
        {
            for (int j = i + 1; j < runners.Count; j++)
            {
                var a = runners[i].transform;
                var b = runners[j].transform;
                Vector3 delta = a.position - b.position;
                float dist = delta.magnitude;
                if (dist > 0f && dist < minDist)
                {
                    Vector3 push = delta.normalized * (minDist - dist) * 0.125f;
                    a.position += push;
                    b.position -= push;
                }
            }
        }
    }

    public Vector3 GetGroupCenter()
    {
        if (runners.Count == 0) return Vector3.zero;
        Vector3 sum = Vector3.zero;
        foreach (var r in runners) sum += r.transform.position;
        return sum / runners.Count;
    }

    public void SpawnRunners(int count)
    {
        Vector3 center = GetGroupCenter();
        int cols = Mathf.CeilToInt(Mathf.Sqrt(count));
        int rows = Mathf.CeilToInt((float)count / cols);
        int spawned = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (spawned >= count) return;
                float xOff = (x - (cols - 1) / 2f) * spawnGridSpacing;
                float zOff = (y - (rows - 1) / 2f) * spawnGridSpacing;
                Vector3 spawnPos = center + new Vector3(xOff, constY, zOff);

                spawnPos.x = Mathf.Clamp(spawnPos.x, -5f, 5f);
                GameObject go = Instantiate(runnerPrefab, spawnPos, Quaternion.identity);
                runners.Add(go);
                spawned++;
            }
        }
    }


    public void KillRunners(int count, bool kill)
    {
        int actualKill = Mathf.Min(count, runners.Count - 1);
        if (kill)
        {
            actualKill = Mathf.Min(count, runners.Count);
        }
        for (int i = 0; i < actualKill; i++)
        {
            GameObject victim = runners[runners.Count - 1];
            runners.RemoveAt(runners.Count - 1);
            Destroy(victim);
        }
        CheckDeath();
    }

    public void DivideRunners(int divisor)
    {
        if (divisor <= 0 || runners.Count <= 1)
            return;

        int targetCount = (int)Mathf.Round(runners.Count / (float)divisor);
        int toKill = runners.Count - targetCount;
        KillRunners(toKill, false);
    }

    public void KillRunner(GameObject runner)
    {
        if (runners.Contains(runner))
        {
            runners.Remove(runner);
            Destroy(runner);
        }
        CheckDeath();
        SFXManager.Instance.Play(individualDeathClip, 0.8f);
    }

    public void CheckDeath()
    {
        if (runners.Count == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
