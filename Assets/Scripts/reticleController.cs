using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reticleController : MonoBehaviour
{
/*
this class handles the reticle. it xommunicates with 2 static booleans in portalShooting
shotOrange, and shotBlue, which return which portal has been shot.
*/
    public Image reticle;

    public Sprite blue;
    public Sprite orange;
    public Sprite empty;
    public Sprite both;

   // public SpriteRenderer SR;

  
    // Start is called before the first frame update
    void Start()
    {
        reticle = GetComponent<Image>();
    
    }

    // Update is called once per frame
    void Update()
    {
        //if only a blue portal is out
        if (portalShooting1.shotBlue && !portalShooting1.shotOrange)
        {
            reticle.sprite = blue;
           
        }

        if (portalShooting1.shotOrange && !portalShooting1.shotBlue)
        {
            reticle.sprite = orange;
        }

        if (portalShooting1.shotBlue && portalShooting1.shotOrange)
        {
            reticle.sprite = both;
        }
        if(!(portalShooting.shotBlue) && (!portalShooting.shotOrange)
        {
            reticle.sprite = empty;
        }
    }
}
