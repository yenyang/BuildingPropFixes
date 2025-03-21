using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using BuildingPropFixes.Systems;
using Unity.Entities;

namespace BuildingPropFixes
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"Mods_Yenyang_{nameof(BuildingPropFixes)}").SetShowsErrorsInUI(false);

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            BuildingPropFixesSystem fixFencePropPrefabsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<BuildingPropFixesSystem>(); 
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
        }
    }
}
