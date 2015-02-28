using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Modules;
using UnityEngine;
namespace UnityEditor
{
	internal class DesktopPluginImporterExtension : DefaultPluginImporterExtension
	{
		internal enum DesktopPluginCPUArchitecture
		{
			None,
			AnyCPU,
			x86,
			x86_64
		}
		internal class DesktopSingleCPUProperty : DefaultPluginImporterExtension.Property
		{
			internal bool isCpuTargetEnabled
			{
				get
				{
					return (int)base.value == (int)base.defaultValue;
				}
				private set
				{
					base.value = ((!value) ? DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.None : base.defaultValue);
				}
			}
			public DesktopSingleCPUProperty(GUIContent name, string platformName) : this(name, platformName, DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.AnyCPU)
			{
			}
			public DesktopSingleCPUProperty(GUIContent name, string platformName, DesktopPluginImporterExtension.DesktopPluginCPUArchitecture architecture) : base(name, "CPU", architecture, platformName)
			{
			}
			internal override void OnGUI(PluginImporterInspector inspector)
			{
				EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Space(10f);
				this.isCpuTargetEnabled = EditorGUILayout.Toggle(base.name, this.isCpuTargetEnabled, new GUILayoutOption[0]);
				EditorGUILayout.EndHorizontal();
			}
		}
		private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_WindowsX86;
		private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_WindowsX86_X64;
		private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_LinuxX86;
		private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_LinuxX86_X64;
		private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_OSXX86;
		private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_OSXX86_X64;
		public DesktopPluginImporterExtension() : base(null)
		{
			this.properties = this.GetProperties();
		}
		private DefaultPluginImporterExtension.Property[] GetProperties()
		{
			List<DefaultPluginImporterExtension.Property> list = new List<DefaultPluginImporterExtension.Property>();
			this.m_WindowsX86 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneWindows));
			this.m_WindowsX86_X64 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86_x64"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneWindows64));
			this.m_LinuxX86 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneLinux), DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.x86);
			this.m_LinuxX86_X64 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86_x64"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneLinux64), DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.x86_64);
			this.m_OSXX86 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneOSXIntel));
			this.m_OSXX86_X64 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86_x64"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneOSXIntel64));
			list.Add(this.m_WindowsX86);
			list.Add(this.m_WindowsX86_X64);
			list.Add(this.m_LinuxX86);
			list.Add(this.m_LinuxX86_X64);
			list.Add(this.m_OSXX86);
			list.Add(this.m_OSXX86_X64);
			return list.ToArray();
		}
		private DesktopPluginImporterExtension.DesktopPluginCPUArchitecture CalculateMultiCPUArchitecture(bool x86, bool x64)
		{
			if (x86 && x64)
			{
				return DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.AnyCPU;
			}
			if (x86 && !x64)
			{
				return DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.x86;
			}
			if (!x86 && x64)
			{
				return DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.x86_64;
			}
			return DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.None;
		}
		private bool IsUsableOnWindows(PluginImporter imp)
		{
			if (!imp.isNativePlugin)
			{
				return true;
			}
			string extension = Path.GetExtension(imp.assetPath);
			return extension == ".dll";
		}
		private bool IsUsableOnOSX(PluginImporter imp)
		{
			if (!imp.isNativePlugin)
			{
				return true;
			}
			string extension = Path.GetExtension(imp.assetPath);
			return extension == ".so" || extension == ".bundle";
		}
		private bool IsUsableOnLinux(PluginImporter imp)
		{
			if (!imp.isNativePlugin)
			{
				return true;
			}
			string extension = Path.GetExtension(imp.assetPath);
			return extension == ".so";
		}
		public override void OnPlatformSettingsGUI(PluginImporterInspector inspector)
		{
			PluginImporter importer = inspector.importer;
			EditorGUI.BeginChangeCheck();
			if (this.IsUsableOnWindows(importer))
			{
				EditorGUILayout.LabelField(EditorGUIUtility.TextContent("BuildSettings.StandaloneWindows"), EditorStyles.boldLabel, new GUILayoutOption[0]);
				this.m_WindowsX86.OnGUI(inspector);
				this.m_WindowsX86_X64.OnGUI(inspector);
				EditorGUILayout.Space();
			}
			if (this.IsUsableOnLinux(importer))
			{
				EditorGUILayout.LabelField(EditorGUIUtility.TextContent("BuildSettings.StandaloneLinux"), EditorStyles.boldLabel, new GUILayoutOption[0]);
				this.m_LinuxX86.OnGUI(inspector);
				this.m_LinuxX86_X64.OnGUI(inspector);
				EditorGUILayout.Space();
			}
			if (this.IsUsableOnOSX(importer))
			{
				EditorGUILayout.LabelField(EditorGUIUtility.TextContent("BuildSettings.StandaloneOSXIntel"), EditorStyles.boldLabel, new GUILayoutOption[0]);
				this.m_OSXX86.OnGUI(inspector);
				this.m_OSXX86_X64.OnGUI(inspector);
			}
			if (EditorGUI.EndChangeCheck())
			{
				inspector.importer.SetPlatformData(BuildTarget.StandaloneLinuxUniversal, "CPU", this.CalculateMultiCPUArchitecture(this.m_LinuxX86.isCpuTargetEnabled, this.m_LinuxX86_X64.isCpuTargetEnabled).ToString());
				inspector.importer.SetPlatformData(BuildTarget.StandaloneOSXUniversal, "CPU", this.CalculateMultiCPUArchitecture(this.m_OSXX86.isCpuTargetEnabled, this.m_OSXX86_X64.isCpuTargetEnabled).ToString());
				this.hasModified = true;
			}
		}
		public override string CalculateFinalPluginPath(string platformName, PluginImporter imp)
		{
			string platformData = imp.GetPlatformData(platformName, "CPU");
			if (string.Compare(platformData, "None", true) == 0)
			{
				return string.Empty;
			}
			if (!string.IsNullOrEmpty(platformData) && string.Compare(platformData, "AnyCPU", true) != 0)
			{
				return Path.Combine(platformData, Path.GetFileName(imp.assetPath));
			}
			return Path.GetFileName(imp.assetPath);
		}
	}
}
