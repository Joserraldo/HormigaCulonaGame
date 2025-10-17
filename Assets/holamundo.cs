using UnityEngine;

public class Holamundo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        
        }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.Space)){
        Debug.Log("Hola Mundo");
     }   
    }
}