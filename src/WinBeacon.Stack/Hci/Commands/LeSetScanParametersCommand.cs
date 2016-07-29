/*
 * Copyright 2015-2016 Huysentruit Wouter
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Diagnostics;
using WinBeacon.Stack.Hci.Opcodes;
using WinBeacon.Stack.Hci.Parameters;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class LeSetScanParametersCommand : Command
    {
        public LeSetScanParametersCommand(bool activeScanning, ushort intervalInMs, ushort windowInMs, bool useRandomAddress, bool onlyAcceptWhitelistedAdvertisers)
            : base(OpcodeGroup.LeController, (int)LeControllerOpcode.SetScanParameters)
        {
            ushort intervalCode = (ushort)Math.Ceiling(intervalInMs / 0.625);
            ushort windowCode = (ushort)Math.Ceiling(windowInMs / 0.625);
            Debug.Assert(intervalCode >= 0x0004);
            Debug.Assert(intervalCode <= 0x4000);
            Debug.Assert(windowCode >= 0x0004);
            Debug.Assert(windowCode <= 0x4000);
            Parameters.Add(new BoolCommandParameter(activeScanning));
            Parameters.Add(new UshortCommandParameter(intervalCode));
            Parameters.Add(new UshortCommandParameter(windowCode));
            Parameters.Add(new BoolCommandParameter(useRandomAddress));
            Parameters.Add(new BoolCommandParameter(onlyAcceptWhitelistedAdvertisers));
        }
    }
}
