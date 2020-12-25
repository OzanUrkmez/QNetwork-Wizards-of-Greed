﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Game.Networking
{
    [CreateAssetMenu(fileName = "PacketDataType", menuName = "Networking/PacketDataType", order = 1)]
    public class PacketDataType : ScriptableObject
    {

        public string Description;

        public ushort ID;

        public UnityEvent<Packet> Handler;

        public TypeCode[] Primitives;

        private ushort calculatedMinimumTotalByteLength = ushort.MaxValue;

        private ushort stringPrimitiveCount;

        private ushort lengthToTellRealLength;

        private void Awake()
        {
            CalculateMinimumByteLength();
            CalculateStringPrimitive();
        }

        private void CalculateMinimumByteLength()
        {
            calculatedMinimumTotalByteLength = 4; //since we start with ID and length.
            foreach (TypeCode t in Primitives)
            {
                switch (t)
                {
                    case TypeCode.Object:
                        calculatedMinimumTotalByteLength += 4;
                        break;
                    case TypeCode.DBNull:
                        calculatedMinimumTotalByteLength += 4;
                        break;
                    case TypeCode.Boolean:
                        calculatedMinimumTotalByteLength += 1;
                        break;
                    case TypeCode.Char:
                        calculatedMinimumTotalByteLength += 2;
                        break;
                    case TypeCode.SByte:
                        calculatedMinimumTotalByteLength += 1;
                        break;
                    case TypeCode.Byte:
                        calculatedMinimumTotalByteLength += 1;
                        break;
                    case TypeCode.Int16:
                        calculatedMinimumTotalByteLength += 2;
                        break;
                    case TypeCode.UInt16:
                        calculatedMinimumTotalByteLength += 2;
                        break;
                    case TypeCode.Int32:
                        calculatedMinimumTotalByteLength += 4;
                        break;
                    case TypeCode.UInt32:
                        calculatedMinimumTotalByteLength += 4;
                        break;
                    case TypeCode.Int64:
                        calculatedMinimumTotalByteLength += 8;
                        break;
                    case TypeCode.UInt64:
                        calculatedMinimumTotalByteLength += 8;
                        break;
                    case TypeCode.Single:
                        calculatedMinimumTotalByteLength += 4;
                        break;
                    case TypeCode.Double:
                        calculatedMinimumTotalByteLength += 8;
                        break;
                    case TypeCode.Decimal:
                        calculatedMinimumTotalByteLength += 16;
                        break;
                    case TypeCode.DateTime:
                        calculatedMinimumTotalByteLength += 8;
                        break;
                    case TypeCode.String:
                        calculatedMinimumTotalByteLength += 2;
                        break;
                    default:
                        Debug.LogError("Unexpected type code during packet data type length calculation");
                        break;
                }

            }
        }

        private void CalculateStringPrimitive()
        {
            stringPrimitiveCount = 0;
            ushort expected = 0;
            for(ushort i = 0; i < Primitives.Length; i++)
            {
                if(Primitives[i] == TypeCode.String)
                {
                    stringPrimitiveCount++;
                    if(i != expected)
                    {
                        Debug.LogError("The packet specification of ID " + ID.ToString() + " has a string primtive that is not at the start or right after another string!");
                    }
                    expected++;
                }
            }

        }

        /// <summary>
        /// This is called minimum since a string can be indefinitely long.
        /// </summary>
        public ushort MinimumByteLength { get { return calculatedMinimumTotalByteLength; } }

        /// <summary>
        /// All Strings should be at the start of the packet, their counts written first, all after ID.
        /// </summary>
        public ushort StringPrimitveCount { get { return stringPrimitiveCount; } }
    }

}
