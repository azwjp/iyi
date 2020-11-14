using UnityEngine;

namespace Iyi
{
    [RequireComponent(typeof(IyiServer))]
    public class AvaterAnimator : AvaterAnimatorBase
    {
        IyiServer server;

        protected override void Start()
        {
            base.Start();
            server = GetComponent<IyiServer>();
        }
        override protected Received GetFaceData()
        {
            return server.faceData;
        }
    }
}
