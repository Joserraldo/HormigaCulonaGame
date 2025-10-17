using UnityEngine;

public class Pj : MonoBehaviour
{
    public float velocidad = 5f; 

    void Update()
    {
        float movimientoX = Input.GetAxis("Horizontal") * velocidad * Time.deltaTime;
        float movimientoZ = Input.GetAxis("Vertical") * velocidad * Time.deltaTime;

        transform.Translate(movimientoX, 0, movimientoZ);
    }
}
