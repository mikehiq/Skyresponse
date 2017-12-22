using Skyresponse.Api;
using Skyresponse.Forms;
using Skyresponse.Persistence;
using Skyresponse.Services.Sound;
using Skyresponse.Services.User;
using Skyresponse.Services.WebSocket;
using Skyresponse.Systemtray;
using Skyresponse.Wrappers.DialogWrappers;
using Skyresponse.Wrappers.HttpWrappers;
using Skyresponse.Wrappers.SoundWrappers;
using Unity;
using Unity.Lifetime;

namespace Skyresponse
{
    public class Startup
    {
        public static IUnityContainer BuildUnityContainer()
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IPersistenceManager, PersistenceManager>();
            unityContainer.RegisterType<IUserService, UserService>();
            unityContainer.RegisterType<IWebSocketService, WebSocketService>();
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
