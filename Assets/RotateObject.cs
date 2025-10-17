
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private bool clockwise = true;
    [SerializeField] public HelloWorld helloWorldScript;
    // Start is called before the first frame update
    private bool isRotating = false;

    void Update()
    {
        if(isRotating)
        {
            float direction = clockwise ? 1f : -1f;
            objectToRotate.Rotate(Vector3.up, direction * rotationSpeed * Time.deltaTime);
            Debug.Log("Rotando"+objectToRotate.name);
        }   
    }
    
    public void ToggleRotation()
    {
        isRotating = !isRotating;
        if(helloWorldScript != null)
        {
            if (isRotating)
            {
                helloWorldScript.ChangeTextHW("Rotando objeto");
            }
            else
            {
                helloWorldScript.ChangeTextHW("Rotacion detenida");
            }
        }
    }
}
