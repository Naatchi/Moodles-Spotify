﻿using ECommons.Configuration;

namespace Moodles.Data;
public class Config : IEzConfig
{
    public bool Enabled = true;
    public bool EnabledDuty = false;
    public bool EnabledCombat = false;
    public Dictionary<string, MyStatusManager> StatusManagers = [];
    public List<MyStatus> SavedStatuses = [];
    public List<Preset> SavedPresets = [];
    public List<AutomationProfile> AutomationProfiles = [];
    public HashSet<string> SeenCharacters = [];
    public bool Censor = false;
    public bool AutoOther = false;

    public bool EnableVFX = true;
    public bool EnableFlyPopupText = true;
    public int FlyPopupTextLimit = 10;

    public HashSet<uint> FavIcons = [];
    public bool AutoFill = false;
    public int SelectorHeight = 33;
    public bool Debug = false;
    public SortOption IconSortOption = SortOption.Numerical;
    public List<WhitelistEntry> Whitelist = [];
}
