using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Currencies
{
    Coin,
    Money,
    Gold,
    Diamond
}
public class Currency : MonoBehaviour
{
    public Currencies currencyType;
    [SerializeField] public float value = 100;
    [SerializeField] public bool goToBag = false;
    [SerializeField] bool goToSafe = false;
    public bool isOnTheSafe = false;

    Rigidbody rb;

    Coroutine coroutineGoToBag;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GoToSafe(float flowSpeed)
    {
        goToSafe = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        StartCoroutine(IEGoToSafe(flowSpeed));
    }

    IEnumerator IEGoToSafe(float flowSpeed)
    {
        //print("Flow Speed: "+flowSpeed);
        while (goToSafe)
        {
            //rb.AddForce(new Vector3(flowSpeed, 0, 0));
            //transform.position += transform.up * Time.deltaTime * flowSpeed;
            rb.velocity = transform.up * flowSpeed;
            yield return null;
        }
    }

    public void StopGoingToSafe()
    {
        goToSafe = false;
        rb.constraints = RigidbodyConstraints.None;
    }

    public void GoToBag(Vector3 targetPos, float speed)
    {
        goToBag = true;
        DisableRigidbody();

        float v0, angle, time, height;
        //angle *= Mathf.Deg2Rad;
        height = targetPos.y + targetPos.magnitude * 1.3f;
        //CalculatePath(targetPos, angle, out v0, out time);

        targetPos = new Vector3(Random.Range(targetPos.x - 0.05f, targetPos.x + 0.15f), targetPos.y, targetPos.z);

        //Height ile atarken sorun oluþuyor onu çöz
        CalculatePathWithheight(targetPos, height, out v0, out angle, out time);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (gameObject.activeSelf)
            coroutineGoToBag = StartCoroutine(IEGoToBag(v0, angle, time, speed));

    }

    public void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        targetPos -= transform.position;

        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));

        time = xt / (v0 * Mathf.Cos(angle));
    }

    private float QuadraticEquation(float a, float b, float c, float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }

    public void CalculatePathWithheight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
    {
        targetPos -= transform.position;

        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float b = Mathf.Sqrt(2 * g * h);
        float a = (-0.5f * g);
        float c = -yt;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);

        time = tplus > tmin ? tplus : tmin;

        angle = Mathf.Atan(b * time / xt);

        v0 = b / Mathf.Sin(angle);

    }

    IEnumerator IEGoToBag(float v0, float angle, float time, float speed)
    {

        float t = 0;
        float xStartPos = transform.position.x;
        float yStartPos = transform.position.y;
        float g = -Physics.gravity.y;
        float speedBooster = 1;
        //print("Max: " + v0 * v0 * Mathf.Pow(Mathf.Sin(angle), 2) / 2 * g);
        while (t < time)
        {
            //Havada süzülürken dönme için
            transform.Rotate(Vector3.right * 2);
            //Burdan kutuya doðru bir eðim oluþturmayý dene sinüs cos falan deneme amaçlý yap bi
            float x = xStartPos + v0 * t * Mathf.Cos(angle);
            float y = yStartPos + v0 * t * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(t, 2);

            transform.position = new Vector3(x, y, transform.position.z);
            t += Time.deltaTime * speed * 0.1f * speedBooster;
            speedBooster += Time.deltaTime * 0.6f;

            rb.velocity = Vector3.zero;

            yield return null;
        }

        gameObject.SetActive(false);
        PoolingManager.PushCurrency(this);
    }

    private void DisableRigidbody()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("CurrencyGoBag");
        //GetComponent<Collider>().enabled = false;
    }



    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Safe"))
    //    {
    //        if (coroutineGoToBag != null)
    //            StopCoroutine(coroutineGoToBag);
    //    }
    //}



}
