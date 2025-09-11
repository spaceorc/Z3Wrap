namespace Z3Wrap;

public enum Z3Status
{
    Unsatisfiable = -1,  // Z3_L_FALSE
    Unknown = 0,         // Z3_L_UNDEF  
    Satisfiable = 1,     // Z3_L_TRUE
}