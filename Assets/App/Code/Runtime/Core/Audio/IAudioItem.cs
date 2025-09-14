using UnityEngine;

namespace Assets.App.Code.Runtime.Core.Audio
{    
    public interface IAudioItem 
    {
        int Id {get;}  
        AudioClip[] Clips {get;}        
    }
}
