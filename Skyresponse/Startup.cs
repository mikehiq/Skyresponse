﻿using Skyresponse.Api;
using Skyresponse.DialogWrappers;
using Skyresponse.Forms;
using Skyresponse.HttpWrappers;
using Skyresponse.Persistence;
using Skyresponse.Services;
using Skyresponse.SoundWrappers;
using Skyresponse.Systemtray;
using Unity;

namespace Skyresponse
{
    public class Startup
    {
        public static IUnityContainer BuildUnityContainer()
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IPersistenceManager, PersistenceManager>();
            unityContainer.RegisterType<ISoundWrapper, SoundWrapper>();
            unityContainer.RegisterType<ISoundService, SoundService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IHttpWrapper, HttpWrapper>();
            unityContainer.RegisterType<IWebSocketWrapper, WebSocketWrapper>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IDialogWrapper, DialogWrapper>();
            unityContainer.RegisterType<ILoginForm, LoginForm>();
            unityContainer.RegisterType<ISkyresponseApi, SkyresponseApi>();
            //Always last
            unityContainer.RegisterType<ISystemTrayApplicationContext, SystemTrayApplicationContext>();

            return unityContainer;
        }
    }
}
