using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using NorthWind.Sales.Backend.DataContexts.EFCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts
{
	internal class NorthWindDomainLogsContextFactory :
	IDesignTimeDbContextFactory<NorthWindDomainLogsContext>
	{
		public NorthWindDomainLogsContext CreateDbContext(string[] args)
		{
			IOptions<DBOptions> DbOptions =
			Microsoft.Extensions.Options.Options.Create(
			new DBOptions
			{
				DomainLogsConnectionString = "Server=GEORGE-ASUS;Database=NorthWindLogsDB;User Id=sa;Password=jorgesa;TrustServerCertificate=True;"
			});
			return new NorthWindDomainLogsContext(DbOptions);
		}
	}

}
