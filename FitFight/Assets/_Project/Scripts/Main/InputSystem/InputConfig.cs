using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Main.InputSystem
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "InputConfig/InputConfig")]
    public class InputConfig : ScriptableObject
    {
        public bool isRingconAndGamepadEnabled;
    }
}