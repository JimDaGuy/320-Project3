using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameObject light;

	public GameObject lightPrefab;

	public GameObject lightUIPrefab;
	
	public Transform lightSpawn;
	public GameObject playerCharacter;
	
	public GameObject heldItemParent;
	public GameObject heldLight;
	public GameObject heldLightUIParent;
	public GameObject heldLightUI;
	public Text heldLightText;
	public Text scoreText;

    public GameObject healthSlider;
    public GameObject manaSlider;
	
	public GameObject SceneManager;
	public string stateString;

	private float lastFire;
	private float lastManaIncrease;
    public bool sticky = true;

	public int health;
    public int ammo;
    public int maxHealth;
    public int maxAmmo;
	public int score;

	private float fireTime = 0.5f;
    private float manaRegenTime = 2.0f;

	void Start ()
	{
	    stateString = SceneManager.GetComponent<MainSceneManager>().currentState.ToString();
        healthSlider = GameObject.FindGameObjectWithTag("HealthSlider");
        manaSlider = GameObject.FindGameObjectWithTag("ManaSlider");

        maxHealth = 100;
        maxAmmo = 10;
        health = 100;
        ammo = 10;

        lastFire = 0;
        lastManaIncrease = 0;
    }
	
	void Update () 
	{
        stateString = SceneManager.GetComponent<MainSceneManager>().currentState.ToString();

        if (stateString == "Ingame")
	    {
	        float xPos = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
	        float yPos = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f;
	        float zPos = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

	        transform.Rotate(0, xPos, 0);
	        transform.Translate(0, 0, zPos);

            lastManaIncrease += Time.deltaTime;
            if (lastManaIncrease >= manaRegenTime && ammo < maxAmmo)
            {
                ammo++;
                manaSlider.GetComponent<Slider>().value = (float)ammo / (float)maxAmmo * 100.0f;
                lastManaIncrease = 0;
            }

            // Shooting Input - Left Click
            if (Input.GetMouseButton(0))
            {
                if (Time.time > fireTime + lastFire && ammo > 0)
                {
                    Fire(lightPrefab);
                    ammo--;
                    manaSlider.GetComponent<Slider>().value = (float)ammo / (float)maxAmmo * 100.0f;
                }
            }
        }
	}

	private void Fire(GameObject lightPrefab)
	{
		// create a transform from the lightSpawn and the playerCharacter
		Transform lightRot = lightSpawn;
		lightRot.rotation = playerCharacter.transform.rotation;
	
		// create a light from a bullet prefab
	    light = (GameObject)Instantiate(lightPrefab, lightSpawn.position, lightRot.rotation);
	
		// add velocity to the light
		light.GetComponent<Rigidbody>().velocity = light.transform.forward * 15;

		// mark the time the light was created
		lastFire = Time.time;

		// remove stake after 2 seconds
		Destroy(light, 4.0f);
	}
	
        /*
	void CycleWeapons(bool forward)
	{
	        int currentIndex = (int)currentWeapon;
	        int increment = (forward) ? 1 : -1;
	        
	        currentIndex += increment;
	        
	        // Fix out of bounds
	        if (currentIndex < 1)
	            currentIndex = numWeapons;
	        else if (currentIndex > numWeapons)
	            currentIndex = 1;
	        
	        currentWeapon = (Weapons)currentIndex;
	        UpdateWeaponViews();
	}
        */
	
	void UpdateWeaponViews()
	{
        Transform prevlightTransform = heldLight.transform;
        Transform uilightTransform = heldLightUI.transform;
        Destroy(heldLight);
        Destroy(heldLightUI);

        // Update held item light
        GameObject newLight = (GameObject)Instantiate(lightPrefab, prevlightTransform.position, prevlightTransform.rotation);
        newLight.gameObject.GetComponentInChildren<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        Destroy(newLight.GetComponent<Rigidbody>());
        newLight.transform.parent = heldItemParent.transform;
        heldLight = newLight;


        //Update UI Text
        heldLightText.text = "Light";

        //Update UI light
        GameObject newLightUI = (GameObject)Instantiate(lightUIPrefab, uilightTransform.localPosition, uilightTransform.localRotation);
        newLightUI.transform.SetParent(heldLightUIParent.transform, false);
        heldLightUI = newLightUI;
    }
	
	private void OnCollisionEnter(Collision other)
	{
        if (stateString == "Ingame")
        {
            //"End" the game when the player reaches the end house
            if (other.gameObject.tag == "EndHouse")
            {
                health = 0;
            }

            //// stick lights to surfaces
            //if (other.gameObject.tag == "Iron" && other.gameObject.tag != "Player" && sticky)
            //{
            //        light.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //}

            // kill the player if the monster hits them
            if (other.gameObject.tag == "Monster")
            {
                //Debug.Log("Hitting the monster!");
                health = 0;
            }

            if (other.gameObject.tag == "Pickup")
            {
                ammo++;
                Destroy(other.gameObject, 0);
            }
        }
    }

        //private void OnCollisonStay(Collision other)
        //{
        //if (stateString == "Ingame")
        //    {
        //        // decrement player health when a ghost or vampire is "hitting" them
        //        if (other.gameObject.tag == "Monster")
        //        {
        //            health--;
        //            healthSlider.GetComponent<Slider>().value = health;
        //        }
        //    }
        //}
}
