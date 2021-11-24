using UnityEngine;

public class CharacterUIHolder : MonoBehaviour
{
    [SerializeField] CharacterBody2D character;

    private void Awake()
    {
        ActivateCharacter();
    }

    private void Start()
    {
        FixCharacterPosition();
    }

    private void FixCharacterPosition()
    {
        Vector3 worldPos = transform.TransformPoint(Vector3.zero);
        character.transform.position = worldPos;
    }

    public void ActivateCharacter()
    {
        character.gameObject.SetActive(true);
    }

    public void DeactivateCharacter()
    {
        character.gameObject.SetActive(false);
    }
}
