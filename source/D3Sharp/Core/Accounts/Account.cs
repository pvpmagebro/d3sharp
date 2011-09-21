﻿/*
 * Copyright (C) 2011 D3Sharp Project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using D3Sharp.Core.Helpers;
using D3Sharp.Core.Objects;
using D3Sharp.Core.Storage;
using D3Sharp.Core.Toons;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;

namespace D3Sharp.Core.Accounts
{
    public class Account : PersistentRPCObject
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public bnet.protocol.EntityId BnetAccountID { get; private set; }
        public bnet.protocol.EntityId BnetGameAccountID { get; private set; }
        public D3.Account.BannerConfiguration BannerConfiguration { get; private set; }

        public string Email { get; private set; }

        public D3.Account.Digest Digest
        {
            get
            {
                var builder = D3.Account.Digest.CreateBuilder().SetVersion(99)
                    .SetBannerConfiguration(this.BannerConfiguration)
                    .SetFlags(0);

                builder.SetLastPlayedHeroId(Toons.Count > 0
                                                ? Toons.First().Value.D3EntityID
                                                : D3.OnlineService.EntityId.CreateBuilder().SetIdHigh(0).SetIdLow(0).
                                                      Build());
                return builder.Build();
            }
        }

        public Dictionary<ulong, Toon> Toons
        {
            get { return ToonManager.GetToonsForAccount(this); }
        }

        public Account(ulong persistantId, string email) // account with given persistantId
            : base(persistantId)
        {
            this.SetFields(email);
        }

        public Account(string email) // account with **newly generated** persistantId
            : base()
        {
            this.SetFields(email);
        }

        private static ulong? _persistantIdCounter = null;
        protected override ulong GenerateNewPersistentId()
        {
            if (_persistantIdCounter == null)
                _persistantIdCounter = AccountManager.GetNextAvailablePersistantId();

            return (ulong)++_persistantIdCounter;
        }

        private void SetFields(string email)
        {
            this.Email = email;
            this.BnetAccountID = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.AccountId).SetLow(this.PersistentID).Build();
            this.BnetGameAccountID = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.GameAccountId).SetLow(this.PersistentID).Build();
            this.BannerConfiguration = D3.Account.BannerConfiguration.CreateBuilder()
                .SetBackgroundColorIndex(20)
                .SetBannerIndex(8)
                .SetPattern(4)
                .SetPatternColorIndex(11)
                .SetPlacementIndex(11)
                .SetSigilAccent(4)
                .SetSigilMain(3)
                .SetSigilColorIndex(7)
                .SetUseSigilVariant(true)
                .Build();
        }

        protected override void NotifySubscriber(Net.BNet.BNetClient client)
        {
            // Check d3sharp/docs/rpc/notification-data-layout.txt for fields keys

            // RealID name field
            var fieldKey1 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet,1, 1, 0);
            var field1 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey1).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue("RealID Name here!").Build()).Build();
            var fieldOperation1 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field1).Build();

            // Hardcoded boolean - always true
            var fieldKey2 = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, 1, 2, 0);
            var field2 = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey2).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(true).Build()).Build();
            var fieldOperation2 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field2).Build();

            // Create a presence.ChannelState
            var state = bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.BnetAccountID).AddFieldOperation(fieldOperation1).AddFieldOperation(fieldOperation2).Build();

            // Embed in channel.ChannelState
            var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);

            // Put in addnotification message
            var builder = bnet.protocol.channel.AddNotification.CreateBuilder().SetChannelState(channelState);

            // Make the rpc call
            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyAdd"), builder.Build(), this.DynamicId);
        }

        public void SaveToDB()
        {
            try
            {
                var query =
                    string.Format(
                        "INSERT INTO accounts (id, email) VALUES({0},'{1}')",
                        this.PersistentID, this.Email);

                    var cmd = new SQLiteCommand(query, DBManager.ToonConnection);
                    cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "SaveToDB()");
            }
        }
    }
}
