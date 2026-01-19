using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public float viteza = 5f;
    public float vitezaRotatie = 10f;
    private Animator animator;
    private CharacterController controller;
    private Transform cameraMain;
    public float mouseSensitivity = 100f;
    public float timpPragRoll = 0.2f;
    private float timpApasareShift = 0f;
    private bool esteInSprint = false;
    private bool esteInRoll = false;
    private Quaternion tintaRotatie;
    private bool esteInAtac=false;
    public Collider swordHitbox;


    void Start()
    { 
        tintaRotatie=transform.rotation;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cameraMain = Camera.main.transform;
    }

    void Update()
    {
        float x = 0;
        float z = 0;
        if (!esteInRoll && !esteInAtac)
            x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = cameraMain.forward;
        Vector3 cameraRight = cameraMain.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 directie = (cameraForward * z + cameraRight * x).normalized;
        if (esteInAtac)
        {
            directie = Vector3.zero;
        }

        if (directie.magnitude >= 0.1f && !esteInRoll && !esteInAtac)
        {
            tintaRotatie = Quaternion.LookRotation(directie);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, tintaRotatie, vitezaRotatie * Time.deltaTime);

        if (!esteInRoll && !esteInAtac)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                timpApasareShift = Time.time;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Time.time - timpApasareShift > timpPragRoll)
                {
                    esteInSprint = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                float durataApasare = Time.time - timpApasareShift;
                if (durataApasare <= timpPragRoll)
                {
                    if (directie.magnitude < 0.1f)
                    {
                        float xRaw = Input.GetAxisRaw("Horizontal");
                        float zRaw = Input.GetAxisRaw("Vertical");
                        if (Mathf.Abs(xRaw) > 0.1f || Mathf.Abs(zRaw) > 0.1f)
                        {
                            Vector3 fwd = cameraMain.forward;
                            Vector3 rgt = cameraMain.right;
                            fwd.y = 0; rgt.y = 0;
                            fwd.Normalize();
                            rgt.Normalize();

                            Vector3 directieSigura = (fwd * zRaw + rgt * xRaw).normalized;
                            ExecutaEvaziune(directieSigura);
                        }
                        else
                        {
                            ExecutaEvaziune(Vector3.zero);
                        }
                    }
                    else
                    {
                        ExecutaEvaziune(directie);
                    }
                }
                esteInSprint = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                attack();
            }
        }
        float vitezaCurenta = esteInSprint ? (viteza * 2f) : viteza;

        float miscareVerticala = -0.5f;
        Vector3 miscareFinala = directie * vitezaCurenta;
        miscareFinala.y = miscareVerticala;

        if (animator != null)
        {
            float targetSpeed = directie.magnitude * vitezaCurenta;
            animator.SetFloat("Speed", targetSpeed, 0.15f, Time.deltaTime);

        }
        if (!esteInRoll)
        {

            controller.Move(miscareFinala * Time.deltaTime);
        }
    }
    void attack()
    {
        esteInAtac = true;
        if (swordHitbox != null)
            swordHitbox.GetComponent<SwordHitbox>().ReseteazaListaAtac();
            swordHitbox.enabled = true;
        animator.SetTrigger("Attack");
        StartCoroutine(ResetAtac(0.9f));
    }
    System.Collections.IEnumerator ResetAtac(float durata)
    {
        yield return new WaitForSeconds(durata);
        if (swordHitbox != null)
            swordHitbox.enabled = false;
        esteInAtac = false;
    }
    void ExecutaEvaziune(Vector3 directie)
    {
        if (directie.magnitude > 0.1f)
        {
            tintaRotatie=Quaternion.LookRotation(directie);
            transform.rotation = tintaRotatie;
            animator.SetTrigger("Roll");
            StartCoroutine(DodgeDash(directie, 12f, 0.7f, 0.1f));
        }
        else
        { 
            animator.SetTrigger("Backstep");
            StartCoroutine(DodgeDash(-transform.forward, 6f, 0.45f,0.35f));
        }
    }
    System.Collections.IEnumerator DodgeDash(Vector3 directieDash, float forta, float durata, float timpRecuperare)
    {
        esteInRoll = true;
        float timpScurs = 0f;
        while (timpScurs < durata)
        {
            controller.Move(directieDash * forta * Time.deltaTime);
            timpScurs += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(timpRecuperare);
        esteInRoll = false;
    }
}