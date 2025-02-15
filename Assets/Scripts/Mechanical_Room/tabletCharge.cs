using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI; 

public class tabletCharge : MonoBehaviour
{
  [SerializeField] float currentTabletCharge = 0.0f;

  [SerializeField] const float maxCharge = 100.0f;

  [SerializeField] float chargeRate = 0.5f;

  [SerializeField] int delayTime = 100;

  [SerializeField] bool tabletIsOnCharger = false;
  
  public Text displayValue;


  void Start()
  {
    if (tabletIsOnCharger)
    {
      while (currentTabletCharge < maxCharge)
        {
            currentTabletCharge += chargeRate;
            if (currentTabletCharge > maxCharge)
            {
              currentTabletCharge = maxCharge; // Ensure it doesn't go over 100
            } 
            
            // Wait for a short amount of time before increasing the charge again
            Thread.Sleep(delayTime);

            displayValue.text = currentTabletCharge.ToString();
        }

    }
  }




}
