using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class HelloWorldPlayer : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    private string nickName;
    private float speed = 5f;
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            //transform.parent.gameObject
            Move();
            
        }
    }

    public void Move()
    {
        //SubmitPositionRequestServerRpc();
        InitializePositionRpc();
    }

    [Rpc(SendTo.Server)]
    void SubmitPositionRequestServerRpc(RpcParams rpcParams = default)
    {
        var randomPosition = GetRandomPositionOnPlane();
        transform.position = randomPosition;
        Position.Value = randomPosition;
    }

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }

    void Update()
    {
        if (!IsOwner) { return; }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        //movementDirection.Normalize();
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
    }

    //[Rpc(SendTo.Server)]
    //private void SendMovementInputRpc()
    //{
    //    Position.Value = transform.position;
    //}

    [Rpc(SendTo.Server)]
    private void InitializePositionRpc()
    {
        Position.Value = new Vector3(0, 1, 0);
        transform.position = Position.Value;
    }
}