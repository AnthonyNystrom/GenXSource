using System;

namespace GraphSynth
{
    /*Enumerator Declaration for Choice Method and what the resulting candidates are like */
    public enum choiceMethods { Design, Automatic };
    public enum candidatesAre { Unspecified, Developing, Feasible };
    public enum nextGenerationSteps
    {
        Unspecified = -5, Stop = -4, Loop = -3, GoToPrevious = -2, GoToNext = -1, GoToRuleSet0 = 0,
        GoToRuleSet1 = 1, GoToRuleSet2 = 2, GoToRuleSet3 = 3, GoToRuleSet4 = 4, GoToRuleSet5 = 5,
        GoToRuleSet6 = 6, GoToRuleSet7 = 7, GoToRuleSet8 = 8, GoToRuleSet9 = 9, GoToRuleSet10 = 10
    };
    /*Enumerator Declaration for How Generation Ended, GenerationStatus */
    public enum GenerationStatuses { Unspecified = -1, Normal, Choice, CycleLimit, NoRules, TriggerRule };

    /* this is for globalSettings and the *_Enter event for graph and grammar displays.*/
    public enum defaultLayoutAlg { None, SpringEmbedder, Tree, Random, Custom1, Custom2, Custom3 };

}
