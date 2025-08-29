using Unity.VisualScripting;
using UnityEngine;

public class Boss1HpHandelr : MonoBehaviour
{
    private Preston _boss1;
    private float _initialHp, _currentHp;
    private bool _page2;

    public bool Page2 => _page2;

    public void Initialize(Preston boss1, float Hp)
    {
        _boss1 = boss1;
        _initialHp = Hp;
        _currentHp = _initialHp;
    }

    public void Damaged(float attackPower)
    {

    }

    
    public void start2Page()
    {
        if(_currentHp <= _initialHp / 2)
        {
            _page2 = true;
        }
    }
}
