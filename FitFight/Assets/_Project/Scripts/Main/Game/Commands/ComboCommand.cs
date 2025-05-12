using System;
using System.Collections.Generic;
using _Project.Scripts.Main.Game.Commands.Base;
using _Project.Scripts.Main.Game.Entity;
using _Project.Scripts.Main.Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Commands
{
    //Note: no proper way of setting selected target yet
    public class ComboCommand : BaseCommand
    {
        [SerializeReference] private ICommand[] _commands;
        
        public override async UniTask ExecuteImmediate()
        {
            if(_commands == null)
                throw new NullReferenceException($"{GetType().Name}: Commands should be assigned first");
            
            await base.ExecuteImmediate();
            Player player = EntityManager.Instance.PlayerEntity;
            List<Enemy> targets = EntityManager.Instance.Enemies;
            
            foreach (var command in _commands)
            {
                command.SetPlayer(player);
                if (command is EnemyTargetBaseCommand targetCommand)
                {
                    targetCommand.SetPriority(-1);
                    targetCommand.SetTargets(targets);
                }
                await command.ExecuteImmediate();
            }
        }
    }
}