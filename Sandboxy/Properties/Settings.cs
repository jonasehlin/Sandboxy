using System.IO;
using System.Xml;

namespace Sandboxy.Properties
{
	class Settings
    {
		public static Settings Default = new Settings();

		private Settings()
		{
			XmlDocument doc = new XmlDocument();
			using (Stream stream = GetType().Assembly.GetManifestResourceStream("Sandboxy.Properties.Settings.xml"))
			{
				doc.Load(stream);
			}

			ReadSettings(doc);
		}

		/*
		Example embedde resource Settings.xml:
		<?xml version="1.0" encoding="utf-8" ?>
		<Settings>
			<Setting name="ConnectionString">Data Source=(local);Initial Catalog=MK_DbTests;Integrated Security=True</Setting>
		</Settings>
		*/
		private void ReadSettings(XmlDocument doc)
		{
			ConnectionString = doc.SelectSingleNode("/Settings/Setting[@name='ConnectionString']").InnerText;
		}

		public string ConnectionString { get; private set; }
	}
}
