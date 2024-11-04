﻿using TraitFinderApp.Client.Model.Search;

namespace TraitFinderApp.Client.Model
{
    public class ClusterLayout
    {
        public string Id;
        public string Name;
        public string Prefix;

        public string DisplayName => Name.Replace("Moonlet Cluster - ", string.Empty);

        public int menuOrder;

        public string Image => WorldPlacements[startWorldIndex].Asteroid.Image;
        public List<Dlc> RequiredDlcs;
        public List<Dlc> ForbiddenDlcs;
        public List<string> RequiredDlcsIDs;
        public List<string> ForbiddenDlcIDs;

        public List<string> WorldPlacementIDs;//transferBinding

        public List<WorldPlacement> WorldPlacements;
        public int clusterCategory;
        public ClusterCategory ClusterCategory;

        public int fixedCoordinate;

        public int startWorldIndex = 0;
        public bool HasFixedCoordinate() => fixedCoordinate > 0;

        public bool DlcRequirementsFulfilled(List<Dlc> requirements) => !RequiredDlcs .Except(requirements).Any() && !ForbiddenDlcs.Intersect(requirements).Any();

        public bool AllowedWithCurrentQuery(SearchQuery query) => query.ActiveMode == ClusterCategory && DlcRequirementsFulfilled(query.ActiveDlcs);
        public void InitBindings()
        {
            WorldPlacements = new(12);
            foreach (var placementId in WorldPlacementIDs)
            {
                WorldPlacements.Add(new WorldPlacement(Asteroid.KeyValues[placementId]));
            }

            RequiredDlcs = new();
            if (RequiredDlcs != null)
                foreach (var dlc in RequiredDlcsIDs)
                {
                    RequiredDlcs.Add(Dlc.KeyValues[dlc]);
                }
            ForbiddenDlcs = new();
            if (ForbiddenDlcIDs != null)
                foreach (var dlc in ForbiddenDlcIDs)
                {
                    ForbiddenDlcs.Add(Dlc.KeyValues[dlc]);
                }
            switch (clusterCategory)
            {
                case 0:
                    ClusterCategory = ClusterCategory.BASEGAME_STANDARD;
                    break;
                case 1:
                    ClusterCategory = ClusterCategory.SPACEDOUT_CLASSIC;
                    break;
                case 2:
                    ClusterCategory = ClusterCategory.SPACEDOUT_SPACEDOUT;
                    break;
                case 3:
                    ClusterCategory = RequiredDlcsIDs.Contains(Dlc.SPACEDOUT_ID) ? ClusterCategory.SPACEDOUT_THELAB : ClusterCategory.BASEGAME_THELAB;
                    break;

            }

        }
        public static List<ClusterLayout> Values => KeyValues.Values.OrderBy(i=>i.menuOrder).ToList();
        public static Dictionary<string, ClusterLayout> KeyValues
        {
            get
            {
                if (_values == null)
                {
                    DataImport.ImportGameData(true);
                }

                return _values;
            }
            set
            {
                _values = value;
            }
        }
        private static Dictionary<string, ClusterLayout> _values = null;
    }
}