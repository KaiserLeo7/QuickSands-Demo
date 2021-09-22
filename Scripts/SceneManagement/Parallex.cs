using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//COMMENTED BY FARAMARZ HOSSEINI


public class Parallex : MonoBehaviour {

    
    private Material material;
    private Vector2 offset;
    [SerializeField] private float xVelocity, yVelocity;
    private static bool shouldMove = false;                 //if true the material moves and repeats

    void Awake() {
        material = GetComponent<Renderer>().material;
    }

    private void Start()
    {
        //preventing the creation of a save file in the main menu
        if (File.Exists(Application.persistentDataPath + "/player.savefile"))
        {
            Sands.Player.LoadPlayer();
            if (!Sands.Player.HasVehicle)
                xVelocity -= 0.15f;
        }

        offset = new Vector2(xVelocity, yVelocity);
    }

    void Update() {
        //moves the material by offset amount
        if (ShouldMove)
        {
            material.mainTextureOffset += offset * Time.deltaTime;
        }
    }

    public static bool ShouldMove
    {
        get
        {
            return shouldMove;
        }
        set
        {
            shouldMove = value;
        }
    }

    private void OnDestroy()
    {
        shouldMove = false;
    }
}
