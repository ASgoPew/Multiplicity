using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The SendSection (0xA) packet.
    /// </summary>
    public class SendSection : TerrariaPacket
    {

        public byte[] TilePayload { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendSection"/> class.
        /// </summary>
        public SendSection()
            : base((byte)PacketTypes.SendSection)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendSection"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public SendSection(BinaryReader br)
            : base(br)
        {
            /*
             * Not interested in the tile data for now.
             

            this.Compressed = br.ReadBoolean();
            int readBytes = 0;
            byte[] buffer = new byte[1024];

            if (Compressed)
            {

                using (MemoryStream tileDataStream = new MemoryStream())
                using (DeflateStream deflateStream = new DeflateStream(br.BaseStream, CompressionMode.Decompress, leaveOpen: true))
                using (BinaryReader compressionReader = new BinaryReader(deflateStream, System.Text.Encoding.UTF8))
                {
                    this.X = compressionReader.ReadInt32();
                    this.Y = compressionReader.ReadInt32();
                    this.Width = compressionReader.ReadInt16();
                    this.Height = compressionReader.ReadInt16();

                    while ((readBytes = compressionReader.Read(buffer, 0, 1024)) != 0)
                    {
                        tileDataStream.Write(buffer, 0, readBytes);
                    }

                    TilePayload = tileDataStream.ToArray();
                }
                return;
            }
            */

            // I hate myself for this, but there's no easy way to check if compressed and read the packet

            this.TilePayload = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
        }

        public override string ToString()
        {
            return
	            $"[SendSection Compressed: TileData: {TilePayload.Length/1024:0.###} kB]";
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short) (TilePayload.Length);
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
            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(TilePayload);
            }
        }

        #endregion

    }
}
