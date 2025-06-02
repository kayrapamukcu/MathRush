using UnityEngine;

public class GatePairTrigger : MonoBehaviour
{
    public MathGate leftGate;
    public MathGate rightGate;

    public AudioClip gateClip;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        float avgX = RunnerManager.Instance.GetGroupCenter().x;
        bool chooseLeft = avgX < 0f;

        MathGate chosenGate = chooseLeft ? leftGate : rightGate;
        SFXManager.Instance.Play(gateClip, 0.8f);

        switch (chosenGate.operationType)
        {
            case "multiply":
                RunnerManager.Instance.SpawnRunners((int)(chosenGate.value - 1) * RunnerManager.Instance.runners.Count);
                break;
            case "add":
                RunnerManager.Instance.SpawnRunners((int)chosenGate.value);
                break;
            case "subtract":
                RunnerManager.Instance.KillRunners((int)chosenGate.value, false);
                break;
            case "divide":
                RunnerManager.Instance.DivideRunners((int)chosenGate.value);
                break;
        }
        Destroy(leftGate.gameObject);
        Destroy(rightGate.gameObject);
        Destroy(gameObject);
    }
}
