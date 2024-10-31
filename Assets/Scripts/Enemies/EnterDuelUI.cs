using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDuelUI : MonoBehaviour
{
    [SerializeField] private string startDuelMessage = "¿Quieres iniciar un duelo con |enemy|?";
    [SerializeField] private TMP_Text message;
    [SerializeField] private string DuelSceneName = "Cards";

    private AttackEnemy attackEnemy;

    public void SetEnemy(AttackEnemy enemy)
    {
        attackEnemy = enemy;
        startDuelMessage = startDuelMessage.Replace("|enemy|", attackEnemy.GetName());
        message.text = startDuelMessage;
    }

    public void EnterDuel()
    {
        SceneManager.LoadScene(DuelSceneName);
    }

    public void Cancel()
    {
        transform.localScale = Vector3.zero;
    }
}
