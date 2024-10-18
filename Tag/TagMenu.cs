﻿using Menu.Remix;
using RainMeadow;
using System;
using System.Linq;
using UnityEngine;

namespace TagMod
{
    internal class TagMenu : CustomLobbyMenu
    {
        private Lobby lobby;
        private TagGameMode gamemode;
        private SlugcatCustomization customization;
        private SlugcatCustomizationSelector customizationHolder;
        private SimplerButton playButton;
        private OpComboBox2 shelterSelect;
        private ProperlyAlignedMenuLabel namesLabel;
        private ProperlyAlignedMenuLabel hidingLabel;
        private ProperlyAlignedMenuLabel huntingLabel;

        public TagMenu(ProcessManager manager) : base(manager, TagMod.TagMenu)
        {
            // customization
            this.lobby = OnlineManager.lobby;
            this.gamemode = (TagGameMode)lobby.gameMode;
            this.customization = gamemode.avatarSettings;

            customization.eyeColor = RainMeadow.RainMeadow.rainMeadowOptions.EyeColor.Value;
            customization.bodyColor = RainMeadow.RainMeadow.rainMeadowOptions.BodyColor.Value;
            customization.playingAs = SlugcatStats.Name.White;
            customization.nickname = OnlineManager.mePlayer.id.name;

            this.customizationHolder = new SlugcatCustomizationSelector(this, this.mainPage, new Vector2(540, 460), customization);
            mainPage.subObjects.Add(this.customizationHolder);

            playButton = new SimplerButton(this, mainPage, Translate("PLAY!"), new Vector2(1056f, 50f), new Vector2(110f, 30f));
            playButton.OnClick += Play;
            mainPage.subObjects.Add(playButton);

            if (lobby.isOwner)
            {
                System.Collections.Generic.List<Menu.Remix.MixedUI.ListItem> shelters = RainWorld.roomNameToIndex.Keys.Where(k => k.Length > 3 && k[2] == '_' && (k[3] == 'S' || k[3] == 's')).Select(e => new Menu.Remix.MixedUI.ListItem(e)).ToList();
                this.shelterSelect = new OpComboBox2(new Configurable<string>("SU_S01"), new Vector2(540, 400), 120, shelters);
                new UIelementWrapper(tabWrapper, this.shelterSelect);
                shelterSelect.OnValueChanged += ShelterSelect_OnValueChanged;
                gamemode.tagData.startingRoom = "SU_S01";

                gamemode.currentCampaign = SlugcatStats.Name.White;
            }

            mainPage.subObjects.Add(namesLabel = new RainMeadow.ProperlyAlignedMenuLabel(this, mainPage, "", new Vector2(870, 560), new Vector2(150, 20f), false));
            mainPage.subObjects.Add(hidingLabel = new RainMeadow.ProperlyAlignedMenuLabel(this, mainPage, "", new Vector2(1020, 560), new Vector2(60, 20f), false));
            mainPage.subObjects.Add(huntingLabel = new RainMeadow.ProperlyAlignedMenuLabel(this, mainPage, "", new Vector2(1100, 560), new Vector2(60, 20f), false));
        }

        private void ShelterSelect_OnValueChanged(Menu.Remix.MixedUI.UIconfig config, string value, string oldValue)
        {
            if (string.IsNullOrEmpty(value)) { TagMod.Error("null value"); }
            else
            {
                TagMod.Debug("starting room is " + value);
                gamemode.tagData.startingRoom = value;
            }
        }

        public override void Update()
        {
            base.Update();
            if (!lobby.isOwner) playButton.buttonBehav.greyedOut = !gamemode.isInGame;

            namesLabel.text = string.Join("\n", new string[] { "Players" }.Concat(lobby.clientSettings.Select(c => c.Value.TryGetData<HunterData>(out var data) ?
            c.Key.id.name
            : null).Where(t => t != null)));
            hidingLabel.text = string.Join("\n", new string[] { "Hiding" }.Concat(lobby.clientSettings.Select(c => c.Value.TryGetData<HunterData>(out var data) ?
            HideTimer.FormatTime(data.TotalTimeHiding)
            : null).Where(t => t != null)));
            huntingLabel.text = string.Join("\n", new string[] { "Hunting" }.Concat(lobby.clientSettings.Select(c => c.Value.TryGetData<HunterData>(out var data) ?
            HideTimer.FormatTime(data.TotalTimeHunting)
            : null).Where(t => t != null)));
        }

        private void Play(SimplerButton button)
        {
            TagMod.Debug("my name is " + customization.nickname);
            TagMod.Debug("starting room is " + gamemode.tagData.startingRoom);
            manager.arenaSitting = null;
            manager.rainWorld.progression.ClearOutSaveStateFromMemory();
            manager.rainWorld.progression.miscProgressionData.currentlySelectedSinglePlayerSlugcat = SlugcatStats.Name.White;
            manager.menuSetup.startGameCondition = ProcessManager.MenuSetup.StoryGameInitCondition.RegionSelect;
            manager.menuSetup.regionSelectRoom = gamemode.tagData.startingRoom;
            manager.RequestMainProcessSwitch(ProcessManager.ProcessID.Game);
        }
    }
}