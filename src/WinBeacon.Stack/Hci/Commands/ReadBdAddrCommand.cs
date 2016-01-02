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

using System.Linq;
using WinBeacon.Stack.Hci.Opcodes;

namespace WinBeacon.Stack.Hci.Commands
{
    internal class ReadBdAddrCommand : Command<DeviceAddress>
    {
        public ReadBdAddrCommand()
            : base(OpcodeGroup.InformationalParameters, (int)InformationalParametersOpcode.ReadBdAddr)
        {
        }

        internal override DeviceAddress ParseCommandResult(Events.CommandCompleteEvent e)
        {
            if (e.ResultData == null || e.ResultData.Length != 6)
                return DeviceAddress.Empty;
            return new DeviceAddress(e.ResultData.Reverse().ToArray());
        }
    }
}
