using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyChase : MonoBehaviour
{
    Animator animator;
    new Rigidbody rigidbody;
    public bool isSpawning = true;
    public Transform chaseTarget;
    [SerializeField] float rotationPerSecond = 10;
    [SerializeField] float uprightTorque = 20;
    [SerializeField] float moveStrength = 5;
    [SerializeField] GameObject highResModel, lowResModel;
    [SerializeField] float renderDistance = 15, chaseDistance = 10;
    bool isTooFar = false, isChasing = false, isStuckToWall = false;
    float distanceCheckTime, distanceCheckCooldown = 0.2f;
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.down * 3;
        distanceCheckTime = Time.time + 3 * distanceCheckCooldown * Random.value;
    }

    void Update()
    {
        if (Time.time < distanceCheckTime)
        {
            return;
        }
        distanceCheckTime = Time.time + distanceCheckCooldown;

        DistanceChecks();

    }

    void DistanceChecks()
    {
        // Chase range check
        isChasing = (transform.position - chaseTarget.position).sqrMagnitude < chaseDistance * chaseDistance;
        animator.SetBool("isChasing", isChasing);
        if (!isSpawning && isChasing)
        {
            PlayManager.instance.AddScore(1);
        }

        // RenderDistance considerations
        // isTooFar = Vector3.Distance(transform.position, Camera.main.transform.position) > 15;
        isTooFar = (transform.position - Camera.main.transform.position).sqrMagnitude > renderDistance * renderDistance;
        // animator.enabled = !isTooFar || isSpawning;
        highResModel.SetActive(!isTooFar);
        lowResModel.SetActive(isTooFar);
        // rigidbody.constraints = isTooFar ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.None;
    }

    void FixedUpdate()
    {
        if (isSpawning)
        {
            return;
        }

        UpdateLeaningForces();

        if (isChasing)
        {
            UpdateCharacterRotation();
            Vector3 runDirection = transform.forward;
            runDirection.y = 0;
            rigidbody.AddForce(runDirection.normalized * moveStrength, ForceMode.Force); // Move the enemy forward
        }
    }

    void UpdateLeaningForces()
    {
        var uprightRotation = Quaternion.FromToRotation(transform.up, Vector3.up);
        rigidbody.AddTorque(new Vector3(uprightRotation.x, uprightRotation.y, uprightRotation.z) * uprightTorque); // Keep the enemy upright

        if (Vector3.Angle(transform.up, Vector3.up) > 60) // Enemy fell, make ragdoll and queue for despawn
        {
            isSpawning = true;
            animator.speed = 0;
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.useGravity = true;
            // Destroy(gameObject, 3);
            StartCoroutine(EnemySpawner.instance.DespawnObject(gameObject));
            PlayManager.instance.AddScore(250);
        }
    }

    void UpdateCharacterRotation()
    {
        Vector3 targetDirection = chaseTarget.position - transform.position;
        targetDirection.y = 0;
        // Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized, Vector3.up);
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationPerSecond * Time.deltaTime);
        if (isTooFar)
        {
            transform.rotation = Quaternion.LookRotation(targetDirection.normalized, Vector3.up);
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            return;
        }
        var signedAngleToTarget = Vector3.SignedAngle(transform.forward, targetDirection.normalized, Vector3.up);
        var stuckToWallMultiplier = isStuckToWall ? 1 : 8;
        rigidbody.AddTorque(Vector3.up * signedAngleToTarget * rotationPerSecond * stuckToWallMultiplier, ForceMode.Force); // Make to enemy face the player
    }

    void OnCollisionEnter(Collision other)
    {
        if (isSpawning)
        {
            if (other.collider.CompareTag("Ground"))
            {
                animator.SetTrigger("isGrounded");
            }
            return;
        }

        if (other.collider.CompareTag("Player"))
        {
            PlayManager.instance.EndGame();
        }
    }

    void OnCollisionStay(Collision other)
    {
        isStuckToWall = false;
        if (!isChasing)
        {
            return;
        }

        // Slide the enemy along walls
        for (int a = 0; a < other.contactCount; a++)
        {
            var contact = other.GetContact(a);
            if (Vector3.Angle(contact.normal, Vector3.up) < 30)
            {
                continue;
            }

            isStuckToWall = true;
            Vector3 correctedForce = Vector3.ProjectOnPlane(transform.forward, contact.normal).normalized;
            correctedForce += contact.normal;
            correctedForce.y = 0f;
            rigidbody.AddForce(correctedForce.normalized * moveStrength, ForceMode.Force);
        }
    }

    public void FinishSpawning()
    {
        isSpawning = false;
        // rigidbody.useGravity = false;
        // rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        rigidbody.constraints = RigidbodyConstraints.None;

        // animator.applyRootMotion = true;
        // rigidbody.isKinematic = true;
    }
}
