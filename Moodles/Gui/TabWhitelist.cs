﻿using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons;
using ECommons.ExcelServices;
using ECommons.GameHelpers;
using Moodles.Data;
using Moodles.OtterGuiHandlers;
using OtterGui.Raii;
using System.Data;

namespace Moodles.Gui;
public static class TabWhitelist
{
    static WhitelistEntry Selected => P.OtterGuiHandler.Whitelist.Current;
    static string Filter = "";
    static bool Editing = true;
    public static void Draw()
    {
        P.OtterGuiHandler.Whitelist.Draw(200f);
        ImGui.SameLine();
        using var group = ImRaii.Group();
        DrawHeader();
        DrawSelected();
    }

    private static void DrawHeader()
    {
        HeaderDrawer.Draw(Selected == null ? "" : (Selected.PlayerName.Censor($"Whitelist entry {C.Whitelist.IndexOf(Selected) + 1}")), 0, ImGui.GetColorU32(ImGuiCol.FrameBg), 0, HeaderDrawer.Button.IncognitoButton(C.Censor, v => C.Censor = v));
    }

    private static void DrawSelected()
    {
        using var child = ImRaii.Child("##Panel", -Vector2.One, true);
        if (!child || Selected == null)
            return;

        if (ImGui.BeginTable("##wl", 2, ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Borders))
        {
            ImGui.TableSetupColumn("##txt", ImGuiTableColumnFlags.WidthFixed, 150);
            ImGui.TableSetupColumn("##inp", ImGuiTableColumnFlags.WidthStretch);

            ImGui.TableNextRow();
            ImGui.TableNextColumn();

            ImGuiEx.TextV($"Player Name @ World:");
            ImGui.TableNextColumn();

            ImGuiEx.InputWithRightButtonsArea(() =>
            {
                ImGui.InputText($"##pname", ref Selected.PlayerName, 100, C.Censor?ImGuiInputTextFlags.Password:ImGuiInputTextFlags.None);
            }, () =>
            {
                if (Svc.Targets.Target is PlayerCharacter pc)
                {
                    if (ImGui.Button("Target"))
                    {
                        Selected.PlayerName = pc.GetNameWithWorld();
                    }
                }
            });

            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGuiEx.TextV($"Allowed Status Types:");
            ImGui.TableNextColumn();
            foreach(var x in Enum.GetValues<StatusType>())
            {
                ImGuiEx.CollectionCheckbox($"{x}", x, Selected.AllowedTypes);
            }

            ImGui.EndTable();
        }
    }

    static void DrawPresetSelector(AutomationCombo combo)
    {
        var exists = P.OtterGuiHandler.PresetFileSystem.TryGetPathByID(combo.Preset, out var spath);
        if (ImGui.BeginCombo("##addnew", spath ?? "Select preset"))
        {
            ImGuiEx.SetNextItemFullWidth();
            ImGui.InputTextWithHint("##search", "Filter", ref Filter, 50);
            foreach (var x in C.SavedPresets)
            {
                if (P.OtterGuiHandler.PresetFileSystem.TryGetPathByID(x.GUID, out var path))
                {
                    if (Filter == "" || path.Contains(Filter, StringComparison.OrdinalIgnoreCase))
                    {
                        var split = path.Split(@"/");
                        var name = split[^1];
                        var directory = split[0..^1].Join(@"/");
                        if (directory != name)
                        {
                            ImGuiEx.RightFloat($"Selector{x.GUID}", () => ImGuiEx.Text(ImGuiColors.DalamudGrey, directory));
                        }
                        if (ImGui.Selectable($"{name}##{x.GUID}", combo.Preset == x.GUID))
                        {
                            combo.Preset = x.GUID;
                        }
                        if (ImGui.IsWindowAppearing() && combo.Preset == x.GUID)
                        {
                            ImGui.SetScrollHereY();
                        }
                    }
                }
            }
            ImGui.EndCombo();
        }
    }
}
