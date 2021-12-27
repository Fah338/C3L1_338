using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myPlayerBehaviour : MonoBehaviour
{

    public float speed; //'float' is short for floating point number, which is basically just a normal number
    public WeaponBehaviour [] weapons; 
    public int arraySize;
    int selectedWeaponIndex = 0;
    void Start()
    {
        References.thePlayer = gameObject;
        weapons = new WeaponBehaviour [arraySize];
        for (int index = 0;index<arraySize;index++){
            weapons[index] = new WeaponBehaviour();
        }
        selectedWeaponIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {

        //WASD to move
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Rigidbody ourRigidBody = GetComponent<Rigidbody>();
        ourRigidBody.velocity = inputVector * speed;

        Ray rayFromCameraToCursor = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        playerPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
        Vector3 cursorPosition = rayFromCameraToCursor.GetPoint(distanceFromCamera);
        gameObject.transform.LookAt(cursorPosition);

        // //Firing
        // if (weapons.Count > 0 && Input.GetButton("Fire1"))
        // {
        //     //Tell our weapon to fire
        //     weapons[selectedWeaponIndex].Fire(cursorPosition);
        // }
        if (Input.GetButton("Fire1"))
        {
            weapons[selectedWeaponIndex].Fire(cursorPosition);
        }

        //weapon switching
        if (Input.GetButtonDown("Fire2"))
        {
            ChangeWeaponIndex(selectedWeaponIndex + 1);
        }
    }

    private void ChangeWeaponIndex(int index)
    {

        //Change our index
        selectedWeaponIndex = index;
        //If it's gone too far, loop back around
        if (selectedWeaponIndex >= weapons.Length)
        {
            selectedWeaponIndex = 0;
        }

        //For each weapon in our list
        for (
            int i = 0; //Declare a variable to keep track of how many iterations we've done
            i < weapons.Length; //Set a limit for how high this variable can go
            i++ //Run this after each time we iterate - increase the iteration count
        )
        {
            if (i == selectedWeaponIndex)
            {
                //If it's the one we just selected, make it visible - 'enable' it
                weapons[i].gameObject.SetActive(true);
            } else
            {
                //If it's not the one we just selected, hide it - disable it.
                weapons[i].gameObject.SetActive(false);
            }
        }

    }
    
    private void OnTriggerEnter(Collider other)
    {
        WeaponBehaviour theirWeapon = other.GetComponentInParent<WeaponBehaviour>();
        if (theirWeapon != null)
        {
            if(selectedWeaponIndex<weapons.Length){
                weapons[selectedWeaponIndex] = theirWeapon;
                selectedWeaponIndex++;
            }
            
            theirWeapon.transform.position = transform.position;
            theirWeapon.transform.rotation = transform.rotation;
            theirWeapon.transform.SetParent(transform);
            
            
        }
    }

}