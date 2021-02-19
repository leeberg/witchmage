using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchMage
{
    public class StepSequence
    {
        public List<Step> steps;
        public Brand stepBranding;
        public List<StepInput> stepInputs;
        public List<Action> stepActions;
        public List<Evaluation> stepEvaluations;
    }
}
