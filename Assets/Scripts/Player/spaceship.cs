using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class JumpTarget
{
    public string target_name;
    public SerializableVector3 position;

    public JumpTarget(string _target, SerializableVector3 _pos)
    {
        target_name = _target;
        position = _pos;
    }
}
public class spaceship : MonoBehaviour
{
    [Header("Movement")]
    public float sensitivity;
    public float maxRot;
    public float speedLimit1;
    public float speedLimit2;
    public float maxAcceleration;
    public float rotSensitivity;

    [Header("Ship Stats")]
    public float maxFuel;
    public float maxIntegrity;
    public float maxVoidShield;

    public float fuel;
    public float integrity;
    public float voidShield;

    public bool isJumping = false;
    public float jumpFuelMultipler = 5;
    public float jumpMaxDistance = 40000;
    public GameObject jumpEffect;

    [Header("Ship Multipliers")]
    public float darkEnergyAbsorption;
    public float darkEnergyImmunity;

    public float armorAbsorption;

    private float fuelConsumptionSpeed = 0;
    private float damageConstant = 2f;
    public  float fuelConsumptionMultiplier = 0.1f;

    internal float jumpDistance = 0;
    [SerializeField]
    public List<JumpTarget> jumpTargets = new List<JumpTarget>();
    public int currentTarget;
    [SerializeField]
    private float voidDMGMultiplier = 0.05f;
    public float currentDarkEnergyLevel;

    [Header("Misc")]
    public GameObject rL;
    public GameObject lL;
    public CameraShake shaker;
    public StationLandingZone lastStation;
    public GameObject ship;

    public AudioSource musicSource;
    public AudioClip gameOver;
    public GameObject spookyMan;
    public AudioSource engine;
    public AudioSource sub_engine;

    [Header("Debug Info")]
    public Vector3 rotation;
    Rigidbody rb;
    public float speed;
    public float speedReal;
    public float accel;

    public bool speedLimit1_reached;
    public bool speedLimit2_reached;

