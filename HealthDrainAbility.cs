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
        // Активируем способность при нажатии кнопки активации и если способность не активна
        if (Input.GetKeyDown(_abilityKeyCode) && !_abilityActive)
        {
            StartCoroutine(ActivateAbility());
        }
    }

    private IEnumerator ActivateAbility()
    {
        _abilityActive = true;
        float timer = 0f;

        // Пока время действия способности не истекло
        while (timer < _abilityDuration)
        {
            // Взаимодействуем с врагами в радиусе действия способности

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _abilityRadius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent<Health>(out Health enemyHealth) && enemyHealth != null)
                {
                    // Забираем здоровье у врага и прибавляем его себе
                    float healthDrained = _healthDrainRate * Time.deltaTime;
                    enemyHealth.TakeDamage(healthDrained);
                    GetComponent<Health>().Heal(healthDrained);

                }
            }

            // Увеличиваем таймер на время прошедшее с предыдущего кадра
            timer += Time.deltaTime;

            yield return null; // Ждем один кадр
        }


        _abilityActive = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _abilityRadius);
    }
}