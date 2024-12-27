// <copyright file="BuildingPropFixesSystem.cs" company="Yenyangs Mods. MIT License">
// Copyright (c) Yenyangs Mods. MIT License. All rights reserved.
// </copyright>

namespace BuildingPropFixes.Systems
{
    using Colossal.Entities;
    using Colossal.Logging;
    using Colossal.Serialization.Entities;
    using Game;
    using Game.Prefabs;
    using System.Collections.Generic;
    using Unity.Collections;
    using Unity.Entities;

    /// <summary>
    /// Fixes the prefab entities of certain prefabs in the region packs.
    /// </summary>
    public partial class BuildingPropFixesSystem : GameSystemBase
    {
        private ILog m_Log;
        private PrefabSystem m_PrefabSystem;
        private EntityQuery m_PropPrefabsQuery;

        private List<string> m_PropNamePieces = new List<string>()
        {
            "Wall",
            "Hedge",
            "Railing",
            "Fence",
            "Gate",
            "Shed",
            "Bed",
            "Room",
        };

        /// <inheritdoc/>
        protected override void OnCreate()
        {
            base.OnCreate();
            m_Log = Mod.log;
            m_Log.Info($"{nameof(BuildingPropFixesSystem)}.OnCreate");

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            m_PropPrefabsQuery = SystemAPI.QueryBuilder()
                .WithAll<PlaceableObjectData, ObjectGeometryData>()
                .WithNone<BuildingData>()
                .Build();

            Enabled = false;
        }

        /// <inheritdoc/>
        protected override void OnUpdate()
        {
            return;
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            // Look through pack prefab entities for placeable objects that are not buildings.
            NativeArray<Entity> prefabEntities = m_PropPrefabsQuery.ToEntityArray(Allocator.Temp);
            foreach (Entity prefabEntity in prefabEntities)
            {
                if (m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase) &&
                    prefabBase is not null &&
                    EvaluatePrefabName(m_PropNamePieces, prefabBase.name) &&
                    EntityManager.TryGetComponent(prefabEntity, out ObjectGeometryData objectGeometryData))
                {
                    objectGeometryData.m_Flags |= Game.Objects.GeometryFlags.Standing | Game.Objects.GeometryFlags.CanSubmerge;
                    EntityManager.SetComponentData(prefabEntity, objectGeometryData);
                } 
            }
        }

        /// <summary>
        /// Loops through name pieces and if name contains a piece returns true.
        /// </summary>
        /// <param name="namePieces">Pieces of a name that pass the filter.</param>
        /// <param name="name">prefab name to parse.</param>
        /// <returns></returns>
        private bool EvaluatePrefabName(List<string> namePieces, string name)
        {
            foreach (string piece in namePieces)
            {
                if (name.Contains(piece))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
