using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle")]
    public Camera cam;
    public float giveDamage = 10f;
    public float shootingRange = 100f;
    public float fireCharge = 15f;
    public PlayerScript player;
    public Animator animator;

    [Header("Rifle Ammunition and shooting")]
    private float nextTimeToShoot = 0.5f;
    private int maximumAmmunition = 20;
    private int mag = 15;
    private int presentAmmunition;
    public float reloadTime = 1.3f;
    private bool setReloading = false;

    [Header("Rifle effects")]
    public ParticleSystem muzzleSpark;
    public GameObject WoodedEffect;
    private void Awake()
    {
        presentAmmunition = maximumAmmunition;
    }
    private void Update()
    {
        if (setReloading)
        {
            return;
        }
        if (presentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToShoot)
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
            nextTimeToShoot = Time.time + 1 / fireCharge;
            Shoot();
        }
        else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("FireWalk", true);

        }
        else if (Input.GetButton("Fire1") && Input.GetButton("Fire2"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAnim", true);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Fire", false);
            animator.SetBool("FireWalk", false);

        }

    }

    void Shoot()
    {

        if (mag == 0)
        {
            Debug.Log("ammunition");
        }
        presentAmmunition--;


        if (presentAmmunition == 0)
        {
            mag--;
        }
        muzzleSpark.Play();
        RaycastHit hitInfo;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);
            Objects objects = hitInfo.transform.GetComponent<Objects>();
            if (objects != null)
            {
                objects.objectHitDamage(giveDamage);
                GameObject WoodGo = Instantiate(WoodedEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(WoodGo, 1f);
            }
        }
    }
    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("Debug reloading....");
        //
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("Reloading", false);
        presentAmmunition = maximumAmmunition;
        player.playerSpeed = 1.9f;
        player.playerSprint = 3f;
        setReloading = false;


        Debug.Log("Debug success reloading....");

    }
}
