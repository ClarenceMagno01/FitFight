using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Entity.Component
{
    public class EnemyFeedbackBaseComponent : EntityFeedbackBaseComponent
    {
        [SerializeField] private MMF_Player _attackFeedback;
    
        public async UniTask PlayAttackFeedback()
        {
            _attackFeedback.PlayFeedbacks();
            while (_attackFeedback.IsPlaying)
                await UniTask.Yield();
        }
        
        public void ResumeAttackFeedbacks()
        {
            _attackFeedback.ResumeFeedbacks();
        }
        
    }
}