    bool ohFuck;
    bool fuelMul;
    public bool controlsLocked;
    public bool rotLocked;
    float rot;
    Quaternion def_rot_q;
    public int jumpOn;
    public bool docked;
    public Vector3 dockPosition;
    public Quaternion dockRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        speedReal = GetComponent<Rigidbody>().velocity.magnitude;
        if (!controlsLocked)
        {
            float inputX = 0, inputY = 0;
            if (!rotLocked)
            {
                inputX = Mathf.Clamp(Input.GetAxisRaw("Mouse X"), -maxRot, maxRot);
                inputY = Mathf.Clamp(Input.GetAxisRaw("Mouse Y"), -maxRot, maxRot);
            }

            rot -= Input.GetAxisRaw("Horizontal") * rotSensitivity * Time.deltaTime;

            rotation = Vector3.Lerp(rotation, new Vector3(-inputY, inputX), Time.deltaTime * sensitivity);
            rotation.z = rot;

            if (Input.GetKeyDown(KeyCode.J))
            {
                jumpOn++;
                if (jumpOn > 2)
                    jumpOn = 0;
                jumpDistance = 0;
            }

            if (jumpOn == 1)
            {
                int mul = Input.GetKey(KeyCode.LeftControl) ? 10 : 1;
                speed = Mathf.Lerp(speed, 0, maxAcceleration);
                if (Input.GetKeyDown(KeyCode.W))
                    jumpDistance += 1000 * mul;
                else if (Input.GetKeyDown(KeyCode.S))
                    jumpDistance -= 1000 * mul;
                jumpDistance = Mathf.Clamp(jumpDistance, 0, jumpMaxDistance);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartCoroutine("WarpJump", null);
                }
            }
            else if(jumpOn == 2)
            {
                if (Input.GetKeyDown(KeyCode.W))
                    currentTarget++;
                else if (Input.GetKeyDown(KeyCode.S))
                    currentTarget--;

                if (currentTarget >= jumpTargets.Capacity)
                    currentTarget = 0;
                if(currentTarget < 0)
                    currentTarget = jumpTargets.Capacity;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Vector3 pos = jumpTargets[currentTarget].position;
                    StartCoroutine("WarpJump", pos);
                }
            }

            if (jumpOn == 0)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    //if (accel == 0)
                    //    shaker.SetShakeTimer(2, 0.02f);
                    accel += 1;

                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    //if (accel == 0)
                    //    shaker.SetShakeTimer(2, 0.02f);
                    accel -= 1;
                }

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    Vector3 def_rot = new Vector3(0, transform.rotation.y, 0);
                    def_rot_q = Quaternion.Euler(def_rot);
                }
                if (Input.GetKey(KeyCode.LeftControl))
                    transform.rotation = Quaternion.Lerp(transform.rotation, def_rot_q, Time.deltaTime * sensitivity);
            }


            if (accel == 0 && Math.Abs(speed) < 1f)
                speed = 0;

            if (Input.GetKeyDown(KeyCode.R))
                rot = 0;

            if (speed > speedLimit1)
                speedLimit1_reached = true;
            else
                speedLimit1_reached = false;
            if (speed > speedLimit2)
                speedLimit2_reached = true;
            else
                speedLimit2_reached = false;

            engine.volume = Mathf.Lerp(engine.volume, Math.Abs(accel / maxAcceleration) + 0.08f, Time.deltaTime * 2);
            engine.pitch = Mathf.Lerp(engine.volume, Math.Abs(accel / maxAcceleration) + 0.5f, Time.deltaTime * 2);
            sub_engine.volume = Mathf.Lerp(sub_engine.volume, Mathf.Clamp(Mathf.Max(Mathf.Abs(inputX), Mathf.Abs(inputY)), 0, 0.85f), Time.deltaTime * 2);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            rL.SetActive(!rL.activeSelf);
            lL.SetActive(!lL.activeSelf);
        }

        fuel = Mathf.Clamp(fuel, 0, maxFuel);
        integrity = Mathf.Clamp(integrity, 0, maxIntegrity);
        voidShield = Mathf.Clamp(voidShield, 0, maxVoidShield);

        accel = Mathf.Clamp(accel, -maxAcceleration, maxAcceleration);
        speed += accel * Time.deltaTime;
        if (!controlsLocked)
        {
            FuelCheck();
            VoidCheck();
            if (fuel > 0)
                fuel -= Time.deltaTime * fuelConsumptionSpeed * fuelConsumptionMultiplier;
            else
            {
                integrity -= Time.deltaTime*fuelConsumptionMultiplier;
            }

        }
        if (integrity <= 0)
        {
            if(!ohFuck)
                StartCoroutine("GameOver");
        }
        SpeedLimit();
    }

    public void UpdateStats()
    {
        UpgradeManager upMan = UpgradeManager.instance;
        maxAcceleration = upMan.u_accel;
        speedLimit1 = upMan.u_speedLim1;
        speedLimit2 = upMan.u_speedLim2;

        maxIntegrity = upMan.u_integrity;
        armorAbsorption = upMan.u_armor;

        maxFuel = upMan.u_fuel;
        fuelConsumptionMultiplier = upMan.u_fuelMul;

        maxVoidShield = upMan.u_shield;
        darkEnergyAbsorption = upMan.u_absorptionVE;
        darkEnergyImmunity = upMan.u_minVE;

        sensitivity = upMan.u_sensitivity;

        jumpMaxDistance = upMan.u_maxDistJump;
        jumpFuelMultipler = upMan.u_fuelMulJump;
    }

    IEnumerator WarpJump(Vector3 v)
    {
        isJumping = true;
        jumpEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        float time = 0;
        shaker.shaking = true;
        while (time < 5)
        {
            shaker.SetShakeMultiplier(time/10);
            time += Time.deltaTime;

            yield return null;
        }
        jumpEffect.SetActive(false);
        if (v == null)
            transform.position = transform.position + transform.forward * jumpDistance;
        else
            transform.position = GameManager.instance.universe.transform.position + v;
        yield return null;
        fuel -= (jumpDistance/jumpMaxDistance) * maxFuel * jumpFuelMultipler;
        shaker.SetShakeMultiplier(0);
        shaker.shaking = false;
        isJumping = false;
        yield return null;
    }

    public IEnumerator GameOver()
    {
        controlsLocked = true;
        ohFuck = true;
        Destroy(ship);
        GameManager.instance.dogStar.transform.SetParent(null);
        Destroy(GameManager.instance.universe);
        transform.LookAt(GameManager.instance.dogStar.transform);
        musicSource.Stop();
        musicSource.clip = gameOver;
        musicSource.Play();
        yield return new WaitForSeconds(2);
        GameObject spook = Instantiate(spookyMan, transform.position + transform.forward * 8000, Quaternion.identity);
        spook.transform.LookAt(transform);
        yield return new WaitForSeconds(11);
        spook.transform.position = transform.position + transform.forward * 1000;
        print("ok");
        yield return new WaitForSeconds(3f);
        spook.transform.position = transform.position + transform.forward * 500;
        yield return new WaitForSeconds(3f);
        spook.transform.position = transform.position + transform.forward * 100;
        yield return new WaitForSeconds(2f);
        spook.transform.position = transform.position + transform.forward * 20;
        yield return new WaitForSeconds(0.5f);
        print("ok");
        Application.Quit();

    }

    private void VoidCheck()
    {
        if (currentDarkEnergyLevel > darkEnergyImmunity)
        {
            float darkEnergyAbsorbed = currentDarkEnergyLevel * (1 - darkEnergyAbsorption);
            if (voidShield > 0)
                voidShield -= darkEnergyAbsorbed * Time.deltaTime * voidDMGMultiplier;
            else
                integrity -= darkEnergyAbsorbed * Time.deltaTime * voidDMGMultiplier;
        }
        else
        {
            voidShield += 0.1f * Time.deltaTime;
        }
        voidShield = Mathf.Clamp(voidShield, 0, maxVoidShield);
    }

    private void SpeedLimit()
    {
        if (speedLimit1_reached)
        {
            fuelMul = true;
            shaker.shaking = true;
            shaker.SetShakeMultiplier(((speed / speedLimit1) - 1)*0.01f, 0.7f);
        }
        else
        {
            fuelMul = false;
            if(jumpOn == 0)
                shaker.shaking = false;
        }
        if (speedLimit2_reached)
            integrity -= Time.deltaTime * (speed / speedLimit2) * 2;
    }

    public void FuelCheck()
    {
        float yaw = Mathf.Abs(Mathf.Clamp(Input.GetAxisRaw("Mouse X"), -1f, 1f));
        float pitch = Mathf.Abs(Mathf.Clamp(Input.GetAxisRaw("Mouse Y"), -1f, 1f));
        float roll = Mathf.Abs(Mathf.Clamp(Input.GetAxisRaw("Horizontal"), -1f, 1f));

        float acc = Mathf.Abs(accel / maxAcceleration);

        float mul = fuelMul ? 1.5f : 1f;

        fuelConsumptionSpeed = (0.02f + acc + yaw + pitch + roll) * mul;
    }

    private void OnCollisionEnter(Collision collision)
    {

        print(collision.relativeVelocity.magnitude);
        float basicDamage = collision.relativeVelocity.magnitude * damageConstant;
        float reduced = basicDamage * (1 - armorAbsorption);
        integrity -= reduced;
        speed = 0;
        accel = 0;
    }

    public void SetDock(Vector3 position, Quaternion rotate, StationLandingZone stationDock)
    {
        dockPosition = position;
        dockRotation = rotate;
        lastStation = stationDock;
        speed = 0;
        accel = 0;
        rot = 0;
    }

    private void FixedUpdate()
    {
        if (!controlsLocked)
        {
            
                transform.Rotate(rotation);
            rb.velocity = transform.forward * speed;
        }
        if(docked && controlsLocked)
        {
            transform.SetPositionAndRotation(dockPosition, dockRotation);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                docked = false;
                controlsLocked = false;
                lastStation.hasPlayer = false;
                rb.AddRelativeForce(transform.up * 8, ForceMode.Impulse);
            }
        }
    }

}
