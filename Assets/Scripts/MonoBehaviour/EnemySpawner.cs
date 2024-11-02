using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    [SerializeField] List<GameObject> pool, inactivePool;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Vector2 spawnRange;
    [SerializeField] float heightOffset = 10;
    [SerializeField] float gracePeriod = 10, spawnCooldown = 5;
    [SerializeField] int spawnCount = 0;

    float minRange, maxVariance;
    void Awake()
    {
        // pool = new List<GameObject>();
        instance = this;
    }

    void Start()
    {
        StartCoroutine(StartSpawner());
        minRange = spawnRange.x;
        maxVariance = spawnRange.y - minRange;
    }

    IEnumerator StartSpawner()
    {
        yield return new WaitForSeconds(gracePeriod);

        for (int a = 0; spawnCount == 0 || a < spawnCount; a++)
        {
            Vector2 offsetDirection = Random.insideUnitCircle.normalized;
            offsetDirection *= minRange + Random.value * maxVariance;
            Vector3 offsetDistance = new Vector3(offsetDirection.x, heightOffset, offsetDirection.y);
            offsetDistance.x *= Random.value > 0.5f ? 1 : -1;
            offsetDistance.z *= Random.value > 0.5f ? 1 : -1;

            Vector3 startingPosition = transform.position + offsetDistance;
            Quaternion startingRotation = Quaternion.Euler(0, Random.value * 360, 0);
            SpawnEnemy(startingPosition, startingRotation);
            if (spawnCooldown < 0.2 && a % Mathf.RoundToInt(0.2f / spawnCooldown) != 0)
            {
                continue; // Yield is expensive, continue spawning instead
            }
            yield return new WaitForSeconds(Mathf.Max(spawnCooldown, 0.2f));
        }
    }

    void ResetObjectState(GameObject obj, Vector3 pos, Quaternion rot)
    {
        obj.transform.position = pos;
        obj.transform.rotation = rot;

        EnemyChase enemyChase = obj.GetComponent<EnemyChase>();
        enemyChase.chaseTarget = transform;
        enemyChase.isSpawning = true;

        Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;

        obj.SetActive(true);
        Animator animator = obj.GetComponent<Animator>();
        animator.ResetTrigger("isGrounded");
        animator.speed = 1;
        animator.Play("Falling Idle");
    }

    void SpawnEnemy(Vector3 pos, Quaternion rot)
    {
        if (inactivePool.Count == 0)
        {
            var newObj = Instantiate(enemyPrefab, pos, rot);
            newObj.GetComponent<EnemyChase>().chaseTarget = transform;

            pool.Add(newObj);
        }
        else
        {
            var oldObj = inactivePool[0];
            inactivePool.RemoveAt(0);

            ResetObjectState(oldObj, pos, rot);
        }

    }

    public IEnumerator DespawnObject(GameObject obj)
    {
        yield return new WaitForSeconds(3);

        inactivePool.Add(obj);
        obj.SetActive(false);
    }
}
