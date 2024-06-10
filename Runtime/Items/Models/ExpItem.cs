using Glitch9.Game;

namespace Glitch9
{
    public enum ExpItemType
    {
        None,
        UserExp,
        SeasonPassExp,
        BarExp,
        CharacterExp
    }

    public class ExpItem : Item
    {
        private ExpItemType _type = ExpItemType.None;
        public ExpItemType Type => _type == ExpItemType.None ? _type = ParseItemType<ExpItemType>() : _type;
        public bool ConsumeImmediately { get; }

        private int? _expAmount;
        public int ExpAmount
        {
            get
            {
                if (_expAmount == null)
                {
                    int.TryParse(Arguments[0], out int result);
                    _expAmount = result;
                }
                return _expAmount.Value;
            }
        }
    }
}
