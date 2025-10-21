using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.UI; 
using System.Linq; 

public class ExplosionController : MonoBehaviour
{
    
    [Header("Referencias de UI")]
    [Tooltip("Arrastra el componente Button de tu UI aquí.")]
    [SerializeField] private Button explosionButton;
    [Tooltip("Arrastra el componente TextMeshProUGUI del texto del botón aquí.")]
    [SerializeField] private TextMeshProUGUI buttonText;
    
    
    [Header("Raíz del personaje (opcional)")]
    [SerializeField] private Transform root;              
    [Header("Parámetros de Explosión")]
    [SerializeField] private float explosionForce = 6f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float upwardsModifier = 0.25f;
    [SerializeField] private float randomTorque = 50f;
    [SerializeField] private bool addColliderIfMissing = true;
    [SerializeField] private bool detachToWorld = true;  


    private struct Part
    {
        public Transform t;
        public Transform parent;
        public Vector3 localPos;
        public Quaternion localRot;
        public Vector3 localScale;
        public Rigidbody rb;
        public bool rbAdded;
        public Collider col;
        public bool colAdded;
    }

    private List<Part> parts;
    private bool exploded = false; 

    void Awake()
    {
        if (root == null) root = transform;
        CacheParts();
    }
    
    void Start()
    {
        
        if (explosionButton != null)
        {
            
            explosionButton.onClick.AddListener(DisassembleAndControlUI);
        }
        else
        {
             Debug.LogError("¡El botón de explosión no está asignado en el Inspector!");
        }
    }

    private void CacheParts()
    {
        parts = new List<Part>(32);
        foreach (var t in root.GetComponentsInChildren<Transform>(includeInactive: true))
        {
            if (t == root) continue;
            parts.Add(new Part
            {
                t = t,
                parent = t.parent,
                localPos = t.localPosition,
                localRot = t.localRotation,
                localScale = t.localScale,
                rb = t.GetComponent<Rigidbody>(),
                col = t.GetComponent<Collider>()
            });
        }
    }
    
    public void DisassembleAndControlUI()
    {
        if (exploded) return; 
        
        
        Disassemble(); 
        exploded = true;
        
        
        if (buttonText != null)
        {
            buttonText.text = "KABOOM!!";
        }
        if (explosionButton != null)
        {
            explosionButton.interactable = false; 
        }
    }

    public void Disassemble()
    {
        if (parts == null || parts.Count == 0) CacheParts();
        Vector3 center = root.position;
        Transform newParent = detachToWorld ? root.parent : root;

        foreach (var i in Enumerable.Range(0, parts.Count))
        {
            var p = parts[i];

            
            if (p.col == null && addColliderIfMissing)
            {
                p.col = p.t.gameObject.AddComponent<BoxCollider>();
                p.colAdded = true;
            }
            if (p.rb == null)
            {
                p.rb = p.t.gameObject.AddComponent<Rigidbody>();
                p.rbAdded = true;
            }
            
            if (p.rb != null)
            {
                if (detachToWorld) p.t.SetParent(newParent, true);

                p.rb.isKinematic = false;
                p.rb.velocity = Vector3.zero;
                p.rb.angularVelocity = Vector3.zero;

                p.rb.AddExplosionForce(explosionForce, center, explosionRadius, upwardsModifier, ForceMode.Impulse);

                Vector3 torque = new Vector3(
                    Random.Range(-randomTorque, randomTorque),
                    Random.Range(-randomTorque, randomTorque),
                    Random.Range(-randomTorque, randomTorque)
                );
                p.rb.AddTorque(torque, ForceMode.Impulse);
            }
            
            parts[i] = p; 
        }
    }

    
    public void Reassemble()
    {
        if (!exploded || parts == null) return;
        
        for (int i = 0; i < parts.Count; i++)
        {
            var p = parts[i];

            if (p.rb != null)
            {
                p.rb.velocity = Vector3.zero;
                p.rb.angularVelocity = Vector3.zero;
                p.rb.isKinematic = true;
            }

            p.t.SetParent(p.parent, false);
            p.t.localPosition = p.localPos;
            p.t.localRotation = p.localRot;
            p.t.localScale   = p.localScale;

            if (p.rbAdded && p.rb != null) Destroy(p.rb);
            if (p.colAdded && p.col != null) Destroy(p.col);

            p.rb = null; p.col = null;
            p.rbAdded = false; p.colAdded = false;

            parts[i] = p;
        }
        
        exploded = false;
        
        if (explosionButton != null)
        {
            explosionButton.interactable = true; 
        }
    }
}