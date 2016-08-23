using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The SocialHandshake (0x5D) packet.
    /// </summary>
    public class SocialHandshake : TerrariaPacket
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialHandshake"/> class.
        /// </summary>
        public SocialHandshake()
            : base((byte)PacketTypes.SocialHandshake)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialHandshake"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public SocialHandshake(BinaryReader br)
            : base(br)
        {
        }

        public override string ToString()
        {
            return string.Format("[SocialHandshake]");
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(0);
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
            }
        }

        #endregion

    }
}
