using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] float speed = 10f;
    Transform insideCircle;
    [SerializeField] GameObject projectile;
    new Rigidbody2D rigidbody;
    Vector3 aimDir=Vector3.up;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0f;
        insideCircle = transform.GetChild(0).GetChild(0);
    }

    private void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onRotate += Rotate;
        input.onStopRotate += StopRotate;
        input.onAttack += Attack;
    }

    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onRotate -= Rotate;
        input.onStopRotate -= StopRotate;
        input.onAttack -= Attack;
    }

    private void Start()
    {
        input.EnableGameplayerInput();
    }

    void Move(Vector2 movement)
    {
        rigidbody.velocity = (movement).normalized * speed;
    }

    void StopMove()
    {
        rigidbody.velocity = Vector2.zero;
    }

    void Rotate(Vector2 rotateDir)
    {
        var rotateAngle = Mathf.Atan2(rotateDir.y, rotateDir.x) * Mathf.Rad2Deg;
        aimDir = rotateDir;
        insideCircle.rotation = Quaternion.Euler(0, 0, rotateAngle-90);
    }

    void StopRotate()
    {
        insideCircle.rotation = Quaternion.identity;
        aimDir = Vector3.up;
    }

    void Attack()
    {
        var clone = Instantiate(projectile, transform.position, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().AddForce(aimDir * 100);
    }


}
