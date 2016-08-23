using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerTeleportThroughPortal (0x60) packet.
    /// </summary>
    public class PlayerTeleportThroughPortal : TerrariaPacket
    {

        public byte PlayerID { get; set; }

        public short PortalColorIndex { get; set; }

        public float NewPositionX { get; set; }

        public float NewPositionY { get; set; }

        public float VelocityX { get; set; }

        public float VelocityY { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTeleportThroughPortal"/> class.
        /// </summary>
        public PlayerTeleportThroughPortal()
            : base((byte)PacketTypes.PlayerTeleportThroughPortal)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTeleportThroughPortal"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public PlayerTeleportThroughPortal(BinaryReader br)
            : base(br)
        {
            this.PlayerID = br.ReadByte();
            this.PortalColorIndex = br.ReadInt16();
            this.NewPositionX = br.ReadSingle();
            this.NewPositionY = br.ReadSingle();
            this.VelocityX = br.ReadSingle();
            this.VelocityY = br.ReadSingle();
        }

        public override string ToString()
        {
            return
	            $"[PlayerTeleportThroughPortal: PlayerID = {PlayerID} PortalColorIndex = {PortalColorIndex} NewPositionX = {NewPositionX} NewPositionY = {NewPositionY} VelocityX = {VelocityX} VelocityY = {VelocityY}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(19);
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
                br.Write(PlayerID);
                br.Write(PortalColorIndex);
                br.Write(NewPositionX);
                br.Write(NewPositionY);
                br.Write(VelocityX);
                br.Write(VelocityY);
            }
        }

        #endregion

    }
}
