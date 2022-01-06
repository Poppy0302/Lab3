using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerBehaviour : MonoBehaviour
{

    
    public float speed; 
    public WeaponBehaviour[] weapons;
    public int selectedWeaponIndex;
    public int arraySize;

    // Start is called before the first frame update
    void Start()
    {
        References.thePlayer = gameObject;
        selectedWeaponIndex = 0;
        weapons = new WeaponBehaviour[arraySize];
        for (int index = 0;index<arraySize;index++ ){
            weapons[index]= new WeaponBehaviour();
        }
        selectedWeaponIndex = 0;
        addWeaponIndex = 0;

    }
    // Update is called once per frame
    void Update()
    {
        //การเคลื่อนที่ของแท่นปืน
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Rigidbody ourRigidBody = GetComponent<Rigidbody>();
        ourRigidBody.velocity = inputVector * speed;

        //การหมุนแท่นปืนเป็นวงกลม
        Ray rayFromCameraToCursor = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        playerPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
        Vector3 cursorPosition = rayFromCameraToCursor.GetPoint(distanceFromCamera);
        gameObject.transform.LookAt(cursorPosition);

        //กำหนดให้กดปุ่มเม้าซ้ายยิงปืน
        if (weapons.Count > 0 && Input.GetButton("Fire1"))
        {
            //Tell our weapon to fire
            weapons[selectedWeaponIndex].Fire(cursorPosition);
        }
        if (Input.GetButton("Fire1"))
        {
            //Tell our weapon to fire
            if (selectedWeaponIndex<=addWeaponIndex){
                weapons[selectedWeaponIndex].Fire(cursorPosition);
            }
        }
                //weapon switching
        if (Input.GetButtonDown("Fire2"))
        {
            ChangeWeaponIndex(selectedWeaponIndex + 1);
        }


    }
    
    private void OnTriggerEnter(Collider other)
    {
        WeaponBehaviour theirWeapon = other.GetComponentInParent<WeaponBehaviour>();
        if (theirWeapon != null)

        {
           for (int i = 0; i < weapons.Length; i++)
            {
                 if ( i == selectedWeaponIndex )
                {
                    weapons[i] = theirWeapon;
                }
                
            }
            theirWeapon.transform.position = transform.position;
            theirWeapon.transform.rotation = transform.rotation;
            //Parent it to us - attach it to us, so it moves with us
            theirWeapon.transform.SetParent(transform);
            //Select it!
            ChangeWeaponIndex(weapons.Length );

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
        for (int i = 0; i < weapons.Length; i++ )
        {
            if (i == selectedWeaponIndex)
            {
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }

    }

    

}
