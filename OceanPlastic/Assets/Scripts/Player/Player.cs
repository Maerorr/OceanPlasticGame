using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float currentOxygen = 100f;
    private float oxygenDecreaseRate = 5f;
    private float oxygenIncreaseRate = 20f;

    private bool isInWater = true;
    
    [SerializeField]
    private Slider oxygenSlider;
    
    private Coroutine oxygenCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        oxygenCoroutine = StartCoroutine(HandleOxygen());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HandleOxygen()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            if (isInWater)
            {
                UpdateOxygen(-oxygenDecreaseRate / 2f);
            }
            else
            {
                UpdateOxygen(oxygenIncreaseRate / 2f);
            }
        }
    }

    private void UpdateOxygen(float delta)
    {
        currentOxygen += delta;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, 100f);
        oxygenSlider.value = currentOxygen;
    }
    
    public void SetIsInWater(bool value)
    {
        isInWater = value;
    }

    public bool IsInWater()
    {
        return isInWater;
    }
}
