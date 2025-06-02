using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MonsterBattle : MonoBehaviour
{
    public int maxHP = 50;
    public int bossHP = 50;
    public int damagePerAttack = 1;
    public float attackInterval = 1.0f;
    public string attackAnimationName = "Basic Attack";
    public Image barFill;

    public AudioClip playerPop;
    public AudioClip bossRoar;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void BeginBattle()
    {
        StartCoroutine(BattleRoutine());
    }

    private IEnumerator BattleRoutine()
    {
        while (bossHP > 0 && RunnerManager.Instance.runners.Count > 0)
        {
            SFXManager.Instance.Play(bossRoar, 0.125f);
            animator.Play("Basic Attack");
            yield return new WaitForSeconds(attackInterval);

            //damage
            if(RunnerManager.Instance.runners.Count > damagePerAttack)
            {
                bossHP -= damagePerAttack;
                RunnerManager.Instance.KillRunners(damagePerAttack, true);
            } else
            {
                bossHP -= RunnerManager.Instance.runners.Count;
                RunnerManager.Instance.KillRunners(RunnerManager.Instance.runners.Count, true);
            }
            
            SFXManager.Instance.Play(playerPop, 0.8f);

            UpdateHealthBar();
        }

        if (bossHP <= 0)
            OnBossDefeated();
        else
            OnPlayersDefeated();
    }

    private void OnBossDefeated()
    {
        Destroy(gameObject);
        int survivors = RunnerManager.Instance.runners.Count;
        ScoreManager.Instance.AddScore(survivors);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnPlayersDefeated()
    {
        RunnerManager.Instance.CheckDeath();
    }

    void UpdateHealthBar()
    {
        if (barFill != null)
            barFill.fillAmount = (float)bossHP / maxHP;
    }
}
