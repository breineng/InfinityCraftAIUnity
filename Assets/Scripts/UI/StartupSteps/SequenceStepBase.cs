using System.Threading.Tasks;
using UnityEngine;

namespace InfinityCraft.UI.StartupSteps
{
    public abstract class SequenceStepBase : MonoBehaviour
    {
        public abstract Task Execute();
    }
}