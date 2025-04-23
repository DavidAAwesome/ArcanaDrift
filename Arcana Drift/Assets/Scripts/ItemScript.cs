using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public itemType type;

    public enum itemType
    {
        Health,
        Mana,
        TurboBoost
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Found player");
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            switch (type)
            {
                case itemType.Health:
                    pc.health = pc.maxHealth;
                    break;
                case itemType.Mana:
                    pc.mana = pc.maxMana;
                    break;
                case itemType.TurboBoost:
                    GameManager.Instance.UnlockAbility(GameManager.Abilities.TurboBoost);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
