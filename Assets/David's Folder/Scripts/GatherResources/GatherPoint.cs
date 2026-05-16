using UnityEngine;

namespace Tides.Resources
{
    public class GatherPoint
    {
        public IResource[] Resources { get; private set; }

        public GatherPoint(IResource[] resources)
        {
            Resources = resources;
        }
    }
}