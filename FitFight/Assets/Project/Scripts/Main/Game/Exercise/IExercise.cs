using System;

namespace _Project.Scripts.Main.Game.Exercise
{
    public interface IExercise
    {
        int Counter { get; }
        void Restart();
        void Run(Action onRep);
    }
}