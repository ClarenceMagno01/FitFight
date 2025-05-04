using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Main.Game.Entity
{
    public interface IEntity
    {
        UniTask TakeDamage(int damage);
    }
}