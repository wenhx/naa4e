# naa4e
## Companion code for the [Microsoft .NET - Architecting Applications for the Enterprise, 2nd Edition book](https://www.microsoftpressstore.com/store/microsoft-.net-architecting-applications-for-the-enterprise-9780133986426)  
The original files forked from https://naa4e.codeplex.com/  

Projects "IBuyStuff-dm" and "IBuyStuff-cqrs" have been upgraded from .Net Framework 4.5 to .Net 8, along with the upgrade of related components, including ASP.Net Identity and Entity Framework.  
The original projects are located in the net45 directory, while the upgraded projects are now located in the net8 directory.  

Since the configuration for Twitter and Facebook login has become invalid, using Twitter and Facebook login will prompt an error.  
If you wish to use this feature, please modify the corresponding key and secret in the appsettings.json configuration file with the correct values obtained from Twitter or Facebook.  