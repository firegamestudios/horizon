using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    // Cria uma layer chamada Player, o n�mero dela vem aqui
    private int playerLayer;

    private void Start()
    {
        // pega o n�mero da Layer pelo nome dela
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            Debug.Log("Player entrou no trigger!");
            
        }
    }
}
