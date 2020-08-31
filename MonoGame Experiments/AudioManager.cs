using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaiFoxes.FMODAudio;
using ChaiFoxes.FMODAudio.Studio;

namespace MonoGame_Experiments
{
    class AudioManager
    {
        private static Dictionary<string, Bank> _banks = new Dictionary<string, Bank>();

        private static string _bankDirectory = "Build/Desktop/";
        public static Bank LoadBank(string bankName)
        {
            if (_banks.ContainsKey(bankName))
                return _banks[bankName];

            Bank newBank = StudioSystem.LoadBank(_bankDirectory + bankName);
            _banks.Add(bankName, newBank);
            return newBank;
        }
        
        public static void UnloadBank(string bankName)
        {
            if (_banks.ContainsKey(bankName))
                _banks.Remove(bankName);
        }

        public static void UnloadAllBanks()
        {
            foreach(var bankPair in _banks.ToArray())
            {
                bankPair.Value.Unload();
                _banks.Remove(bankPair.Key);
            }
        }

        public static EventDescription LoadEvent(string eventPath)
        {
            if (eventPath.StartsWith("event:/"))
                eventPath = eventPath.Remove(0, 7);
            return StudioSystem.GetEvent("event:/" + eventPath);
        }

        //Bank masterBank = StudioSystem.LoadBank("Build/Desktop/Master.bank", FMOD.Studio.LOAD_BANK_FLAGS.NORMAL);
        //Bank stringsBank = StudioSystem.LoadBank("Build/Desktop/Master.strings.bank", FMOD.Studio.LOAD_BANK_FLAGS.NORMAL);
        //EventDescription jingle = StudioSystem.GetEvent("event:/Test");
        //jingle.LoadSampleData();
        //    EventInstance eventInstance = jingle.CreateInstance();
        //eventInstance.Start();
    }
}
