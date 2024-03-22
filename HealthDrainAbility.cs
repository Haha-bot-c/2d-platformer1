using UnityEngine;
using System.Collections;

public class HealthDrainAbility : MonoBehaviour
{
    [SerializeField] private float _abilityDuration = 6f;
    [SerializeField] private float _healthDrainRate = 5f;
    [SerializeField] private float _abilityRadius = 5f;
    [SerializeField] private KeyCode _abilityKeyCode = KeyCode.E;

    private bool _abilityActive = false;
    private void Update()
    {
        // ���������� ����������� ��� ������� ������ ��������� � ���� ����������� �� �������
        if (Input.GetKeyDown(_abilityKeyCode) && !_abilityActive)
        {
            StartCoroutine(ActivateAbility());
        }
    }

    private IEnumerator ActivateAbility()
    {
        _abilityActive = true;
        float timer = 0f;

        // ���� ����� �������� ����������� �� �������
        while (timer < _abilityDuration)
        {
            // ��������������� � ������� � ������� �������� �����������

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _abilityRadius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent<Health>(out Health enemyHealth) && enemyHealth != null)
                {
                    // �������� �������� � ����� � ���������� ��� ����
                    float healthDrained = _healthDrainRate * Time.deltaTime;
                    enemyHealth.TakeDamage(healthDrained);
                    GetComponent<Health>().Heal(healthDrained);

                }
            }

            // ����������� ������ �� ����� ��������� � ����������� �����
            timer += Time.deltaTime;

            yield return null; // ���� ���� ����
        }


        _abilityActive = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _abilityRadius);
    }
}