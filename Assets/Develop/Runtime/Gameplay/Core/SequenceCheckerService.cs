using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Develop.Runtime.Gameplay.Core
{
    public class SequenceCheckerService
    {
        private readonly string _sequence;
        private int _currentIndex;

        public SequenceCheckerService(string sequence)
        {
            _sequence = sequence;
        }

        public SequenceCheckResult Check(char symbol)
        {
            if (symbol != _sequence[_currentIndex])
                return SequenceCheckResult.Failed;

            _currentIndex++;

            if (_currentIndex == _sequence.Length)
                return SequenceCheckResult.Completed;

            return SequenceCheckResult.Correct;
        }
    }
}
