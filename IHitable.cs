using System;


interface IHitable
{
    int Health { get; set; }
    void GetDamaged();

    bool CheckOnHit(int firstCoord, int secondCoord);
}

