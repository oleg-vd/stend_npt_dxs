﻿using System;

/*
 * Copyright 2012, 2013 by Mario Vernari, Cet Electronics
 * Part of "Cet Open Toolbox" (http://cetdevelop.codeplex.com/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Cet.IO.Protocols
{
    /// <summary>
    /// Modbus client proxy
    /// </summary>
    public class ModbusClient
        : IProtocol
    {
        public ModbusClient(IProtocolCodec codec)
        {
            this.Codec = codec;
        }


        /// <summary>
        /// The reference to the codec to be used
        /// </summary>
        public IProtocolCodec Codec { get; private set; }


        /// <summary>
        /// The address of the remote device
        /// </summary>
        public byte Address { get; set; }


        /// <summary>
        /// Entry-point for submitting any command
        /// </summary>
        /// <param name="port">The client port for the transport</param>
        /// <param name="command">The command to be submitted</param>
        /// <returns>The result of the query</returns>

        public CommResponse ExecuteGeneric(
            ICommClient port,
            ModbusCommand command)
        {
            var data = new ClientCommData(this);
            data.UserData = command;
            this.Codec.ClientEncode(data);

            return port
                .Query(data);
        }
        public CommResponse ExecuteGeneric(
            ICommClient port,
            ModbusCommand command, int retries, int timeout)
        {
            var data = new ClientCommData(this);
            data.UserData = command;
            data.Retries = retries;
            data.Timeout = timeout;
            this.Codec.ClientEncode(data);

            return port
                .Query(data);
        }

    }
}
