using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public MonsterBattle monsterBattle;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        RunnerManager.Instance.canMove = false;
        monsterBattle.BeginBattle();
    }
}
