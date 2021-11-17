using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private CharacterBody2D character;
    [SerializeField] private CharacterGenerator2D generator;

    public void Random()
    {
        generator.Generate(character);
    }
}
