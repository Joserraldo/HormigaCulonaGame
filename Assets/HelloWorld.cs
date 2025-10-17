using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HelloWorld : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    
    // Start is called before the first frame update
    void Start()
    {
        if(button != null){
            button.onClick.AddListener(()=> ChangeTextHW("Hola Mundo!!"));
        }
    }

    // Update is called once per frame
    public void ChangeTextHW(string newText)
    {
     if(textMeshPro != null){
        textMeshPro.text = newText;
     }
     }   
}


