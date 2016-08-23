using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The UpdateNPCBuff (0x36) packet.
    /// </summary>
    public class UpdateNPCBuff : TerrariaPacket
    {

        public short NPCID { get; set; }

        public byte BuffID { get; set; }

        public short Time { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNPCBuff"/> class.
        /// </summary>
        public UpdateNPCBuff()
            : base((byte)PacketTypes.UpdateNPCBuff)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNPCBuff"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public UpdateNPCBuff(BinaryReader br)
            : base(br)
        {
            this.NPCID = br.ReadInt16();
            this.BuffID = br.ReadByte();
            this.Time = br.ReadInt16();
        }

        public override string ToString()
        {
            return $"[UpdateNPCBuff: NPCID = {NPCID} BuffID = {BuffID} Time = {Time}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(5);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            /*
             * Length and ID headers get written in the base packet class.
             */
            if (includeHeader) {
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
            using (BinaryWriter br = new BinaryWriter(stream, new System.Text.UTF8Encoding(), leaveOpen: true)) {
                br.Write(NPCID);
                br.Write(BuffID);
                br.Write(Time);
            }
        }

        #endregion

    }
}
