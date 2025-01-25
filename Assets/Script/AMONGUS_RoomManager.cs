using UnityEngine;
using Mirror;

public class AMONGUS_RoomManager : NetworkRoomManager
{
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)//�������� ���� ������ Ŭ���̾�Ʈ�� ���� �� �����ϴ� �Լ� 
    {
        base.OnRoomServerConnect(conn);

        var playerObject = Instantiate(spawnPrefabs[0]); //spawnPrefabs -> �̸� ����� ���� ��Ʈ��ũ ������ ����Ʈ. 

        NetworkServer.Spawn(playerObject, conn);

        //var networkIdentity = playerObject.GetComponent<NetworkIdentity>();

        //networkIdentity.RemoveClientAuthority();

        //bool isAuthority = networkIdentity.AssignClientAuthority(conn); //NetworkIdentity�� ���� ������ �����ο�.

        //NetworkServer -> �������� ��Ʈ��ũ ������ �����ϴ� ������ ��.
        //���������� ������Ʈ ����, ����ȭ, �޽��� ó������ �ٷ�.
        //������Ʈ ����ȭ = �������� ������ ������Ʈ�� Ŭ���̾�Ʈ�� ����ȭ��. ��� ����� Ŭ���̾�Ʈ�� ������ ������Ʈ�� �ν��� �� �ֵ��� ����.
        //Ŭ���̾�Ʈ ���� = Ŭ���̾�Ʈ ���� �� ���� ó��. Ŭ���̾�Ʈ�� ������ Ư�� ������Ʈ�� ����(���Ѻο�)�� �� �ֵ��� ����.
        //������ ���� = Ŭ���̾�Ʈ�� �����͸� �ְ�ޱ� ���� �޽����� ó��.

        //NetworkServer.Spawn(���� ������Ʈ. NetworkConnection) 
        //��Ʈ��ũ ������Ʈ�� �����ϰ�, �̸� ������ Ŭ���̾�Ʈ ���� ����ȭ�ϴ� ������ �ϴ� �Լ�.
        //������Ʈ ��� -> �������� ������ ������Ʈ�� ��Ʈ��ũ�� ���.
        //Ŭ���̾�Ʈ�� ���� -> ����� Ŭ���̾�Ʈ���� ������ ������Ʈ�� ������ ������ Ŭ���̾�Ʈ�� �ش� ������Ʈ�� ������ �� �� �ֵ��� ��.
        //���� ���� -> Ư�� Ŭ���̾�Ʈ���� ������Ʈ�� ������(����)�� �ο��� �� ����.
    }
}
