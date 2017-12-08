using Skyresponse.Api;
using Skyresponse.DialogWrappers;
using Skyresponse.Forms;
using Skyresponse.HttpWrappers;
using Skyresponse.Persistence;
using Skyresponse.Services;
using Unity;

namespace Skyresponse
{
    public class Startup
    {
        public static IUnityContainer BuildUnityContainer()
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IPersistenceManager, PersistenceManager>();
            unityContainer.RegisterType<ISoundService, SoundService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IHttpWrapper, HttpWrapper>();
            unityContainer.RegisterType<IWebSocketWrapper, WebSocketWrapper>();
            unityContainer.RegisterType<IDialogWrapper, DialogWrapper>();
            unityContainer.RegisterType<ILoginForm, LoginForm>();
            unityContainer.RegisterType<ISkyresponseApi, SkyresponseApi>();
            //Always last
            unityContainer.RegisterType<IMainForm, MainForm>();

            return unityContainer;
        }
    }
}
