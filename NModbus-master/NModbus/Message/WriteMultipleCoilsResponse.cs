﻿using System;
using System.Net;

namespace NModbus.Message
{
    public class WriteMultipleCoilsResponse : AbstractModbusMessage, IModbusMessage
    {
        public WriteMultipleCoilsResponse()
        {
        }

        public WriteMultipleCoilsResponse(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
            : base(slaveAddress, ModbusFunctionCodes.WriteMultipleCoils)
        {
            StartAddress = startAddress;
            NumberOfPoints = numberOfPoints;
        }

        public ushort NumberOfPoints
        {
            get => MessageImpl.NumberOfPoints.Value;

            set
            {
                if (value > Modbus.MaximumDiscreteRequestResponseSize)
                {
                    string msg = $"Maximum amount of data {Modbus.MaximumDiscreteRequestResponseSize} coils.";
                    throw new ArgumentOutOfRangeException("NumberOfPoints", msg);
                }

                MessageImpl.NumberOfPoints = value;
            }
        }

        public ushort StartAddress
        {
            get => MessageImpl.StartAddress.Value;
            set => MessageImpl.StartAddress = value;
        }

        public override int MinimumFrameSize => 6;

        public byte[] V5Frame { get; set; }
        public bool V5Active { get ; set ; }
        public byte[] Modbus_Frame { get ; set  ; }
        public string VarName { get ; set ; }
        public int V5Serial { get; set; }
        public string V5IPAddress { get; set; }
        public int V5Port { get; set; }

        public override string ToString()
        {
            string msg = $"Wrote {NumberOfPoints} coils starting at address {StartAddress}.";
            return msg;
        }

        protected override void InitializeUnique(byte[] frame)
        {
            StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
            NumberOfPoints = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
        }
    }
}
