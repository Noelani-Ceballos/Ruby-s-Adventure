using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
 
   public AudioClip collectedClip;
   public ParticleSystem particles;
   void OnTriggerEnter2D(Collider2D other)
   {
      RubyController controller = other.GetComponent<RubyController>();
      if (controller != null)
      {
         if(controller.health < controller.maxHealth)
         {
             controller.ChangeHealth(2);
             particles.Play();
            Destroy(gameObject, 0.6f);

            controller.PlaySound(collectedClip);
         }
      
      }

   }


       


}

