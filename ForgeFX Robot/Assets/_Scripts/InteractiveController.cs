using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created on 12/10/21 by Ken Vernon.
// Description: This script defines and manages an asset's interactability and how it reacts to end-user input.
// Updated on 12/11/21 as v0.1.0 by Ken Vernon.
// - Imported UnityEngine.UI library.
// - Created Text variable called 'interactText' with intention to initialize in the inspector.
// - Created Vector3 variables called 'startPos' and 'startRot' and initialized them in Start().
// - Created SetPosition().
// - Created GetScreenPosition().
// - Created GetSnapPosition().
// - Created bool variable called 'isAttached' and initialized it at declaration.
// - Removed Update() as it is not used.
// Updated on 12/12/21 as v0.2.0 by Ken Vernon.
// - Modified Start() to initialize 'colorChange' as 'true' if all 3 selectable bools start as 'false.'
// - Created Highlight().
// - Created GameObject array variable called 'interactiveObjs' with intention to initialize in the inspector.
// - Created Color array variable called 'ogColors' and initialized it in Start().
// - Created Material array variables called 'materials' and 'ogMats' and initialized them in Start().
// - Created Color variable called 'highlightColor' with intention to initialize in the inspector.
// - Created Material variable called 'highlightMat' with intention to initialize in the inspector.
// - Created OutlineController array variable called 'outlineScripts' with intention to initialize in the inspector.
// - Created enum variable called 'Highlights' and initialized it at declaration.
// - Created Highlights variable called 'highlightStyle' to allow manual selecting within the inspector.

public class InteractiveController : MonoBehaviour
{
    public enum Highlights { materialSwap, colorChange, outlineShader};
    [SerializeField]
    public Highlights highlightStyle;
    [Header("Material Swap Params")]
    [Header("Highlight Parameters")]
    [Tooltip("Use if highlighting action is performed through swapping materials on the interactive asset.")]
    public Material highlightMat;
    [Header("Color Change Params")]
    [Tooltip("Use if highlighting action is perfromed through changing the color multiplier on the interactive asset's material.")]
    public Color highlightColor;
    [Header("Outline Shader Params")]
    [Tooltip("Use if highlighting action is performed through triggering a custom shader's parameter on the interactive asset.")]
    public OutlineController[] outlineScripts;

    [Header("Universal Parameters")]
    public Text interactText;
    [Tooltip("Add all game objects that represent this interactive asset in the scene.")]
    public GameObject[] interactiveObjs;

    private Vector3 startPos, startRot, screenPos;
    private bool isAttached = true;
    private Color[] ogColors;
    private Material[] materials, ogMats;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation.eulerAngles;

        ogColors = new Color[interactiveObjs.Length];
        materials = new Material[ogColors.Length];

        for(int i = 0; i < ogColors.Length; i++)
        {
            materials[i] = interactiveObjs[i].GetComponent<MeshRenderer>().material;
            ogColors[i] = materials[i].color;
        }

        ogMats = materials;
    }

    public void SetPosition(Vector3 posValue)
    {
        if(Vector3.Distance(posValue, startPos) > 0.01f)
        {
            transform.position = posValue;
        }
        else
        {
            transform.position = startPos;
        }
    }

    public Vector3 GetScreenPosition()
    {
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        return screenPos;
    }

    public Vector3 GetSnapPosition()
    {
        return Camera.main.WorldToScreenPoint(startPos);
    }

    public void SetStatus(bool value)
    {
        isAttached = value;

        if(isAttached == false)
        {
            interactText.text = "Detatched";
        }
        else
        {
            interactText.text = "Attached";
        }
    }

    public bool GetStatus()
    {
        return isAttached;
    }

    public void Highlight(bool value)
    {
        if((int)highlightStyle == 0)
        {
            if (value == true)
            {
                // highlight
                for (int i = 0; i < materials.Length; i++)
                {
                    interactiveObjs[i].GetComponent<MeshRenderer>().material= highlightMat;
                }
            }
            else
            {
                // don't highlight
                for (int i = 0; i < materials.Length; i++)
                {
                    interactiveObjs[i].GetComponent<MeshRenderer>().material = ogMats[i];
                }
            }
        }
        else if ((int)highlightStyle == 1)
        {
            if (value == true)
            {
                // highlight
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].color = highlightColor;
                }
            }
            else
            {
                // don't highlight
                for (int i = 0; i < materials.Length; i++)
                {
                     materials[i].color = ogColors[i];
                }
            }
        }
        else if ((int)highlightStyle == 2)
        {
            if(outlineScripts != null)
            {
                if (value == true)
                {
                    // highlight
                    foreach(var script in outlineScripts)
                    {
                        script.OutlineWidth = 5f;
                    }
                }
                else
                {
                    // don't highlight
                    foreach (var script in outlineScripts)
                    {
                        script.OutlineWidth = 0f;
                    }
                }
            }
        }

    }
}
