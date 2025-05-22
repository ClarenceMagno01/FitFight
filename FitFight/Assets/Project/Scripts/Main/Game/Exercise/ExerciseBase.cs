using System;

namespace _Project.Scripts.Main.Game.Exercise
{
    public abstract class ExerciseBase : IExercise
    {
        protected int counter; // counter for required valid movements to complete 1 rep

        public virtual void Restart()
        {
            counter = 0;
        }
        public abstract void Run(Action onRep);
        
    }
}