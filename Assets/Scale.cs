
using UnityEngine;

public class Scale : MonoBehaviour
{
    [SerializeField] public HelloWorld helloWorldScript;
    [SerializeField] private Transform ObjectToScale;
    [SerializeField] private float scaleSpeed = 0.1f;
    [SerializeField] private float minscale = 0.005f;

    private string ScaleText = "Escalado a:";
    private string WarningText = "No se puede hacer más pequeño";    
    
    // Start is called before the first frame update
    void Start()
    {
        helloWorldScript.ChangeTextHW("Empty");
    }

    // Update is called once per frame
    public void ScaleUp()
    {
        ObjectToScale.localScale += Vector3.one * scaleSpeed;
        helloWorldScript.ChangeTextHW(ScaleText + ObjectToScale.localScale.ToString("F2"));
    }
    public void ScaleDown()
    {
        if (ObjectToScale.localScale.x > minscale)
        {
            ObjectToScale.localScale -= Vector3.one * scaleSpeed;
            helloWorldScript.ChangeTextHW(ScaleText + ObjectToScale.localScale.ToString("F2"));
        }
        else
        {
            helloWorldScript.ChangeTextHW(WarningText);
        }
    }
}
