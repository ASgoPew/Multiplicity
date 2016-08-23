using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerDodge (0x3E) packet.
    /// </summary>
    public class PlayerDodge : TerrariaPacket
    {

        public byte PlayerID { get; set; }

        /// <summary>
        /// Gets or sets the Flag - 1 = Ninja Dodge 2 = Shadow Dodge|
        /// </summary>
        public byte Flag { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDodge"/> class.
        /// </summary>
        public PlayerDodge()
            : base((byte)PacketTypes.PlayerDodge)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDodge"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public PlayerDodge(BinaryReader br)
            : base(br)
        {
            this.PlayerID = br.ReadByte();
            this.Flag = br.ReadByte();
        }

        public override string ToString()
        {
            return $"[PlayerDodge: PlayerID = {PlayerID} Flag = {Flag}]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(2);
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
                br.Write(Flag);
            }
        }

        #endregion

    }
}
