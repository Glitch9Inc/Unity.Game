
using Glitch9.Game;

namespace Glitch9.Apis.Google.Sheets
{
    [Type(typeof(RewardData), new string[] { "MissionReward", "GNReward", "Reward" })]
    public class RewardType : IType
    {
        public object DefaultValue => null;

        public object Read(string value)
        {
            return new RewardData(value);
        }

        public string Write(object value)
        {
            RewardData v = (RewardData)value;
            return v.ToString();
        }
    }
}