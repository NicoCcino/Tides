using UnityEngine;

[CreateAssetMenu(fileName = "TideCyclesSO", menuName = "Scriptable Objects/TideCyclesSO")]
public class TideCyclesSO : ScriptableObject
{
    [field: SerializeField] public TideCycle[] tideCycles { get; private set; }

}
