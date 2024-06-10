using System;
using Glitch9.Apis.Google.Firestore;
using Glitch9.Cloud;

namespace Glitch9.Game
{
    public class Stamina
    {
        [CloudData] public int LastValue { get; set; }
        [CloudData("last_used")] public string RawLastUsed { get; set; }
        public DateTime LastUsed
        {
            get => DateTime.Parse(RawLastUsed);
            set
            {
            } //RawLastUsed = value.ToFirestoreTimestamp();
        }

        public int MaxValue => 0;// User.Experience.GetUserStamina(User.Player.Level);
        public int Value
        {
            get
            {
                TimeSpan leftTime = DateTime.Now - LastUsed;
                int value = (int)(LastValue + leftTime.TotalMinutes / 5);
                if (value > MaxValue) value = MaxValue;
                return value;
            }
        }

        public Stamina() { }
        public Stamina(int lastValue)
        {
            LastValue = lastValue;
            LastUsed = DateTime.Now;
        }

        public float GetPercentage() => (float)Value / MaxValue;

        public void Use(int amount = 1, bool executeImmediately = true, Action<bool> onResult = null)
        {
            int last = Value - amount;
            if (last < 0) last = 0;

            Stamina stamina = new Stamina(last);

            onResult += (success) =>
            {
                LastUsed = stamina.LastUsed;
                LastValue = stamina.LastValue;
                //ActionBarManager.Instance.UpdateStamina();
            };

            Save(stamina, executeImmediately, onResult);
        }

        public void Add(int amount, bool executeImmediately = true, Action<bool> onResult = null)
        {
            int last = Value + amount;
            if (last > MaxValue) last = MaxValue;

            Stamina stamina = new Stamina(last);

            onResult += (success) =>
            {
                LastUsed = stamina.LastUsed;
                LastValue = stamina.LastValue;
                //ActionBarManager.Instance.UpdateStamina();
            };

            Save(stamina, executeImmediately, onResult);
        }

        public TimeSpan GetTimeUntilFull()
        {
            int missingStamina = MaxValue - Value;
            int minutes = missingStamina * 5;
            return new TimeSpan(0, minutes, 0);
        }

        public DateTime GetFullTime()
        {
            return DateTime.Now + GetTimeUntilFull();
        }

        public void ResetStamina(bool executeImmediately = true, Action<bool> onResult = null)
        {
            LastValue = MaxValue;
            LastUsed = DateTime.Now;
            Save(this, executeImmediately, onResult);
            //ActionBarManager.Instance.UpdateStamina();
        }

        private void Save(Stamina stamina, bool executeImmediately, Action<bool> onResult)
        {
            //GNFirestore.UserDocument().UpdateField("Stamina", stamina, executeImmediately, onResult);
        }
    }
}