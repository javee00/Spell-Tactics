using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    private string _name;
    private EnterDuelUI duel;

    private void Awake()
    {
        duel = FindFirstObjectByType<EnterDuelUI>();
    }

    private void OnMouseDown()
    {
        EnterDuel();
    }

    public void EnterDuel()
    {
        Debug.Log("Entering duel with " + _name);
        duel.SetEnemy(this);
        duel.transform.localScale = Vector3.one;
    }

    public string GetName()
    {
        return _name;
    }

    public void SetName(string name)
    {
        _name = name;
    }
}
