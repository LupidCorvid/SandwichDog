using UnityEngine;

public class SandwichBase : MonoBehaviour
{
    [SerializeField] private Food food;
    public Food Food => food;

    private void Awake()
    {
        ChefManager.Instance.snapTargets.Add(this.food);
    }

    private void FixedUpdate()
    {
        
    }
}
