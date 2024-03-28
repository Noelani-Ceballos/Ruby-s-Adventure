using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
  public float speed = 3.0f;

   public int maxHealth = 5;

   public float timeInvincible = 2.0f;
    
    public int health { get { return currentHealth; }}
   
    public int currentHealth;

    bool isInvincible;
    
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal; 
    float vertical;
    
    public GameObject projectilePrefab; 

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

     AudioSource audioSource;

     public AudioClip walkingSound;
     public AudioClip projectileSound;
     
     public AudioClip damageSound;
    
    public AudioClip collectibleSound;
    public bool audioPlay;

    
    // Start is called before the first frame update
    void Start()
    {
       rigidbody2d = GetComponent<Rigidbody2D>(); 
       animator = GetComponent<Animator>(); 
       currentHealth = maxHealth;
      
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }


    // Update is called once per frame
    void Update()
    {
      //Checks for left/right keys//
      horizontal = Input.GetAxis("Horizontal");
      vertical = Input.GetAxis("Vertical");

      Vector2 move = new Vector2(horizontal, vertical);
      if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
      {
        //PlaySound(walkingSound);
        lookDirection.Set(move.x, move.y);
        lookDirection.Normalize();
      }

      animator.SetFloat("Look X", lookDirection.x);
      animator.SetFloat("Look Y", lookDirection.y);
      animator.SetFloat("Speed", move.magnitude);
    
    if (horizontal != 0 || vertical != 0)
    {
        
        audioPlay = true;
    }
    else
    {
        audioPlay = false;
    }
    
    if(audioPlay == true && !audioSource.isPlaying)
    {
        PlaySound(walkingSound);
    }
    
    if(audioPlay == false)
    {
        audioSource.Stop();
    }

    

      if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }


        //Launching Projectile//
         if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
    
        if (Input.GetKeyDown(KeyCode.X))
        {
        
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            
            }

        
        }
    }




     void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        
        rigidbody2d.MovePosition(position);
    
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible) return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(damageSound);
         }

        PlaySound(collectibleSound);
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    
        
      


     void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        
        Projectiles projectile = projectileObject.GetComponent<Projectiles>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(projectileSound);
    
    }
    void OnTriggerEnter2D(Collider2D other)
    {
         NonPlayerCharacter character = other.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            
    }
   
}

    
    
