using System;

namespace _Project.Scripts.Main.Game.Exercise
{
    public interface IExercise
    {
        void Restart();
        void Run(Action onRep);
    }
}