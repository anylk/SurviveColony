using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public BulletPool pool;
    private IKController ikController;
    public Weapon currentWeapon;

    [SerializeField] private bool autoFire;
    [Range(.1f, 1f)]
    public float aimDuration = .18f;
    [Range(0f, 1f)]
    public float aimWeight = 0;
    public bool aiming;


    private void Awake()
    {
        ikController = GetComponent<IKController>();
    }

    private void Start()
    {
        ikController.SetWeaponData(currentWeapon.ikWeaponData);
        currentWeapon.Initiaze(pool);
    }

    private void Update()
    {
        if (currentWeapon == null)
        {
            return;
        }

        if (aiming)
        {
            aimWeight += Time.deltaTime / aimDuration;
        }
        else
        {
            aimWeight -= Time.deltaTime / aimDuration;
        }

        aimWeight = Mathf.Clamp(aimWeight, 0, 1);

        ikController.UpdateAim(aimWeight);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartFiring();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopFiring();
        }

        if (autoFire && aimWeight == 1)
        {
            currentWeapon.UpdateFiring(Time.deltaTime);
        }

        currentWeapon.UpdateBullet(Time.deltaTime);
    }

    public void EquipWeapon()
    {

    }

    public void StartFiring()
    {
        autoFire = true;
        aiming = true;
    }

    public void StopFiring()
    {
        autoFire = false;
        aiming = false;
    }
}