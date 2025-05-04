using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Entity.Component
{
    public abstract class EntityFeedbackBaseComponent : MonoBehaviour
    {
       [SerializeField] private MMF_Player _damageFeedback;
       [SerializeField] private MMF_Player _debuffFeedback;
       [SerializeField] private MMF_Player _statBonusFeedback;
       [SerializeField] private MMF_Player _healFeedback;
       
       public async UniTask PlayDamageFeedback(int damage)
       {
           await PlayFloatingTextFeedback(_damageFeedback,$"-{damage}");
       }
       
       public async UniTask PlayHealFeedback(int amount)
       {
           await PlayFloatingTextFeedback(_healFeedback,$"+{amount} HP");
       }
       
       public async UniTask PlayDebuffFeedback(string text)
       {
           await PlayFloatingTextFeedback(_debuffFeedback,text);
       }
       
       public async UniTask PlayStatBonusFeedback(string text)
       {
           await PlayFloatingTextFeedback(_statBonusFeedback,text);
       }
       
       
       private async UniTask PlayFloatingTextFeedback(MMF_Player feedback,string text)
       {
           MMF_FloatingText floatingText = feedback.GetFeedbackOfType<MMF_FloatingText>(); 
           floatingText.Value = text;
           feedback.PlayFeedbacks();
           while (feedback.IsPlaying)
               await UniTask.Yield();
       }
     
    }
}