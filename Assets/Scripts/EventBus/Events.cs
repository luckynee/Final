using System;
using PokerSeed.Garden;

namespace EventBus
{
    public interface IEvent
    {
        
    }
    
    public struct OnSfxVolumeChange : IEvent
    {
        public float Volume;
        public OnSfxVolumeChange(float volume)
        {
            Volume = volume;
        }
    }
    
    public struct OnBgmVolumeChange : IEvent
    {
        public float Volume;
        public OnBgmVolumeChange(float volume)
        {
            
            Volume = volume;
        }
    }
    
    public struct OnSfxTrigger : IEvent
    {
        public Enums.SFXType SfxType;
        public OnSfxTrigger(Enums.SFXType sfxType)
        {
            SfxType = sfxType;
        }
    }
    
    public struct OnAmbienceTrigger : IEvent
    {
        public Enums.AmbienceType AmbienceType;
        public OnAmbienceTrigger(Enums.AmbienceType ambienceType)
        {
            AmbienceType = ambienceType;
        }
    }
    
    public struct MainMenuInit : IEvent
    {
        
    }

    public struct CreditBtnAddListenerEvent : IEvent
    {
        public Action delegates;

        public CreditBtnAddListenerEvent(Action delegates)
        {
            this.delegates = delegates;
        }
    }

    public struct PointInfoBtnAddListenerEvent : IEvent
    {
        public Action delegates;
        
        public PointInfoBtnAddListenerEvent(Action delegates)
        {
            this.delegates = delegates;
        }
    }

    public struct FailurePopUpAddEventListenerEvent : IEvent
    {
        public Action delegates;

        public FailurePopUpAddEventListenerEvent(Action delegates)
        {
            this.delegates = delegates;
        }
    }
    
    public struct SettingBtnAddListenerEvent : IEvent
    {
        public Action delegates;

        public SettingBtnAddListenerEvent(Action delegates)
        {
            this.delegates = delegates;
        }
    }

    public struct PlantInfoPopUpInit : IEvent
    {
        public PlantData PlantData;
        public Enums.PlantColor PlantColor;
        
        public PlantInfoPopUpInit(PlantData plantData, Enums.PlantColor plantColor)
        {
            PlantData = plantData;
            PlantColor = plantColor;
        }
    }
    
    public struct PlantBtnAddListenerEvent : IEvent
    {
        public Action delegates;

        public PlantBtnAddListenerEvent(Action delegates)
        {
            this.delegates = delegates;
        }
    }
    
    public struct OnAmbienceStop : IEvent
    {
        public Enums.AmbienceType AmbienceType;
        public OnAmbienceStop(Enums.AmbienceType ambienceType)
        {
            AmbienceType = ambienceType;
        }
    }
    
    public struct OnBgmStop : IEvent
    {
        public Enums.AmbienceType AmbienceType;
        public OnBgmStop(Enums.AmbienceType ambienceType)
        {
            AmbienceType = ambienceType;
        }
    }
    
    public struct OnGameRoundStart : IEvent
    {
        
    }
}