
using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour{
     
   public float health = 50f;
   public float RagdollTime = 2f;

   void Start()
   {

   }

   public void TakeDamage (float amount)
   {
       health -= amount;
       if (health <= 0f)
       {
           //ragdoll
          StartCoroutine(Die());
       }
   }

   IEnumerator Die ()
   {
        yield return new WaitForSeconds(RagdollTime);
        Destroy(gameObject);
   }

    
   
}
