using CoChain;

namespace Urbrural
{
    /// <summary>
    /// A blog post for upon a certain project.
    /// </summary>
    public class Review : Entity
    {
        public static readonly Review Empty = new Review();

        public const short
            TYP_ACK = 1,
            TYP_CMT = 2;

        public static readonly Map<short, string> Typs = new Map<short, string>
        {
            {TYP_ACK, "点赞"},
            {TYP_CMT, "评论"},
        };


        public override void Read(ISource s, short msk = 0xff)
        {
            base.Read(s, msk);
        }

        public override void Write(ISink s, short msk = 0xff)
        {
            base.Write(s, msk);
        }
    }
}