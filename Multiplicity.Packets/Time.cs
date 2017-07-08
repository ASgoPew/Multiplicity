using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The Time (0x12) packet.
    /// </summary>
    public class Time : TerrariaPacket
    {

        public bool DayTime { get; set; }

        public int TimeValue { get; set; }

        public short SunModY { get; set; }

        public short MoonModY { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> class.
        /// </summary>
        public Time()
            : base((byte)PacketTypes.Time)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public Time(BinaryReader br)
            : base(br)
        {
            this.DayTime = br.ReadBoolean();
            this.TimeValue = br.ReadInt32();
            this.SunModY = br.ReadInt16();
            this.MoonModY = br.ReadInt16();
        }

        public override string ToString()
        {
            return $"[Time: DayTime = {DayTime} TimeValue = {TimeValue} SunModY = {SunModY} MoonModY = {MoonModY}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(9);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            /*
             * Length and ID headers get written in the base packet class.
             */
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            /*
             * Always make sure to not close the stream when serializing.
             * 
             * It is up to the caller to decide if the underlying stream
             * gets closed.  If this is a network stream we do not want
             * the regressions of unconditionally closing the TCP socket
             * once the payload of data has been sent to the client.
             */
            using (BinaryWriter br = new BinaryWriter(stream, new System.Text.UTF8Encoding(), leaveOpen: true))
            {
                br.Write(DayTime);
                br.Write(TimeValue);
                br.Write(SunModY);
                br.Write(MoonModY);
            }
        }

        #endregion

    }
}
