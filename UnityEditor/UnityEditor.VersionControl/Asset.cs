using System;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace UnityEditor.VersionControl
{
	public sealed class Asset
	{
		[Flags]
		public enum States
		{
			None = 0,
			Local = 1,
			Synced = 2,
			OutOfSync = 4,
			Missing = 8,
			CheckedOutLocal = 16,
			CheckedOutRemote = 32,
			DeletedLocal = 64,
			DeletedRemote = 128,
			AddedLocal = 256,
			AddedRemote = 512,
			Conflicted = 1024,
			LockedLocal = 2048,
			LockedRemote = 4096,
			Updating = 8192,
			ReadOnly = 16384,
			MetaFile = 32768
		}
		private string m_guid;
		public extern Asset.States state
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		public extern string path
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		public extern bool isFolder
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		public extern bool readOnly
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		public extern bool isMeta
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		public extern bool locked
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		public extern string name
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		public extern string fullName
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		public extern bool isInCurrentProject
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		internal bool IsUnderVersionControl
		{
			get
			{
				return this.IsState(Asset.States.Synced) || this.IsState(Asset.States.OutOfSync) || this.IsState(Asset.States.AddedLocal);
			}
		}
		public string prettyPath
		{
			get
			{
				return this.path;
			}
		}
		public Asset(string clientPath)
		{
			this.InternalCreateFromString(clientPath);
		}
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalCreateFromString(string clientPath);
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Dispose();
		~Asset()
		{
			this.Dispose();
		}
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsChildOf(Asset other);
		internal static bool IsState(Asset.States isThisState, Asset.States partOfThisState)
		{
			return (isThisState & partOfThisState) != Asset.States.None;
		}
		public bool IsState(Asset.States state)
		{
			return Asset.IsState(this.state, state);
		}
		public bool IsOneOfStates(Asset.States[] states)
		{
			for (int i = 0; i < states.Length; i++)
			{
				Asset.States states2 = states[i];
				if ((this.state & states2) != Asset.States.None)
				{
					return true;
				}
			}
			return false;
		}
		public void Edit()
		{
			UnityEngine.Object @object = this.Load();
			if (@object != null)
			{
				AssetDatabase.OpenAsset(@object);
			}
		}
		public UnityEngine.Object Load()
		{
			if (this.state == Asset.States.DeletedLocal || this.isMeta)
			{
				return null;
			}
			return AssetDatabase.LoadAssetAtPath(this.path, typeof(UnityEngine.Object));
		}
		internal static string StateToString(Asset.States state)
		{
			if (Asset.IsState(state, Asset.States.AddedLocal))
			{
				return "Added Local";
			}
			if (Asset.IsState(state, Asset.States.AddedRemote))
			{
				return "Added Remote";
			}
			if (Asset.IsState(state, Asset.States.CheckedOutLocal) && !Asset.IsState(state, Asset.States.LockedLocal))
			{
				return "Checked Out Local";
			}
			if (Asset.IsState(state, Asset.States.CheckedOutRemote) && !Asset.IsState(state, Asset.States.LockedRemote))
			{
				return "Checked Out Remote";
			}
			if (Asset.IsState(state, Asset.States.Conflicted))
			{
				return "Conflicted";
			}
			if (Asset.IsState(state, Asset.States.DeletedLocal))
			{
				return "Deleted Local";
			}
			if (Asset.IsState(state, Asset.States.DeletedRemote))
			{
				return "Deleted Remote";
			}
			if (Asset.IsState(state, Asset.States.Local))
			{
				return "Local";
			}
			if (Asset.IsState(state, Asset.States.LockedLocal))
			{
				return "Locked Local";
			}
			if (Asset.IsState(state, Asset.States.LockedRemote))
			{
				return "Locked Remote";
			}
			if (Asset.IsState(state, Asset.States.OutOfSync))
			{
				return "Out Of Sync";
			}
			return string.Empty;
		}
		internal string StateToString()
		{
			return Asset.StateToString(this.state);
		}
	}
}
