using System.Collections;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [Header("Material Types")]
    [SerializeField] MeshRenderer[] _mRender;
    public Material baseMat;
    public Material hitFlashMaterial;

    [Header("Enemy Health Count")]
    public int enemyHealth = 5;
        
    private void Start()
    {
        foreach(MeshRenderer m in _mRender)
        {
            m.material = baseMat;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            Destroy(other.gameObject);
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        enemyHealth--;
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            foreach (MeshRenderer m in _mRender)
            {
                m.material = hitFlashMaterial;
            }
            yield return new WaitForSeconds(0.1f);

            foreach (MeshRenderer m in _mRender)
            {
                m.material = baseMat;
            }
        }
        
    }
}
