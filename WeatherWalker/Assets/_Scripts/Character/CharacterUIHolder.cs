using UnityEngine;

public class CharacterUIHolder : MonoBehaviour
{
    [SerializeField] CharacterBody2D character;

    private void Awake()
    {
        character.gameObject.SetActive(true);
    }

    private void Start()
    {
        FixCharacterPosition();
    }

    private void FixCharacterPosition()
    {
        Vector3 worldPos = transform.TransformPoint(Vector3.zero);
        Debug.Log(worldPos);
        character.transform.position = worldPos; // Camera.main.ScreenToWorldPoint(worldPos);
    }
}
