using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using GivingTree.Data.Services;

namespace GivingTree.Web
{
	// Set up Autofac Inversion of Control Dependency Injection, which will allow us to remove any hardcoded string referring to the database and instead configure the database that it points to dependent on the settings here (ie. if we want to switch between dev/release config, we can do it here)
	// We also need to register this class in our Global.asax
	public class ContainerConfig
	{
		internal static void RegisterContainer()
		{
			// here we need to tell the container about the different components and abstractions that we have in the project (ie. what services we have that we want injected into other pieces of software inside the application)
			// So when the project comes across something like the controller that is asking for IFruitTreeData, this container knows what to do with it
			var builder = new ContainerBuilder();

			// custom extension method specifically designed to integrate MVC 5 with Autofac. 
			// what this is going to do is scan through the project for the different controller types, and register those with Autofac. The parameters here tell Autofac what assembly contains the controllers for the application
			builder.RegisterControllers(typeof(MvcApplication).Assembly);

			// now we tell the ContainerBuilder about the specific services we have, and use this type whenever something asks for an object that implements it.
			// anywhere in the application we can now ask for IFruitTreeData, and receive whatever type is specified here. So in the future if we ever want to move away from using this type, we only need to make the change once, inside the RegisterType section
			builder.RegisterType<SqlFruitTreeData>()
				.As<IFruitTreeData>()
				//
				.InstancePerRequest();
			builder.RegisterType<GivingTreeDbContext>().InstancePerRequest();


			// now we just need to create the container from ContainerBuilder
			var container = builder.Build();
			// this sets our container as the dependency resolver throughout the project, instead of the default resolver that's supplied
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));



		}
	}
}
