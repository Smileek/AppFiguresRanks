using System;
using System.Json;

namespace Rankings_Common
{
	public class Product
	{
		public Product (JsonValue json)
		{
			Id = json ["id"];
			Name = json ["name"];
			PackageName = json["package_name"];
			Developer = json ["developer"];
			Store = json ["store"];
			IconUrl = json ["icon"];
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public string PackageName { get; set; }

		public string Developer { get; set; }

		public string Store { get; set; }

		public string IconUrl { get; set; }
	}
}

