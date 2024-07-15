using CounterStrikeSharp.API.Core;
using System.Runtime.InteropServices;
using static CounterStrikeSharp.API.Core.Listeners;

namespace CS2_HammerIDFix
{
	public partial class CS2_HammerIDFix : BasePlugin
	{
		private int iOffset;
		private bool bOS;
		private bool bOnce = true;
		private byte[] bytePatch = { 0xB0, 0x01 };
		public override string ModuleName => "CS2-HammerIDFix";
		public override string ModuleVersion => "1.DZ.0";
		public override void Load(bool hotReload)
		{
			bOS = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
			iOffset = GameData.GetOffset("GetHammerUniqueId");
			RegisterListener<OnEntityCreated>(OnEntityCreated_Listener);
		}

		unsafe private void OnEntityCreated_Listener(CEntityInstance entity)
		{
			if (bOnce)
			{
				void** vtable = *(void***)entity.Handle;
				if (bOS) Memory.UnixMemoryUtils.PatchBytesAtAddress((IntPtr)vtable[iOffset], bytePatch, 2);
				else Memory.WinMemoryUtils.PatchBytesAtAddress((IntPtr)vtable[iOffset], bytePatch, 2);
				bOnce = false;
			}
		}
	}
}
