using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    //We make team an integer, so we can use it as an index in arrays
    public int team;
    public Laser laserPrefab;
    public int health;
    public int attackPower;

    protected Rigidbody rb;
    protected Animator anim;

    private Eye[] eyes;
    private float fov = 60;

    //A virtual method CAN BE OVERRIDDEN (but doesn't have to be)
    protected virtual void Start ()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        eyes = GetComponentsInChildren<Eye>();

        Color teamColor = GameManager.instance.teamColors[team];
        transform.Find("Teddy/Teddy_Body").GetComponent<Renderer>().material.color = teamColor;
    }

    protected bool CanSee (Vector3 hitPoint, Transform hitTransform)
    {
        foreach(Eye eye in eyes)
        {
            Vector3 startPos = eye.transform.position;
            //The direction from the Unit to the point that we hit
            Vector3 direction = hitPoint - startPos;

            //If the other object is outside of my field of view, I can't see it 
            if (Vector3.Angle(transform.forward, direction) > fov)
            {
                return false;
            }

            //We create a ray that starts at the eye and goes towards the hitPoint
            Ray eyeRay = new Ray(startPos, direction);
            RaycastHit hitInfo;
            if (Physics.Raycast(eyeRay, out hitInfo))
            {
                //If an eye hits something else than the hitTransform, we can't see it
                if (hitInfo.transform != hitTransform)
                {
                    return false;
                }
            }
        }
        //If both eye-rays hit the same transform as hitTransform, we can see it
        return true;
    }

    protected void ShootLasers(Vector3 hitPoint, Transform hitTransform)
    {
        foreach(Eye eye in eyes)
        {
            Laser laserClone = Instantiate(laserPrefab);
            laserClone.Init(eye.transform.position, hitPoint);
        }

        //Was the object that we hit a Unit?
        Unit unit = hitTransform.GetComponent<Unit>();
        if (unit != null)
        {
            //We are calling OnHit on the OTHER unit 
            //(this Unit damages the OTHER Unit)
            unit.OnHit(attackPower);
        }
    }

    public void OnHit (int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die ()
    {
        anim.SetBool("Death", true);
    }
}
