using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using UnityEngine;

public class TargetMovement : NetworkBehaviour
{
    public struct MoveData : IReplicateData
    {
        public Vector3 Direction;
        private uint _tick;
        public void Dispose() { }
        public uint GetTick() => _tick;
        public void SetTick(uint value) => _tick = value;
    }
    public struct ReconcileData : IReconcileData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector2 Velocity;
        public float AngularVelocity;
        public ReconcileData(Vector3 position, Quaternion rotation, Vector2 velocity, float angularVelocity)
        {
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            _tick = 0;
        }
        private uint _tick;
        public void Dispose() { }
        public uint GetTick() => _tick;
        public void SetTick(uint value) => _tick = value;
    }

    public Transform Target { get; set; }

    [SerializeField]
    private float _MoveSpeed;
    [SerializeField]
    private Rigidbody2D _Rigidbody2D;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        TimeManager.OnTick += TimeManager_OnTick;
        TimeManager.OnPostTick += TimeManager_OnPostTick;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        TimeManager.OnTick -= TimeManager_OnTick;
        TimeManager.OnPostTick -= TimeManager_OnPostTick;
    }

    private void TimeManager_OnTick()
    {
        Move(BuildActions());
    }
    private void TimeManager_OnPostTick()
    {
        if (IsServerStarted)
        {
            ReconcileData rd = new(transform.position, transform.rotation, _Rigidbody2D.velocity, _Rigidbody2D.angularVelocity);
            Reconciliation(rd);
        }
    }

    private MoveData BuildActions()
    {
        if (!IsServerStarted)
        {
            return default;
        }

        MoveData moveData = new()
        {
            Direction = Target.position - transform.position
        };
        return moveData;
    }

    [Replicate]
    private void Move(MoveData md, ReplicateState state = ReplicateState.Invalid, Channel channel = Channel.Unreliable)
    {
        _Rigidbody2D.velocity = md.Direction.normalized * _MoveSpeed;
    }

    [Reconcile]
    private void Reconciliation(ReconcileData rd, Channel channel = Channel.Unreliable)
    {
        transform.SetPositionAndRotation(rd.Position, rd.Rotation);
        _Rigidbody2D.velocity = rd.Velocity;
        _Rigidbody2D.angularVelocity = rd.AngularVelocity;
    }
}