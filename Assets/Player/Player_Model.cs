public class Player_Model
{
    //�÷��̾� ���� ���� 
    public bool canDash = true;         // �뽬���ɿ���(��Ÿ�� ��)
    public bool isGrounded = false;     // ���� ��Ҵ��� ����
    public bool isWalking = false;      // �ȱ���
    public bool isDashing = false;      // �뽬��
    public bool isJumping = false;      // ������
    public bool isFalling = false;      // �������� �� 
    public bool isHit = false;          // �ǰݴ��� 
    public bool inCinematic = false;    // ������
    public bool inGearSetting = false;  // ���� ������
    public bool inGearAction = false;   // ���� �ߵ��� 
    public bool inUIControl = false;    // UI ������ 
}
