using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Light2D lightingComponent;

    private float timer = 0;
    void Start()
    {
        lightingComponent = gameObject.GetComponent<Light2D>();
    }

    private void FixedUpdate() {
        if (timer > .15){
            lightingComponent.intensity = Random.Range(.75f, .95f);
            timer = 0;
        }
        timer += Time.fixedDeltaTime;
    }

}
