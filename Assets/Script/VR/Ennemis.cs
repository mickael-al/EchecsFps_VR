using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChessVR
{
    public class Ennemis : MonoBehaviour
    {
        [SerializeField] private float life = 100.0f;
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private float projectileRateSpawn = 0.4f;        
        [SerializeField] private List<GameObject> projectilePrefab;
        private BoxCollider boundsCollider;        
        private float minX, maxX, minZ, maxZ;
        private Vector3 direction;
        private float speed;
        private GameObject targetPlayer;
        private float timeUntilNextProjectile = 0;
        
        public void Setup(GameObject target,float moveS,float projectileRs,float lifeP)
        {
            targetPlayer = target;
            speed = moveS;
            moveSpeed = moveS;
            projectileRateSpawn = projectileRs;
            life = lifeP;
        }
        
        private void Start()
        {
            boundsCollider = GetComponentInParent<BoxCollider>();
            CalculateBounds();
            direction = new Vector3(Random.Range(0.0f, 1.0f),0.0f,Random.Range(0.0f, 1.0f));
            speed = moveSpeed;
        }

        private void CalculateBounds()
        {
            minX = boundsCollider.transform.position.x + boundsCollider.center.x - boundsCollider.size.x * 0.5f;
            maxX = boundsCollider.transform.position.x + boundsCollider.center.x + boundsCollider.size.x * 0.5f;
            minZ = boundsCollider.transform.position.z + boundsCollider.center.z - boundsCollider.size.z * 0.5f;
            maxZ = boundsCollider.transform.position.z + boundsCollider.center.z + boundsCollider.size.z * 0.5f;
        }

        public void addDegat(float degat)
        {
            life -= degat;
            if(life <= 0)
            {
                Destroy(boundsCollider.transform.gameObject);
            }
        }

        private void Update()
        {
            transform.position += direction * speed * Time.deltaTime;

            if(transform.position.x > maxX || transform.position.x < minX )
            {
                direction.x = -direction.x;
                transform.position = new Vector3(transform.position.x > maxX ? maxX : minX,transform.position.y,transform.position.z);
            }
            if(transform.position.z > maxZ || transform.position.z < minZ)
            {
                direction.z = -direction.z;
                transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z > maxZ ? maxZ : minZ);
            }

            timeUntilNextProjectile -= Time.deltaTime;
            if (timeUntilNextProjectile <= 0 && targetPlayer != null)
            {
                Vector3 directionToPlayer = (targetPlayer.transform.position - transform.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab[Random.Range(0,projectilePrefab.Count)], transform.position+new Vector3(0,1.0f,0), Quaternion.LookRotation(directionToPlayer));   
                timeUntilNextProjectile = projectileRateSpawn;
            }
        }
    }
}