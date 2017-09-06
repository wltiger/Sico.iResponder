using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sico.iResponder.App.Common
{
    public class Settings
    {
        private readonly Dictionary<Key, ParticipantKey> _participantKeyMappings;

        public Settings()
        {
            _participantKeyMappings = new Dictionary<Key, ParticipantKey>()
            {
                {Key.D1, new ParticipantKey() {ParticipantNo = 1, AnswerNo = 1}},
                {Key.D2, new ParticipantKey() {ParticipantNo = 1, AnswerNo = 2}},
                {Key.D3, new ParticipantKey() {ParticipantNo = 1, AnswerNo = 3}},
                {Key.D4, new ParticipantKey() {ParticipantNo = 1, AnswerNo = 4}},

                {Key.Q, new ParticipantKey() {ParticipantNo = 2, AnswerNo = 1}},
                {Key.W, new ParticipantKey() {ParticipantNo = 2, AnswerNo = 2}},
                {Key.E, new ParticipantKey() {ParticipantNo = 2, AnswerNo = 3}},
                {Key.R, new ParticipantKey() {ParticipantNo = 2, AnswerNo = 4}},

                {Key.A, new ParticipantKey() {ParticipantNo = 3, AnswerNo = 1}},
                {Key.S, new ParticipantKey() {ParticipantNo = 3, AnswerNo = 2}},
                {Key.D, new ParticipantKey() {ParticipantNo = 3, AnswerNo = 3}},
                {Key.F, new ParticipantKey() {ParticipantNo = 3, AnswerNo = 4}},

                {Key.Z, new ParticipantKey() {ParticipantNo = 4, AnswerNo = 1}},
                {Key.X, new ParticipantKey() {ParticipantNo = 4, AnswerNo = 2}},
                {Key.C, new ParticipantKey() {ParticipantNo = 4, AnswerNo = 3}},
                {Key.V, new ParticipantKey() {ParticipantNo = 4, AnswerNo = 4}},
            };
        }

        public IDictionary<Key, ParticipantKey> ParticipantKeyMappings => _participantKeyMappings;

        public int QuestionCountDown => 10;

        public int QuestionAnswerCountDown => 5;

        public int QuestionResultCountDown => 5;
    }
}
