using System;
using BetterMoistureRedux.Config;
using HarmonyLib;
using Vintagestory.API.Common;

namespace BetterMoistureRedux;
public class BetterMoistureReduxModSystem : ModSystem {
	public static ILogger Logger { get; private set; }
	public static ICoreAPI Api { get; private set; }
	public static Harmony harmony { get; private set; }
	public override void Start(ICoreAPI api) {
		api.Logger.Debug("[BetterMoistureRedux] Start");
		base.Start(api);

		harmony = new Harmony(Mod.Info.ModID);
		Api = api;
		Logger = Mod.Logger;

		try {
			ModConfig.Instance = api.LoadModConfig<ModConfig>(ModConfig.ConfigName) ?? new ModConfig();
			api.StoreModConfig(ModConfig.Instance, ModConfig.ConfigName);
		} catch (Exception) { ModConfig.Instance = new ModConfig(); }

		harmony.PatchAllUncategorized();

		if (ModConfig.Instance.removeFarmlandAdjacentWaterSound) {
			harmony.PatchCategory("removeFarmlandWaterSloshing");
		}
	}

	public override void Dispose() {
		harmony?.UnpatchAll(Mod.Info.ModID);
		harmony = null;
		Logger = null;
		Api = null;
		base.Dispose();
	}
}
