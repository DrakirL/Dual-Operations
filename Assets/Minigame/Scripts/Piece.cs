using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool active;
    [Header("Connection type")]   
    public bool startConnector;
    public bool endConnector;
    public bool firewall;
    public Sprite defaultSprite;
    public Sprite activeSprite;

    public SpriteRenderer sprite;

    [Header("Rotation")]
    [SerializeField] bool rotation;
    [SerializeField] float rotateSpeed;
    float startRot;
    [Space(10)]

    public bool input = true;

    [Header("*Values will rotate automatically if piece has been rotated in editor*")]
    public int [] values;
    public int [] startValues;

    float realRotation;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        // Rotates the values in runtime on objects that is rotated in editor
        int mod = Mathf.RoundToInt(transform.rotation.eulerAngles.z / 90);
        if(mod > 0)
            for (int i = 0; i < mod; i++)
            {
                RotateValuesLeft();
            }
       else
            for (int i = 0; i > mod; i--)
            {
                RotateValuesRight();
            }      
    }

    private void Start()
    {
        if(defaultSprite != null)
            sprite.sprite = defaultSprite;
        realRotation = transform.rotation.eulerAngles.z;
        startRot = transform.rotation.eulerAngles.z;
        startValues = new int[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            startValues[i] = values[i];
        }
    }

    void Update()       
    {
        if (active)
        {
            if (activeSprite != null)
                sprite.sprite = activeSprite;
        }
        else
        {
            if (defaultSprite != null)
                sprite.sprite = defaultSprite;
        }
                 
        if (transform.rotation.eulerAngles.z != realRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,realRotation), rotateSpeed / 10);
        }
        
    }

    private void OnMouseOver()
    {
        if (input)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && rotation)
                DualOperationsAudioPlayer.Instance.Hack(0);

            // Rotate left on left mouse click
            if (Input.GetMouseButtonDown(0) && rotation)
                RotateLeft();
            // Rotate right on right mouse click
            if (Input.GetMouseButtonDown(1) && rotation)
                RotateRight();
        }                    
    }

    public void RotateLeft()
    {
        realRotation += 90;

        // Convert to eulerAngles value
        if(realRotation == 360)
            realRotation = 0;

        RotateValuesLeft();
    }
    public void RotateRight()
    {
        realRotation -= 90;

        // Convert to eulerAngles value
        if (realRotation == 0)
            realRotation = 360;

        RotateValuesRight();
    }
    // Rotate array values representing the sides of the box
    public void RotateValuesLeft()
    {
        int temp = values[0];
        for (int i = 0; i < values.Length-1; i++)
        {
            values[i] = values[i + 1];
        }
        values[3] = temp;
    }
    public void RotateValuesRight()
    {
        int temp = values[3];
        for (int i = values.Length - 1; i > 0; i--)
        {
            values[i] = values[i - 1];
        }
        values[0] = temp;
    }

    public void GetStartPos()
    {
        realRotation = startRot;
        for (int i = 0; i < startValues.Length; i++)
        {
            values[i] = startValues[i];
        }
    }
}
