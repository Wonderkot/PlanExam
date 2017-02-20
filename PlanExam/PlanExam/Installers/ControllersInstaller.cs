using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using PlanExam.Abstract;
using PlanExam.Implementation;

namespace PlanExam.Installers
{
    using Plumbing;

    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.
                    FromThisAssembly().
                    BasedOn<IController>().
                    If(c => c.Name.EndsWith("Controller")).
                    LifestyleTransient());

            container.Register(Component.For<IScaleService>().ImplementedBy<ImageScaleService>().LifestylePerWebRequest().Named("ImageScaleService"));
            container.Register(Component.For<IScaleService>().ImplementedBy<PdfScaleService>().LifestylePerWebRequest().Named("PdfScaleService"));

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
        }
    }
}