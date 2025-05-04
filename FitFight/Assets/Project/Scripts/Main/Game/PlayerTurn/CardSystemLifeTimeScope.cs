using _Project.Scripts.Main.Game.Entity;
using _Project.Scripts.Main.Game.PlayerTurn.Systems;
using _Project.Scripts.Main.InputSystem;
using _Project.Scripts.Main.Managers;
using _Project.Scripts.Main.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Main.Game.PlayerTurn
{
    public class CardSystemLifeTimeScope  : LifetimeScope
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private HeldCards _heldCards;
        [SerializeField] private TargetSystem _targetSystem;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            builder.UseComponents(components =>
            {
                components.AddInstance(_heldCards);
                components.AddInstance(_inputReader);
                components.AddInstance(_targetSystem);
            });
            
             builder.RegisterEntryPoint<CardSystemController>(Lifetime.Scoped);
        }
    }
}