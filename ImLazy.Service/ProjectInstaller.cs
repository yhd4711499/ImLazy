﻿using System.ComponentModel;
using System.Configuration.Install;

namespace ImLazy.Service
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
		}
	}
}
